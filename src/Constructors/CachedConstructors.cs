using Soenneker.Extensions.Array.Object;
using Soenneker.Extensions.Type.Array;
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
    private readonly Lazy<FrozenDictionary<int, CachedConstructor>> _cachedDict;
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

        _cachedDict = new Lazy<FrozenDictionary<int, CachedConstructor>>(() =>
        {
            CachedConstructor[] arr = _cachedArray.Value;
            var dict = new Dictionary<int, CachedConstructor>(arr.Length);
            for (var i = 0; i < arr.Length; i++)
            {
                dict[arr[i].ToHashKey()] = arr[i]; // last-one-wins for duplicate sig hashes
            }

            return dict.ToFrozenDictionary();
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
        int key = parameterTypes.ToHashKey();
        return _cachedDict.Value.GetValueOrDefault(key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null) => GetCachedConstructor(parameterTypes)?.ConstructorInfo;

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

    public T? CreateInstance<T>(params object[] parameters)
    {
        if (parameters.Length == 0)
            return CreateInstance<T>();

        Type[] parameterTypes = parameters.ToTypes();

        CachedConstructor? cachedConstructor = GetCachedConstructor(parameterTypes);
        return cachedConstructor is null ? default : cachedConstructor.Invoke<T>(parameters);
    }
}