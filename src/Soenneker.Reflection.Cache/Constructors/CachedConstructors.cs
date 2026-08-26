using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Reflection.Cache.Utils;
using System;
using System.Buffers;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Soenneker.Reflection.Cache.Constructors;

/// <inheritdoc cref="ICachedConstructors"/>
public sealed class CachedConstructors : ICachedConstructors
{
    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;
    private ValueAtomicLock _sync;

    private ValueLazy<ConstructorInfo[]> _constructorInfos;
    private ValueLazy<CachedConstructor[]> _cachedArray;

    // Fast path for parameterless construction (if available)
    private ValueLazy<Func<object?>> _parameterlessActivator;

    public CachedConstructors(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ConstructorInfo[] GetConstructorInfos() =>
        _constructorInfos.GetOrCreate(_threadSafe, ref _sync, this,
            static self => self._cachedType.Type!.GetConstructors(self._cachedTypes.Options.ConstructorFlags));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedConstructor[] GetCachedConstructorArray() =>
        _cachedArray.GetOrCreate(_threadSafe, ref _sync, this,
            static self => self.BuildCachedConstructorArray(self.GetConstructorInfos()));

    private CachedConstructor[] BuildCachedConstructorArray(ConstructorInfo[] infos)
    {
        var result = new CachedConstructor[infos.Length];

        for (var i = 0; i < infos.Length; i++)
            result[i] = new CachedConstructor(infos[i], _cachedTypes, _threadSafe);

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Func<object?> GetParameterlessActivator() =>
        _parameterlessActivator.GetOrCreate(_threadSafe, ref _sync, this, static self => self.BuildParameterlessActivator());

    private Func<object?> BuildParameterlessActivator()
    {
        Type type = _cachedType.Type!;
        ConstructorInfo? ctor = type.GetConstructor(Type.EmptyTypes);
        if (ctor is null)
            return static () => null;

        try
        {
            NewExpression newExpr = Expression.New(ctor);
            Expression<Func<object?>> lambda = Expression.Lambda<Func<object?>>(Expression.Convert(newExpr, typeof(object)));
            return lambda.Compile();
        }
        catch
        {
            return () => Activator.CreateInstance(type);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedConstructor? GetCachedConstructor(Type[]? parameterTypes = null)
    {
        if (parameterTypes == null)
            return GetCachedConstructor(Array.Empty<Type>(), 0);

        return GetCachedConstructor(parameterTypes, parameterTypes.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedConstructor? GetCachedConstructor(Type[] parameterTypes, int length)
    {
        if (length == 0)
        {
            CachedConstructor[] ctors = GetCachedConstructorArray();
            for (var i = 0; i < ctors.Length; i++)
            {
                if (ctors[i].ConstructorInfo?.IsStatic == false && ctors[i].GetParameters().Length == 0)
                    return ctors[i];
            }

            return null;
        }

        CachedConstructor[] candidates = GetCachedConstructorArray();

        for (var i = 0; i < candidates.Length; i++)
        {
            if (candidates[i].ConstructorInfo?.IsStatic != false)
                continue;

            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != length)
                continue;

            var match = true;
            for (var j = 0; j < length; j++)
            {
                if (!ReferenceEquals(ps[j].ParameterType, parameterTypes[j]))
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedConstructor? GetCachedConstructor(Type t0)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));

        CachedConstructor[] candidates = GetCachedConstructorArray();

        for (var i = 0; i < candidates.Length; i++)
        {
            if (candidates[i].ConstructorInfo?.IsStatic != false)
                continue;

            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != 1)
                continue;

            if (ReferenceEquals(ps[0].ParameterType, t0))
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));

        CachedConstructor[] candidates = GetCachedConstructorArray();

        for (var i = 0; i < candidates.Length; i++)
        {
            if (candidates[i].ConstructorInfo?.IsStatic != false)
                continue;

            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != 2)
                continue;

            if (ReferenceEquals(ps[0].ParameterType, t0) && ReferenceEquals(ps[1].ParameterType, t1))
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));
        if (t2 is null)
            throw new ArgumentNullException(nameof(t2));

        CachedConstructor[] candidates = GetCachedConstructorArray();

        for (var i = 0; i < candidates.Length; i++)
        {
            if (candidates[i].ConstructorInfo?.IsStatic != false)
                continue;

            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != 3)
                continue;

            if (ReferenceEquals(ps[0].ParameterType, t0) && ReferenceEquals(ps[1].ParameterType, t1) && ReferenceEquals(ps[2].ParameterType, t2))
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2, Type t3)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));
        if (t2 is null)
            throw new ArgumentNullException(nameof(t2));
        if (t3 is null)
            throw new ArgumentNullException(nameof(t3));

        CachedConstructor[] candidates = GetCachedConstructorArray();

        for (var i = 0; i < candidates.Length; i++)
        {
            if (candidates[i].ConstructorInfo?.IsStatic != false)
                continue;

            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != 4)
                continue;

            if (ReferenceEquals(ps[0].ParameterType, t0) && ReferenceEquals(ps[1].ParameterType, t1) && ReferenceEquals(ps[2].ParameterType, t2) &&
                ReferenceEquals(ps[3].ParameterType, t3))
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null) => GetCachedConstructor(parameterTypes)?.ConstructorInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConstructorInfo? GetConstructor(Type t0) => GetCachedConstructor(t0)?.ConstructorInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConstructorInfo? GetConstructor(Type t0, Type t1) => GetCachedConstructor(t0, t1)?.ConstructorInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2) => GetCachedConstructor(t0, t1, t2)?.ConstructorInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2, Type t3) => GetCachedConstructor(t0, t1, t2, t3)?.ConstructorInfo;

    public CachedConstructor[] GetCachedConstructors() => GetCachedConstructorArray();

    public ConstructorInfo?[] GetConstructors() => GetConstructorInfos();

    public object? CreateInstance()
    {
        // Use the cached parameterless activator if present; otherwise null if no default ctor.
        Func<object?> f = GetParameterlessActivator();
        return f();
    }

    public T? CreateInstance<T>()
    {
        object? obj = CreateInstance();
        return obj is null ? default : (T?) obj;
    }

    public object? CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
            return CreateInstance();

        // For common arities, avoid allocating Type[] and avoid params-object[] fallbacks.
        // Treat null arguments as typeof(object) (preserves current behavior of ToTypes()).
        switch (parameters.Length)
        {
            case 1:
            {
                object? a0 = parameters[0];
                CachedConstructor? ctor = GetCachedConstructor(a0?.GetType() ?? typeof(object));
                return ctor?.Invoke(a0);
            }
            case 2:
            {
                object? a0 = parameters[0];
                object? a1 = parameters[1];
                CachedConstructor? ctor = GetCachedConstructor(a0?.GetType() ?? typeof(object), a1?.GetType() ?? typeof(object));
                return ctor?.Invoke(a0, a1);
            }
            case 3:
            {
                object? a0 = parameters[0];
                object? a1 = parameters[1];
                object? a2 = parameters[2];
                CachedConstructor? ctor = GetCachedConstructor(a0?.GetType() ?? typeof(object), a1?.GetType() ?? typeof(object), a2?.GetType() ?? typeof(object));
                return ctor?.Invoke(a0, a1, a2);
            }
            case 4:
            {
                object? a0 = parameters[0];
                object? a1 = parameters[1];
                object? a2 = parameters[2];
                object? a3 = parameters[3];
                CachedConstructor? ctor = GetCachedConstructor(a0?.GetType() ?? typeof(object), a1?.GetType() ?? typeof(object), a2?.GetType() ?? typeof(object),
                    a3?.GetType() ?? typeof(object));
                return ctor?.Invoke(a0, a1, a2, a3);
            }
        }

        int length = parameters.Length;

        // For larger arities, avoid allocating a Type[] by renting and passing an explicit length.
        Type[] rented = ArrayPool<Type>.Shared.Rent(length);
        try
        {
            for (var i = 0; i < length; i++)
                rented[i] = parameters[i]?.GetType() ?? typeof(object);

            CachedConstructor? cachedConstructor = GetCachedConstructor(rented, length);
            return cachedConstructor?.Invoke(parameters);
        }
        finally
        {
            // Avoid clearing the entire rented buffer (which may be larger than 'length').
            // Clear only the used segment to prevent retaining Type references.
            Array.Clear(rented, 0, length);
            ArrayPool<Type>.Shared.Return(rented, clearArray: false);
        }
    }

    public object? CreateInstance(object? arg0)
    {
        if (arg0 is null)
            return CreateInstance([arg0!]);

        CachedConstructor? ctor = GetCachedConstructor(arg0.GetType());
        return ctor?.Invoke(arg0);
    }

    public object? CreateInstance(object? arg0, object? arg1)
    {
        if (arg0 is null || arg1 is null)
            return CreateInstance([arg0!, arg1!]);

        CachedConstructor? ctor = GetCachedConstructor(arg0.GetType(), arg1.GetType());
        return ctor?.Invoke(arg0, arg1);
    }

    public object? CreateInstance(object? arg0, object? arg1, object? arg2)
    {
        if (arg0 is null || arg1 is null || arg2 is null)
            return CreateInstance([arg0!, arg1!, arg2!]);

        CachedConstructor? ctor = GetCachedConstructor(arg0.GetType(), arg1.GetType(), arg2.GetType());
        return ctor?.Invoke(arg0, arg1, arg2);
    }

    public object? CreateInstance(object? arg0, object? arg1, object? arg2, object? arg3)
    {
        if (arg0 is null || arg1 is null || arg2 is null || arg3 is null)
            return CreateInstance([arg0!, arg1!, arg2!, arg3!]);

        CachedConstructor? ctor = GetCachedConstructor(arg0.GetType(), arg1.GetType(), arg2.GetType(), arg3.GetType());
        return ctor?.Invoke(arg0, arg1, arg2, arg3);
    }

    public T? CreateInstance<T>(params object[] parameters)
    {
        if (parameters.Length == 0)
            return CreateInstance<T>();

        object? obj = CreateInstance(parameters);
        return obj is null ? default : (T?) obj;
    }

    public T? CreateInstance<T>(object? arg0) => (T?) CreateInstance(arg0);

    public T? CreateInstance<T>(object? arg0, object? arg1) => (T?) CreateInstance(arg0, arg1);

    public T? CreateInstance<T>(object? arg0, object? arg1, object? arg2) => (T?) CreateInstance(arg0, arg1, arg2);

    public T? CreateInstance<T>(object? arg0, object? arg1, object? arg2, object? arg3) => (T?) CreateInstance(arg0, arg1, arg2, arg3);
}
