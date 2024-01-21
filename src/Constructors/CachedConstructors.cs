using System;
using System.Collections.Generic;
using System.Linq;
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

    public CachedConstructors(CachedType cachedType)
    {
        _cachedType = cachedType;

        _cachedArray = new Lazy<CachedConstructor[]>(SetArray, true);
        _cachedDict = new Lazy<Dictionary<int, CachedConstructor>>(SetDict, true);
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
        // Use the dictionary if it's already populated
        if (_cachedDict.IsValueCreated)
        {
            return _cachedDict.Value.Values.ToArray();
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
        // Use the array if it's already populated
        if (_cachedArray.IsValueCreated)
        {
            var dict = new Dictionary<int, CachedConstructor>(_cachedArray.Value.Length);

            foreach (CachedConstructor constructor in _cachedArray.Value)
            {
                int key = constructor.GetCacheKey();
                dict.Add(key, constructor);
            }

            return dict;
        }

        ConstructorInfo[] constructorInfos = _cachedType.Type!.GetConstructors(ReflectionCacheConstants.BindingFlags);

        var constructorsDict = new Dictionary<int, CachedConstructor>(constructorInfos.Length);

        for (var i = 0; i < constructorInfos.Length; i++)
        {
            ConstructorInfo info = constructorInfos[i];

            Type[] parameterTypes = info.GetParameters().Select(c => c.ParameterType).ToArray();

            int key = parameterTypes.GetCacheKey();

            constructorsDict.Add(key, new CachedConstructor(info));
        }

        return constructorsDict;
    }

    public CachedConstructor[] GetCachedConstructors()
    {
        return _cachedArray.Value;
    }

    public ConstructorInfo?[] GetConstructors()
    {
        CachedConstructor[]? cachedArray = _cachedArray.Value;

        int count = cachedArray.Length;

        var constructors = new ConstructorInfo?[count];

        for (var i = 0; i < count; i++)
        {
            constructors[i] = cachedArray[i].ConstructorInfo;
        }

        return constructors;
    }

    public object? CreateInstance()
    {
        //TODO: One day parameterless invoke of the constructorInfo may be faster than Activator.CreateInstance
        //CachedConstructor? cachedConstructor = GetCachedConstructor();
        //return cachedConstructor?.Invoke();

        return Activator.CreateInstance(_cachedType.Type!);
    }

    public object? CreateInstance(params object[] parameters)
    {
        Type[] parameterTypes = GetParameterTypes(parameters);

        CachedConstructor? cachedConstructor = GetCachedConstructor(parameterTypes);

        return cachedConstructor?.Invoke(parameters);
    }

    public T? CreateInstance<T>(params object[] parameters)
    {
        Type[] parameterTypes = GetParameterTypes(parameters);

        CachedConstructor? cachedConstructor = GetCachedConstructor(parameterTypes);

        if (cachedConstructor == null)
            return default;

        return cachedConstructor.Invoke<T>(parameters);
    }

    private static Type[] GetParameterTypes(object[] parameters)
    {
        var parameterTypes = new Type[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            parameterTypes[i] = parameters[i].GetType();
        }

        return parameterTypes;
    }
}