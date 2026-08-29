using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Methods;

/// <inheritdoc cref="ICachedMethod"/>
public sealed class CachedMethod : ICachedMethod
{
    public MethodInfo? MethodInfo { get; }

    public string? Name => MethodInfo?.Name;

    public Type? ReturnType => MethodInfo?.ReturnType;

    private ValueLazy<CachedParameters> _parameters;
    private ValueLazy<ParameterInfo[]> _parameterInfos;
    private ValueLazy<CachedCustomAttributes> _attributes;

    // Thread-safe cache for constructed generic methods (only created if needed)
    private ValueLazy<IConstructedGenericCache> _genericMethodCache;

    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    // Fast, untyped invoker compiled once per method
    private ValueLazy<Func<object?, object?[]?, object?>> _invoker;

    // Arity-specialized invokers avoid params object[] allocations for common cases (1..4 args).
    private static readonly object _unsupportedInvoker = new();
    private ValueLazy<object> _invoker1;
    private ValueLazy<object> _invoker2;
    private ValueLazy<object> _invoker3;
    private ValueLazy<object> _invoker4;
    private ValueAtomicLock _initializationLock;

    // Thread-static exact-length arrays for fallback invocations (reflection requires exact parameter count).
    [ThreadStatic] private static object?[]? _tsArgs1;
    [ThreadStatic] private static object?[]? _tsArgs2;
    [ThreadStatic] private static object?[]? _tsArgs3;
    [ThreadStatic] private static object?[]? _tsArgs4;

    public CachedMethod(MethodInfo? methodInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        MethodInfo = methodInfo;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedParameters GetParametersCache() =>
        _parameters.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedParameters(self.GetParameterInfos(), self._cachedTypes));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ParameterInfo[] GetParameterInfos() =>
        _parameterInfos.GetOrCreate(_threadSafe, ref _initializationLock, MethodInfo, static methodInfo => methodInfo?.GetParameters() ?? []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedCustomAttributes GetAttributesCache() =>
        _attributes.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedCustomAttributes(self, self._cachedTypes, self._threadSafe));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IConstructedGenericCache GetGenericMethodCache() =>
        _genericMethodCache.GetOrCreate(_threadSafe, ref _initializationLock, _threadSafe,
            static threadSafe => threadSafe ? new ConcurrentConstructedGenericCache() : new NonConcurrentConstructedGenericCache());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Func<object?, object?[]?, object?> GetInvoker() =>
        _invoker.GetOrCreate(_threadSafe, ref _initializationLock, MethodInfo!, static methodInfo => BuildSafeInvoker(methodInfo));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Func<object?, object?, object?>? GetInvoker1()
    {
        object value = _invoker1.GetOrCreate(_threadSafe, ref _initializationLock, MethodInfo!,
            static methodInfo => BuildSafeInvoker1(methodInfo) ?? _unsupportedInvoker);
        return ReferenceEquals(value, _unsupportedInvoker) ? null : (Func<object?, object?, object?>)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Func<object?, object?, object?, object?>? GetInvoker2()
    {
        object value = _invoker2.GetOrCreate(_threadSafe, ref _initializationLock, MethodInfo!,
            static methodInfo => BuildSafeInvoker2(methodInfo) ?? _unsupportedInvoker);
        return ReferenceEquals(value, _unsupportedInvoker) ? null : (Func<object?, object?, object?, object?>)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Func<object?, object?, object?, object?, object?>? GetInvoker3()
    {
        object value = _invoker3.GetOrCreate(_threadSafe, ref _initializationLock, MethodInfo!,
            static methodInfo => BuildSafeInvoker3(methodInfo) ?? _unsupportedInvoker);
        return ReferenceEquals(value, _unsupportedInvoker) ? null : (Func<object?, object?, object?, object?, object?>)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Func<object?, object?, object?, object?, object?, object?>? GetInvoker4()
    {
        object value = _invoker4.GetOrCreate(_threadSafe, ref _initializationLock, MethodInfo!,
            static methodInfo => BuildSafeInvoker4(methodInfo) ?? _unsupportedInvoker);
        return ReferenceEquals(value, _unsupportedInvoker) ? null : (Func<object?, object?, object?, object?, object?, object?>)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedParameters? GetCachedParameters()
    {
        if (MethodInfo is null)
            return null;

        return GetParametersCache();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ParameterInfo[] GetParameters()
    {
        if (MethodInfo is null)
            return [];

        return GetParameterInfos();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedCustomAttributes? GetCachedCustomAttributes()
    {
        if (MethodInfo is null)
            return null;

        return GetAttributesCache();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute
    {
        if (MethodInfo is null)
            return null;

        return GetAttributesCache().GetCachedCustomAttribute<T>(inherit);
    }

    public CachedMethod? MakeCachedGenericMethod(params CachedType[] cachedTypes)
    {
        if (MethodInfo is null)
            return null;

        if (cachedTypes is null)
            throw new ArgumentNullException(nameof(cachedTypes));

        int len = cachedTypes.Length;

        if (len == 0)
            return MethodInfo.IsGenericMethodDefinition ? null : this;

        // Probe cache without allocating/filling a Type[] (allocate only on miss)
        TypeHandleSequenceKey key = TypeHandleSequenceKey.FromCachedTypes(cachedTypes);

        IConstructedGenericCache cache = GetGenericMethodCache();

        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        var typeArr = new Type[len];
        for (var i = 0; i < len; i++)
            typeArr[i] = cachedTypes[i].Type!;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod(typeArr);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);

        // Store (idempotent under contention)
        cache.SetIfAbsent(key, newCached);

        return newCached;
    }

    // ---- allocation-reducing overloads (avoid params CachedType[] allocations) ----

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? MakeCachedGenericMethod(CachedType t0)
    {
        if (MethodInfo is null)
            return null;

        Type type0 = t0.Type!;
        TypeHandleSequenceKey key = TypeHandleSequenceKey.From1(type0.TypeHandle);

        IConstructedGenericCache cache = GetGenericMethodCache();
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([type0]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1)
    {
        if (MethodInfo is null)
            return null;

        Type type0 = t0.Type!;
        Type type1 = t1.Type!;
        TypeHandleSequenceKey key = TypeHandleSequenceKey.From2(type0.TypeHandle, type1.TypeHandle);

        IConstructedGenericCache cache = GetGenericMethodCache();
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([type0, type1]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1, CachedType t2)
    {
        if (MethodInfo is null)
            return null;

        Type type0 = t0.Type!;
        Type type1 = t1.Type!;
        Type type2 = t2.Type!;
        TypeHandleSequenceKey key = TypeHandleSequenceKey.From3(type0.TypeHandle, type1.TypeHandle, type2.TypeHandle);

        IConstructedGenericCache cache = GetGenericMethodCache();
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([type0, type1, type2]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1, CachedType t2, CachedType t3)
    {
        if (MethodInfo is null)
            return null;

        Type type0 = t0.Type!;
        Type type1 = t1.Type!;
        Type type2 = t2.Type!;
        Type type3 = t3.Type!;
        TypeHandleSequenceKey key = TypeHandleSequenceKey.From4(type0.TypeHandle, type1.TypeHandle, type2.TypeHandle, type3.TypeHandle);

        IConstructedGenericCache cache = GetGenericMethodCache();
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([type0, type1, type2, type3]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    // ---- allocation-reducing overloads (avoid CachedType wrapper/params allocations) ----

    /// <summary>
    /// Creates cached Generic Method.
    /// </summary>
    /// <param name="t0">First generic type argument.</param>
    /// <returns>The same builder instance, so additional classes or variants can be chained.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? MakeCachedGenericMethod(Type t0)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));

        if (MethodInfo is null)
            return null;

        TypeHandleSequenceKey key = TypeHandleSequenceKey.From1(t0.TypeHandle);

        IConstructedGenericCache cache = GetGenericMethodCache();
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([t0]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    /// <summary>
    /// Creates cached Generic Method.
    /// </summary>
    /// <param name="t0">First generic type argument.</param>
    /// <param name="t1">Second generic type argument.</param>
    /// <returns>The same builder instance, so additional classes or variants can be chained.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? MakeCachedGenericMethod(Type t0, Type t1)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));

        if (MethodInfo is null)
            return null;

        TypeHandleSequenceKey key = TypeHandleSequenceKey.From2(t0.TypeHandle, t1.TypeHandle);

        IConstructedGenericCache cache = GetGenericMethodCache();
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([t0, t1]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    /// <summary>
    /// Creates cached Generic Method.
    /// </summary>
    /// <param name="t0">First generic type argument.</param>
    /// <param name="t1">Second generic type argument.</param>
    /// <param name="t2">Third generic type argument.</param>
    /// <returns>The same builder instance, so additional classes or variants can be chained.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? MakeCachedGenericMethod(Type t0, Type t1, Type t2)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));
        if (t2 is null)
            throw new ArgumentNullException(nameof(t2));

        if (MethodInfo is null)
            return null;

        TypeHandleSequenceKey key = TypeHandleSequenceKey.From3(t0.TypeHandle, t1.TypeHandle, t2.TypeHandle);

        IConstructedGenericCache cache = GetGenericMethodCache();
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([t0, t1, t2]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    /// <summary>
    /// Creates cached Generic Method.
    /// </summary>
    /// <param name="t0">First generic type argument.</param>
    /// <param name="t1">Second generic type argument.</param>
    /// <param name="t2">Third generic type argument.</param>
    /// <param name="t3">T for the make cached generic method operation.</param>
    /// <returns>The same builder instance, so additional classes or variants can be chained.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? MakeCachedGenericMethod(Type t0, Type t1, Type t2, Type t3)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));
        if (t2 is null)
            throw new ArgumentNullException(nameof(t2));
        if (t3 is null)
            throw new ArgumentNullException(nameof(t3));

        if (MethodInfo is null)
            return null;

        TypeHandleSequenceKey key = TypeHandleSequenceKey.From4(t0.TypeHandle, t1.TypeHandle, t2.TypeHandle, t3.TypeHandle);

        IConstructedGenericCache cache = GetGenericMethodCache();
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([t0, t1, t2, t3]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object[] GetCustomAttributes()
    {
        if (MethodInfo is null)
            return [];
        return GetAttributesCache().GetCustomAttributes();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance)
    {
        if (MethodInfo is null)
            return null;
        // Use compiled invoker; pass null args to avoid allocating empty array.
        return GetInvoker()(instance, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, params object[] param)
    {
        if (MethodInfo is null)
            return null;
        if (param.Length == 0)
            return GetInvoker()(instance, null);
        return GetInvoker()(instance, param);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? InvokeThreadStatic(Func<object?, object?[]?, object?> invoker, object? instance, object? arg0)
    {
        object?[] arr = _tsArgs1 ??= new object?[1];
        _tsArgs1 = null;
        try
        {
            arr[0] = arg0;
            return invoker(instance, arr);
        }
        finally
        {
            arr[0] = null;
            _tsArgs1 ??= arr;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? InvokeThreadStatic(Func<object?, object?[]?, object?> invoker, object? instance, object? arg0, object? arg1)
    {
        object?[] arr = _tsArgs2 ??= new object?[2];
        _tsArgs2 = null;
        try
        {
            arr[0] = arg0;
            arr[1] = arg1;
            return invoker(instance, arr);
        }
        finally
        {
            arr[0] = null;
            arr[1] = null;
            _tsArgs2 ??= arr;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? InvokeThreadStatic(Func<object?, object?[]?, object?> invoker, object? instance, object? arg0, object? arg1, object? arg2)
    {
        object?[] arr = _tsArgs3 ??= new object?[3];
        _tsArgs3 = null;
        try
        {
            arr[0] = arg0;
            arr[1] = arg1;
            arr[2] = arg2;
            return invoker(instance, arr);
        }
        finally
        {
            arr[0] = null;
            arr[1] = null;
            arr[2] = null;
            _tsArgs3 ??= arr;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? InvokeThreadStatic(Func<object?, object?[]?, object?> invoker, object? instance, object? arg0, object? arg1, object? arg2, object? arg3)
    {
        object?[] arr = _tsArgs4 ??= new object?[4];
        _tsArgs4 = null;
        try
        {
            arr[0] = arg0;
            arr[1] = arg1;
            arr[2] = arg2;
            arr[3] = arg3;
            return invoker(instance, arr);
        }
        finally
        {
            arr[0] = null;
            arr[1] = null;
            arr[2] = null;
            arr[3] = null;
            _tsArgs4 ??= arr;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, object? arg0)
    {
        if (MethodInfo is null)
            return null;

        Func<object?, object?, object?>? f = GetInvoker1();
        if (f is not null)
            return f(instance, arg0);

        return InvokeThreadStatic(GetInvoker(), instance, arg0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, object? arg0, object? arg1)
    {
        if (MethodInfo is null)
            return null;

        Func<object?, object?, object?, object?>? f = GetInvoker2();
        if (f is not null)
            return f(instance, arg0, arg1);

        return InvokeThreadStatic(GetInvoker(), instance, arg0, arg1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, object? arg0, object? arg1, object? arg2)
    {
        if (MethodInfo is null)
            return null;

        Func<object?, object?, object?, object?, object?>? f = GetInvoker3();
        if (f is not null)
            return f(instance, arg0, arg1, arg2);

        return InvokeThreadStatic(GetInvoker(), instance, arg0, arg1, arg2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, object? arg0, object? arg1, object? arg2, object? arg3)
    {
        if (MethodInfo is null)
            return null;

        Func<object?, object?, object?, object?, object?, object?>? f = GetInvoker4();
        if (f is not null)
            return f(instance, arg0, arg1, arg2, arg3);

        return InvokeThreadStatic(GetInvoker(), instance, arg0, arg1, arg2, arg3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? Invoke<T>(object instance) => (T?)Invoke(instance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? Invoke<T>(params object[] param) => (T?)Invoke(null!, param);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? Invoke<T>(object instance, object? arg0) => (T?)Invoke(instance, arg0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? Invoke<T>(object instance, object? arg0, object? arg1) => (T?)Invoke(instance, arg0, arg1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? Invoke<T>(object instance, object? arg0, object? arg1, object? arg2) => (T?)Invoke(instance, arg0, arg1, arg2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? Invoke<T>(object instance, object? arg0, object? arg1, object? arg2, object? arg3) => (T?)Invoke(instance, arg0, arg1, arg2, arg3);

    // -------- internals --------

    private static Func<object?, object?[]?, object?> BuildSafeInvoker(MethodInfo mi)
    {
        // Fallback to MethodInfo.Invoke for byref/byref-like signatures or on any compile failure
        ParameterInfo[] parmsProbe = mi.GetParameters();
        for (var i = 0; i < parmsProbe.Length; i++)
        {
            Type pt = parmsProbe[i].ParameterType;
            if (pt.IsByRef)
            {
                return (instance, args) => mi.Invoke(instance, args ?? Array.Empty<object?>());
            }

            // .NET doesn't expose IsByRefLike directly pre .NET 7 on Type, but common cases are Span/ReadOnlySpan
            if (pt.FullName is not null && (pt.FullName.StartsWith("System.Span`1", StringComparison.Ordinal) ||
                                            pt.FullName.StartsWith("System.ReadOnlySpan`1", StringComparison.Ordinal)))
            {
                return (instance, args) => mi.Invoke(instance, args ?? Array.Empty<object?>());
            }
        }

        // Build: (object? instance, object?[]? args) => (object?) <call>
        ParameterExpression instParam = Expression.Parameter(typeof(object), "instance");
        ParameterExpression argsParam = Expression.Parameter(typeof(object[]), "args");

        var callArgs = new Expression[parmsProbe.Length];

        for (var i = 0; i < parmsProbe.Length; i++)
        {
            // args[i] == null is allowed for ref types/nullable; runtime will throw when invalid
            BinaryExpression index = Expression.ArrayIndex(argsParam, Expression.Constant(i));
            UnaryExpression cast = Expression.Convert(index, parmsProbe[i].ParameterType);
            callArgs[i] = cast;
        }

        Expression? instanceExpr = mi.IsStatic ? null : Expression.Convert(instParam, mi.DeclaringType!);
        Expression call = Expression.Call(instanceExpr, mi, callArgs);

        // Box return to object, or return null for void
        Expression body = mi.ReturnType == typeof(void)
            ? Expression.Block(call, Expression.Constant(null, typeof(object)))
            : Expression.Convert(call, typeof(object));

        // Handle zero-arg invocations by allowing args to be null
        if (parmsProbe.Length == 0)
        {
            // guard: args == null ? call() : call()
            body = Expression.Block(body); // nothing extra; CreateDelegate handles fine with null args
        }

        try
        {
            Expression<Func<object?, object?[]?, object?>> lambda = Expression.Lambda<Func<object?, object?[]?, object?>>(body, instParam, argsParam);
            return lambda.Compile(); // Tiered JIT will optimize quickly under load
        }
        catch
        {
            // Safe fallback
            return (instance, args) => mi.Invoke(instance, args ?? Array.Empty<object?>());
        }
    }

    private static bool IsByRefLikeOrByRef(Type t)
    {
        if (t.IsByRef)
            return true;

        string? fullName = t.FullName;
        return fullName is not null && (fullName.StartsWith("System.Span`1", StringComparison.Ordinal) ||
                                        fullName.StartsWith("System.ReadOnlySpan`1", StringComparison.Ordinal));
    }

    private static bool CanUseFastInvoker(MethodInfo mi, out ParameterInfo[] parameters)
    {
        parameters = mi.GetParameters();
        for (var i = 0; i < parameters.Length; i++)
        {
            if (IsByRefLikeOrByRef(parameters[i].ParameterType))
                return false;
        }

        return true;
    }

    private static Func<object?, object?, object?>? BuildSafeInvoker1(MethodInfo mi)
    {
        try
        {
            if (!CanUseFastInvoker(mi, out ParameterInfo[] ps) || ps.Length != 1)
                return null;

            ParameterExpression instParam = Expression.Parameter(typeof(object), "instance");
            ParameterExpression a0 = Expression.Parameter(typeof(object), "a0");

            Expression? instanceExpr = mi.IsStatic ? null : Expression.Convert(instParam, mi.DeclaringType!);
            UnaryExpression arg0 = Expression.Convert(a0, ps[0].ParameterType);
            Expression call = Expression.Call(instanceExpr, mi, arg0);

            Expression body = mi.ReturnType == typeof(void)
                ? Expression.Block(call, Expression.Constant(null, typeof(object)))
                : Expression.Convert(call, typeof(object));

            return Expression.Lambda<Func<object?, object?, object?>>(body, instParam, a0).Compile();
        }
        catch
        {
            return null;
        }
    }

    private static Func<object?, object?, object?, object?>? BuildSafeInvoker2(MethodInfo mi)
    {
        try
        {
            if (!CanUseFastInvoker(mi, out ParameterInfo[] ps) || ps.Length != 2)
                return null;

            ParameterExpression instParam = Expression.Parameter(typeof(object), "instance");
            ParameterExpression a0 = Expression.Parameter(typeof(object), "a0");
            ParameterExpression a1 = Expression.Parameter(typeof(object), "a1");

            Expression? instanceExpr = mi.IsStatic ? null : Expression.Convert(instParam, mi.DeclaringType!);
            UnaryExpression arg0 = Expression.Convert(a0, ps[0].ParameterType);
            UnaryExpression arg1 = Expression.Convert(a1, ps[1].ParameterType);
            Expression call = Expression.Call(instanceExpr, mi, arg0, arg1);

            Expression body = mi.ReturnType == typeof(void)
                ? Expression.Block(call, Expression.Constant(null, typeof(object)))
                : Expression.Convert(call, typeof(object));

            return Expression.Lambda<Func<object?, object?, object?, object?>>(body, instParam, a0, a1).Compile();
        }
        catch
        {
            return null;
        }
    }

    private static Func<object?, object?, object?, object?, object?>? BuildSafeInvoker3(MethodInfo mi)
    {
        try
        {
            if (!CanUseFastInvoker(mi, out ParameterInfo[] ps) || ps.Length != 3)
                return null;

            ParameterExpression instParam = Expression.Parameter(typeof(object), "instance");
            ParameterExpression a0 = Expression.Parameter(typeof(object), "a0");
            ParameterExpression a1 = Expression.Parameter(typeof(object), "a1");
            ParameterExpression a2 = Expression.Parameter(typeof(object), "a2");

            Expression? instanceExpr = mi.IsStatic ? null : Expression.Convert(instParam, mi.DeclaringType!);
            UnaryExpression arg0 = Expression.Convert(a0, ps[0].ParameterType);
            UnaryExpression arg1 = Expression.Convert(a1, ps[1].ParameterType);
            UnaryExpression arg2 = Expression.Convert(a2, ps[2].ParameterType);
            Expression call = Expression.Call(instanceExpr, mi, arg0, arg1, arg2);

            Expression body = mi.ReturnType == typeof(void)
                ? Expression.Block(call, Expression.Constant(null, typeof(object)))
                : Expression.Convert(call, typeof(object));

            return Expression.Lambda<Func<object?, object?, object?, object?, object?>>(body, instParam, a0, a1, a2).Compile();
        }
        catch
        {
            return null;
        }
    }

    private static Func<object?, object?, object?, object?, object?, object?>? BuildSafeInvoker4(MethodInfo mi)
    {
        try
        {
            if (!CanUseFastInvoker(mi, out ParameterInfo[] ps) || ps.Length != 4)
                return null;

            ParameterExpression instParam = Expression.Parameter(typeof(object), "instance");
            ParameterExpression a0 = Expression.Parameter(typeof(object), "a0");
            ParameterExpression a1 = Expression.Parameter(typeof(object), "a1");
            ParameterExpression a2 = Expression.Parameter(typeof(object), "a2");
            ParameterExpression a3 = Expression.Parameter(typeof(object), "a3");

            Expression? instanceExpr = mi.IsStatic ? null : Expression.Convert(instParam, mi.DeclaringType!);
            UnaryExpression arg0 = Expression.Convert(a0, ps[0].ParameterType);
            UnaryExpression arg1 = Expression.Convert(a1, ps[1].ParameterType);
            UnaryExpression arg2 = Expression.Convert(a2, ps[2].ParameterType);
            UnaryExpression arg3 = Expression.Convert(a3, ps[3].ParameterType);
            Expression call = Expression.Call(instanceExpr, mi, arg0, arg1, arg2, arg3);

            Expression body = mi.ReturnType == typeof(void)
                ? Expression.Block(call, Expression.Constant(null, typeof(object)))
                : Expression.Convert(call, typeof(object));

            return Expression.Lambda<Func<object?, object?, object?, object?, object?, object?>>(body, instParam, a0, a1, a2, a3).Compile();
        }
        catch
        {
            return null;
        }
    }

}
