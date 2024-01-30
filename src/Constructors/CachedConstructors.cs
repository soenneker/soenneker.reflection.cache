using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Constructors;

///<inheritdoc cref="ICachedConstructors"/>
public class CachedConstructors : ICachedConstructors
{
    private readonly Lazy<CachedConstructor[]> _cachedArray;
    private readonly Lazy<Dictionary<int, CachedConstructor>> _cachedDict;

    private readonly CachedType _cachedType;

    private readonly Lazy<ConstructorInfo?[]> _cachedConstructorInfos;

    public CachedConstructors(CachedType cachedType, bool threadSafe = true)
    {
        _cachedType = cachedType;

        _cachedArray = new Lazy<CachedConstructor[]>(SetArray, threadSafe);
        _cachedDict = new Lazy<Dictionary<int, CachedConstructor>>(SetDict, threadSafe);

        _cachedConstructorInfos = new Lazy<ConstructorInfo?[]>(_cachedArray.Value.ToConstructors, threadSafe);
    }

    public CachedConstructor? GetCachedConstructor(Type[]? parameterTypes = null)
    {
        int key = parameterTypes.GetCacheKey();
        return _cachedDict.Value.GetValueOrDefault(key);
    }

    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null)
    {
        return GetCachedConstructor(parameterTypes)?.ConstructorInfo;
    }

    private CachedConstructor[] SetArray()
    {
        if (_cachedDict.IsValueCreated)
        {
            Dictionary<int, CachedConstructor>.ValueCollection cachedDictValues = _cachedDict.Value.Values;
            var result = new CachedConstructor[cachedDictValues.Count];
            var i = 0;

            foreach (CachedConstructor constructor in cachedDictValues)
            {
                result[i++] = constructor;
            }

            return result;
        }

        ConstructorInfo[] constructorInfos = _cachedType.Type!.GetConstructors(ReflectionCacheConstants.BindingFlags);
        var cachedConstructors = new CachedConstructor[constructorInfos.Length];

        for (var i = 0; i < constructorInfos.Length; i++)
        {
            cachedConstructors[i] = new CachedConstructor(constructorInfos[i]);
        }

        return cachedConstructors;
    }

    private Dictionary<int, CachedConstructor> SetDict()
    {
        if (_cachedArray.IsValueCreated)
        {
            CachedConstructor[]? cachedArrayValue = _cachedArray.Value;
            var dict = new Dictionary<int, CachedConstructor>(cachedArrayValue.Length);

            for (var i = 0; i < cachedArrayValue.Length; i++)
            {
                int key = cachedArrayValue[i].GetCacheKey();
                dict[key] = cachedArrayValue[i];
            }

            return dict;
        }

        ConstructorInfo[] constructorInfos = _cachedType.Type!.GetConstructors(ReflectionCacheConstants.BindingFlags);
        var constructorsDict = new Dictionary<int, CachedConstructor>(constructorInfos.Length);

        for (var i = 0; i < constructorInfos.Length; i++)
        {
            ConstructorInfo info = constructorInfos[i];

            ParameterInfo[] parameters = info.GetParameters();
            Type[] parameterTypes = new Type[parameters.Length];

            for (var j = 0; j < parameters.Length; j++)
            {
                parameterTypes[j] = parameters[j].ParameterType;
            }

            int key = parameterTypes.GetCacheKey();

            constructorsDict[key] = new CachedConstructor(info);
        }

        return constructorsDict;
    }

    public CachedConstructor[] GetCachedConstructors()
    {
        return _cachedArray.Value;
    }

    public ConstructorInfo?[] GetConstructors()
    {
        return _cachedConstructorInfos.Value;
    }

    public object? CreateInstance()
    {
        //TODO: One day parameterless invoke of the constructorInfo may be faster than Activator.CreateInstance
        return Activator.CreateInstance(_cachedType.Type!);
    }

    public T? CreateInstance<T>()
    {
        return (T?)CreateInstance();
    }

    public object? CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
            return CreateInstance();

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

        if (cachedConstructor == null)
            return default;

        return cachedConstructor.Invoke<T>(parameters);
    }
}