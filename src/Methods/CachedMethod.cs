using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

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

    public CachedMethod(MethodInfo? methodInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        MethodInfo = methodInfo;
        _threadSafe = threadSafe;

        if (methodInfo == null)
            return;

        _cachedTypes = cachedTypes;

        _parameters = new Lazy<CachedParameters>(() => new CachedParameters(this, cachedTypes, threadSafe),
                                                 threadSafe);

        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, cachedTypes, threadSafe),
                                                       threadSafe);

        _genericMethodCache = new Lazy<IConstructedGenericCache>(
            () => threadSafe ? new ConcurrentConstructedGenericCache()
                             : new NonConcurrentConstructedGenericCache(),
            threadSafe
        );

        _invoker = new Lazy<Func<object?, object?[]?, object?>>(
            () => BuildInvoker(methodInfo),
            threadSafe
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedParameters? GetCachedParameters()
    {
        if (MethodInfo is null) return null;
        return _parameters!.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ParameterInfo[] GetParameters()
    {
        if (MethodInfo is null) return [];
        return _parameters!.Value.GetParameters();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedCustomAttributes? GetCachedCustomAttributes()
    {
        if (MethodInfo is null) return null;
        return _attributes!.Value;
    }

    public CachedMethod? MakeCachedGenericMethod(params CachedType[] cachedTypes)
    {
        if (MethodInfo is null) return null;

        // Compute both key and concrete Type[] in one pass to avoid multiple walks.
        var key = 17;
        var typeArr = new Type[cachedTypes.Length];

        for (var i = 0; i < cachedTypes.Length; i++)
        {
            CachedType ct = cachedTypes[i];
            Type t = ct.Type!;                // assuming ct.Type is non-null for generic args
            typeArr[i] = t;
            // simple order-sensitive rolling hash; must match your keying rules
            key = unchecked(key * 31 + t.GetHashCode());
        }

        IConstructedGenericCache cache = _genericMethodCache!.Value;

        if (cache.TryGet(key, out CachedMethod? found))
            return found;

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod(typeArr);
        var newCached = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);

        // Store (idempotent under contention)
        cache.SetIfAbsent(key, newCached);

        return newCached;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object[] GetCustomAttributes()
    {
        if (MethodInfo is null) return [];
        return _attributes!.Value.GetCustomAttributes();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance)
    {
        if (MethodInfo is null) return null;
        // Use compiled invoker; pass null args to avoid allocating empty array.
        return _invoker!.Value(instance, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object instance, params object[] param)
    {
        if (MethodInfo is null) return null;
        return _invoker!.Value(instance, param);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? Invoke<T>(object instance)
        => (T?)Invoke(instance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? Invoke<T>(params object[] param)
        => (T?)Invoke(null!, param);

    // -------- internals --------

    private static Func<object?, object?[]?, object?> BuildInvoker(MethodInfo mi)
    {
        // Build: (object? instance, object?[]? args) => (object?) <call>
        ParameterExpression instParam = Expression.Parameter(typeof(object), "instance");
        ParameterExpression argsParam = Expression.Parameter(typeof(object[]), "args");

        ParameterInfo[] parms = mi.GetParameters();
        var callArgs = new Expression[parms.Length];

        for (var i = 0; i < parms.Length; i++)
        {
            // args[i] == null is allowed for ref types/nullable; runtime will throw when invalid
            BinaryExpression index = Expression.ArrayIndex(argsParam, Expression.Constant(i));
            UnaryExpression cast = Expression.Convert(index, parms[i].ParameterType);
            callArgs[i] = cast;
        }

        Expression? instanceExpr = mi.IsStatic ? null : Expression.Convert(instParam, mi.DeclaringType!);
        Expression call = Expression.Call(instanceExpr, mi, callArgs);

        // Box return to object, or return null for void
        Expression body = mi.ReturnType == typeof(void)
            ? Expression.Block(call, Expression.Constant(null, typeof(object)))
            : Expression.Convert(call, typeof(object));

        // Handle zero-arg invocations by allowing args to be null
        if (parms.Length == 0)
        {
            // guard: args == null ? call() : call()
            body = Expression.Block(body); // nothing extra; CreateDelegate handles fine with null args
        }

        Expression<Func<object?, object?[]?, object?>> lambda = Expression.Lambda<Func<object?, object?[]?, object?>>(body, instParam, argsParam);
        return lambda.Compile(); // Tiered JIT will optimize quickly under load
    }

    /// Abstraction so we can use the fastest structure depending on thread-safety.
    private interface IConstructedGenericCache
    {
        bool TryGet(int key, out CachedMethod? value);
        void SetIfAbsent(int key, CachedMethod value);
    }

    // Non-thread-safe, minimal overhead
    private sealed class NonConcurrentConstructedGenericCache : IConstructedGenericCache
    {
        private readonly Dictionary<int, CachedMethod> _map = new(capacity: 4); // small seed
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(int key, out CachedMethod? value) => _map.TryGetValue(key, out value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetIfAbsent(int key, CachedMethod value)
        {
            _map.TryAdd(key, value);
        }
    }

    // Thread-safe without external locks
    private sealed class ConcurrentConstructedGenericCache : IConstructedGenericCache
    {
        private readonly ConcurrentDictionary<int, CachedMethod> _map = new(concurrencyLevel: Environment.ProcessorCount, capacity: 4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(int key, out CachedMethod? value) => _map.TryGetValue(key, out value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetIfAbsent(int key, CachedMethod value) => _map.TryAdd(key, value);
    }
}
