﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Extensions.Array.Object;
using Soenneker.Extensions.Type.Array;
using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Constructors;

///<inheritdoc cref="ICachedConstructors"/>
public sealed class CachedConstructors : ICachedConstructors
{
    private readonly Lazy<CachedConstructor[]> _cachedArray;
    private readonly Lazy<Dictionary<int, CachedConstructor>> _cachedDict;

    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;

    private readonly Lazy<ConstructorInfo?[]> _cachedConstructorInfos;

    public CachedConstructors(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType;
        _cachedTypes = cachedTypes;

        _cachedArray = new Lazy<CachedConstructor[]>(() => SetArray(threadSafe), threadSafe);
        _cachedDict = new Lazy<Dictionary<int, CachedConstructor>>(() => SetDict(threadSafe), threadSafe);
        _cachedDict = new Lazy<Dictionary<int, CachedConstructor>>(() => SetDict(threadSafe), threadSafe);

        _cachedConstructorInfos = new Lazy<ConstructorInfo?[]>(() => _cachedArray.Value.ToConstructorInfos(), threadSafe);
    }

    public CachedConstructor? GetCachedConstructor(Type[]? parameterTypes = null)
    {
        int key = parameterTypes.ToHashKey();
        return _cachedDict.Value.GetValueOrDefault(key);
    }

    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null)
    {
        return GetCachedConstructor(parameterTypes)?.ConstructorInfo;
    }

    private CachedConstructor[] SetArray(bool threadSafe)
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

        ConstructorInfo[] constructorInfos = _cachedType.Type!.GetConstructors(_cachedTypes.Options.ConstructorFlags);
        int length = constructorInfos.Length;

        var cachedConstructors = new CachedConstructor[length];

        for (var i = 0; i < length; i++)
        {
            cachedConstructors[i] = new CachedConstructor(constructorInfos[i], _cachedTypes, threadSafe);
        }

        return cachedConstructors;
    }

    private Dictionary<int, CachedConstructor> SetDict(bool threadSafe)
    {
        if (_cachedArray.IsValueCreated)
        {
            CachedConstructor[] cachedArrayValue = _cachedArray.Value;
            int length = cachedArrayValue.Length;

            var dict = new Dictionary<int, CachedConstructor>(length);

            for (var i = 0; i < length; i++)
            {
                int key = cachedArrayValue[i].ToHashKey();
                dict[key] = cachedArrayValue[i];
            }

            return dict;
        }

        ConstructorInfo[] constructorInfos = _cachedType.Type!.GetConstructors(_cachedTypes.Options.ConstructorFlags);
        int constructorInfosLength = constructorInfos.Length;

        var constructorsDict = new Dictionary<int, CachedConstructor>(constructorInfosLength);

        ReadOnlySpan<ConstructorInfo> constructorsSpan = constructorInfos;

        for (var i = 0; i < constructorInfosLength; i++)
        {
            ConstructorInfo info = constructorsSpan[i];

            ParameterInfo[] parameters = info.GetParameters();

            int parametersLength = parameters.Length;

            var parameterTypes = new Type[parametersLength];

            ReadOnlySpan<ParameterInfo> parametersSpan = parameters;

            for (var j = 0; j < parametersLength; j++)
            {
                parameterTypes[j] = parametersSpan[j].ParameterType;
            }

            int key = parameterTypes.ToHashKey();

            constructorsDict[key] = new CachedConstructor(info, _cachedTypes, threadSafe);
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
        return (T?)Activator.CreateInstance(_cachedType.Type!);
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