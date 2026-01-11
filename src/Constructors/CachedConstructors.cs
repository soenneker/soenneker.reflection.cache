using Soenneker.Extensions.Array.Object;
using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soenneker.Reflection.Cache.Constructors;

/// <inheritdoc cref="ICachedConstructors"/>
public sealed class CachedConstructors : ICachedConstructors
{
    private readonly Lazy<CachedConstructor[]> _cachedArray;
    private readonly Lazy<FrozenDictionary<string, CachedConstructor[]>> _byName;
    private readonly Lazy<ConstructorInfo?[]> _cachedConstructorInfos;

    // Fast path for parameterless construction (if available)
    private readonly Lazy<Func<object?>> _parameterlessActivator;

    public CachedConstructors(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        CachedType cachedType1 = cachedType;
        CachedTypes cachedTypes1 = cachedTypes;

        LazyThreadSafetyMode mode = threadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None;

        var constructorInfos = new Lazy<ConstructorInfo[]>(() => cachedType1.Type!.GetConstructors(cachedTypes1.Options.ConstructorFlags), mode);

        _cachedArray = new Lazy<CachedConstructor[]>(() =>
        {
            ConstructorInfo[] infos = constructorInfos.Value;
            var result = new CachedConstructor[infos.Length];
            for (var i = 0; i < infos.Length; i++)
            {
                result[i] = new CachedConstructor(infos[i], cachedTypes1, threadSafe);
            }

            return result;
        }, mode);

        _byName = new Lazy<FrozenDictionary<string, CachedConstructor[]>>(() =>
        {
            CachedConstructor[] arr = _cachedArray.Value;

            // Count constructors per name first to avoid List<> allocations.
            var countsByName = new Dictionary<string, int>(StringComparer.Ordinal);
            var names = new string[arr.Length];

            for (var i = 0; i < arr.Length; i++)
            {
                ConstructorInfo? ci = arr[i].ConstructorInfo;
                string name = ci is null ? string.Empty : ci.Name; // .ctor/.cctor
                names[i] = name;
                countsByName[name] = countsByName.TryGetValue(name, out int c) ? c + 1 : 1;
            }

            var byName = new Dictionary<string, CachedConstructor[]>(countsByName.Count, StringComparer.Ordinal);
            var fill = new Dictionary<string, int>(countsByName.Count, StringComparer.Ordinal);

            foreach (KeyValuePair<string, int> kvp in countsByName)
            {
                byName[kvp.Key] = new CachedConstructor[kvp.Value];
                fill[kvp.Key] = 0;
            }

            for (var i = 0; i < arr.Length; i++)
            {
                string name = names[i];
                int idx = fill[name];
                byName[name][idx] = arr[i];
                fill[name] = idx + 1;
            }

            return byName.ToFrozenDictionary(StringComparer.Ordinal);
        }, mode);

        _cachedConstructorInfos = new Lazy<ConstructorInfo?[]>(() => _cachedArray.Value.ToConstructorInfos(), mode);

        _parameterlessActivator = new Lazy<Func<object?>>(() =>
        {
            Type type = cachedType1.Type!;
            ConstructorInfo? ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor is null)
                return static () => null;

            try
            {
                // Build: () => new T()
                NewExpression newExpr = Expression.New(ctor);
                Expression<Func<object?>> lambda = Expression.Lambda<Func<object?>>(Expression.Convert(newExpr, typeof(object)));
                return lambda.Compile();
            }
            catch
            {
                return () => Activator.CreateInstance(type);
            }
        }, mode);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedConstructor? GetCachedConstructor(Type[]? parameterTypes = null)
    {
        if (parameterTypes == null || parameterTypes.Length == 0)
        {
            // Prefer parameterless .ctor if present
            if (_byName.Value.TryGetValue(".ctor", out CachedConstructor[]? ctors))
            {
                for (var i = 0; i < ctors.Length; i++)
                {
                    if (ctors[i].GetParameters().Length == 0)
                        return ctors[i];
                }
            }
            return null;
        }

        if (!_byName.Value.TryGetValue(".ctor", out CachedConstructor[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
            var ps = candidates[i].GetParameters();
            if (ps.Length != parameterTypes.Length)
                continue;

            var match = true;
            for (var j = 0; j < ps.Length; j++)
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

        if (!_byName.Value.TryGetValue(".ctor", out CachedConstructor[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
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

        if (!_byName.Value.TryGetValue(".ctor", out CachedConstructor[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
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

        if (!_byName.Value.TryGetValue(".ctor", out CachedConstructor[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
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

        if (!_byName.Value.TryGetValue(".ctor", out CachedConstructor[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
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

    public CachedConstructor[] GetCachedConstructors() => _cachedArray.Value;

    public ConstructorInfo?[] GetConstructors() => _cachedConstructorInfos.Value;

    public object? CreateInstance()
    {
        // Use the cached parameterless activator if present; otherwise null if no default ctor.
        Func<object?> f = _parameterlessActivator.Value;
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

        // Avoid a LINQ pass; reuse your extension which is likely optimized.
        Type[] parameterTypes = parameters.ToTypes();

        CachedConstructor? cachedConstructor = GetCachedConstructor(parameterTypes);
        return cachedConstructor?.Invoke(parameters);
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

        Type[] parameterTypes = parameters.ToTypes();

        CachedConstructor? cachedConstructor = GetCachedConstructor(parameterTypes);
        return cachedConstructor is null ? default : cachedConstructor.Invoke<T>(parameters);
    }

    public T? CreateInstance<T>(object? arg0) => (T?) CreateInstance(arg0);

    public T? CreateInstance<T>(object? arg0, object? arg1) => (T?) CreateInstance(arg0, arg1);

    public T? CreateInstance<T>(object? arg0, object? arg1, object? arg2) => (T?) CreateInstance(arg0, arg1, arg2);

    public T? CreateInstance<T>(object? arg0, object? arg1, object? arg2, object? arg3) => (T?) CreateInstance(arg0, arg1, arg2, arg3);
}