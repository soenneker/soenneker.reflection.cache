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

    private readonly Lazy<CachedParameters>? _parameters;
    private readonly Lazy<CachedCustomAttributes>? _attributes;

    // Thread-safe cache for constructed generic methods (only created if needed)
    private readonly Lazy<IConstructedGenericCache>? _genericMethodCache;

    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    // Fast, untyped invoker compiled once per method
    private readonly Lazy<Func<object?, object?[]?, object?>>? _invoker;

    // Arity-specialized invokers avoid params object[] allocations for common cases (1..4 args).
    private readonly Lazy<Func<object?, object?, object?>?>? _invoker1;
    private readonly Lazy<Func<object?, object?, object?, object?>?>? _invoker2;
    private readonly Lazy<Func<object?, object?, object?, object?, object?>?>? _invoker3;
    private readonly Lazy<Func<object?, object?, object?, object?, object?, object?>?>? _invoker4;

    public CachedMethod(MethodInfo? methodInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        MethodInfo = methodInfo;
        _threadSafe = threadSafe;

        if (methodInfo == null)
            return;

        _cachedTypes = cachedTypes;

        _parameters = new Lazy<CachedParameters>(() => new CachedParameters(this, cachedTypes, threadSafe), threadSafe);

        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, cachedTypes, threadSafe), threadSafe);

        _genericMethodCache = new Lazy<IConstructedGenericCache>(
            () => threadSafe ? new ConcurrentConstructedGenericCache() : new NonConcurrentConstructedGenericCache(), threadSafe);

        _invoker = new Lazy<Func<object?, object?[]?, object?>>(() => BuildSafeInvoker(methodInfo), threadSafe);
        _invoker1 = new Lazy<Func<object?, object?, object?>?>(() => BuildSafeInvoker1(methodInfo), threadSafe);
        _invoker2 = new Lazy<Func<object?, object?, object?, object?>?>(() => BuildSafeInvoker2(methodInfo), threadSafe);
        _invoker3 = new Lazy<Func<object?, object?, object?, object?, object?>?>(() => BuildSafeInvoker3(methodInfo), threadSafe);
        _invoker4 = new Lazy<Func<object?, object?, object?, object?, object?, object?>?>(() => BuildSafeInvoker4(methodInfo), threadSafe);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedParameters? GetCachedParameters()
    {
        if (MethodInfo is null)
            return null;

        return _parameters!.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ParameterInfo[] GetParameters()
    {
        if (MethodInfo is null)
            return [];

        return _parameters!.Value.GetParameters();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedCustomAttributes? GetCachedCustomAttributes()
    {
        if (MethodInfo is null)
            return null;

        return _attributes!.Value;
    }

    public CachedMethod? MakeCachedGenericMethod(params CachedType[] cachedTypes)
    {
        if (MethodInfo is null)
            return null;

        var typeArr = new Type[cachedTypes.Length];
        TypeHandleSequenceKey key = TypeHandleSequenceKey.FromCachedTypes(cachedTypes, typeArr);

        IConstructedGenericCache cache = _genericMethodCache!.Value;

        if (cache.TryGet(key, out CachedMethod? found))
            return found;

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

        IConstructedGenericCache cache = _genericMethodCache!.Value;
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

        IConstructedGenericCache cache = _genericMethodCache!.Value;
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

        IConstructedGenericCache cache = _genericMethodCache!.Value;
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

        IConstructedGenericCache cache = _genericMethodCache!.Value;
        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod([type0, type1, type2, type3]);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        cache.SetIfAbsent(key, newCached);
        return newCached;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object[] GetCustomAttributes()
    {
        if (MethodInfo is null)
            return [];
        return _attributes!.Value.GetCustomAttributes();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance)
    {
        if (MethodInfo is null)
            return null;
        // Use compiled invoker; pass null args to avoid allocating empty array.
        return _invoker!.Value(instance, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, params object[] param)
    {
        if (MethodInfo is null)
            return null;
        return _invoker!.Value(instance, param);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, object? arg0)
    {
        if (MethodInfo is null)
            return null;

        Func<object?, object?, object?>? f = _invoker1!.Value;
        if (f is not null)
            return f(instance, arg0);

        return _invoker!.Value(instance, [arg0]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, object? arg0, object? arg1)
    {
        if (MethodInfo is null)
            return null;

        Func<object?, object?, object?, object?>? f = _invoker2!.Value;
        if (f is not null)
            return f(instance, arg0, arg1);

        return _invoker!.Value(instance, [arg0, arg1]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, object? arg0, object? arg1, object? arg2)
    {
        if (MethodInfo is null)
            return null;

        Func<object?, object?, object?, object?, object?>? f = _invoker3!.Value;
        if (f is not null)
            return f(instance, arg0, arg1, arg2);

        return _invoker!.Value(instance, [arg0, arg1, arg2]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, object? arg0, object? arg1, object? arg2, object? arg3)
    {
        if (MethodInfo is null)
            return null;

        Func<object?, object?, object?, object?, object?, object?>? f = _invoker4!.Value;
        if (f is not null)
            return f(instance, arg0, arg1, arg2, arg3);

        return _invoker!.Value(instance, [arg0, arg1, arg2, arg3]);
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