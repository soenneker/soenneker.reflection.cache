using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Methods;

///<inheritdoc cref="ICachedMethods"/>
public class CachedMethods : ICachedMethods
{
    private readonly Lazy<Dictionary<int, CachedMethod>> _cachedDict;
    private readonly Lazy<CachedMethod[]> _cachedArray;

    private readonly CachedType _cachedType;

    private readonly Lazy<MethodInfo?[]> _cachedMethodsInfos;

    private readonly CachedTypes _cachedTypes;

    public CachedMethods(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType;
        _cachedTypes = cachedTypes;

        _cachedDict = new Lazy<Dictionary<int, CachedMethod>>(() => SetDict(threadSafe), threadSafe);
        _cachedArray = new Lazy<CachedMethod[]>(() => SetArray(threadSafe), threadSafe);
        _cachedMethodsInfos = new Lazy<MethodInfo?[]>(() => _cachedArray.Value.ToMethods(), threadSafe);
    }

    public CachedMethod GetCachedMethod(string name)
    {
        int key = ReflectionCacheUtil.GetCacheKeyForMethod(name);

        return _cachedDict.Value.GetValueOrDefault(key);
    }

    public MethodInfo? GetMethod(string name)
    {
        return GetCachedMethod(name).MethodInfo;
    }

    public CachedMethod GetCachedMethod(string name, Type[] parameterTypes)
    {
        int key = ReflectionCacheUtil.GetCacheKeyForMethod(name, parameterTypes);

        return _cachedDict.Value.GetValueOrDefault(key);
    }

    public CachedMethod GetCachedMethod(string name, CachedType[] cachedParameterTypes)
    {
        int key = ReflectionCacheUtil.GetCacheKeyForMethod(name, cachedParameterTypes);

        return _cachedDict.Value.GetValueOrDefault(key);
    }

    public MethodInfo? GetMethod(string name, Type[] types)
    {
        return GetCachedMethod(name, types).MethodInfo;
    }

    private CachedMethod[] SetArray(bool threadSafe)
    {
        if (_cachedDict.IsValueCreated && _cachedDict.Value.Count > 0)
        {
            Dictionary<int, CachedMethod>.ValueCollection cachedDictValues = _cachedDict.Value.Values;
            var result = new CachedMethod[cachedDictValues.Count];
            var i = 0;

            foreach (CachedMethod method in cachedDictValues)
            {
                result[i++] = method;
            }

            return result;
        }

        MethodInfo[] methodInfos = _cachedType.Type!.GetMethods(ReflectionCacheConstants.BindingFlags);
        int count = methodInfos.Length;

        var cachedArray = new CachedMethod[count];

        for (var i = 0; i < count; i++)
        {
            cachedArray[i] = new CachedMethod(methodInfos[i], _cachedTypes, threadSafe);
        }

        return cachedArray;
    }

    private Dictionary<int, CachedMethod> SetDict(bool threadSafe)
    {
        Dictionary<int, CachedMethod> cachedDict;
        int count;

        // Don't recreate these objects if the dict is already created
        if (_cachedArray.IsValueCreated)
        {
            CachedMethod[] cachedMethods = _cachedArray.Value;

            count = cachedMethods.Length;

            cachedDict = new Dictionary<int, CachedMethod>(count);

            for (var i = 0; i < count; i++)
            {
                CachedMethod cachedMethod = cachedMethods[i];
                int key = cachedMethod.ToCacheKey();

                cachedDict.Add(key, cachedMethod);
            }

            return cachedDict;
        }

        MethodInfo[] methodInfos = _cachedType.Type!.GetMethods(ReflectionCacheConstants.BindingFlags);

        count = methodInfos.Length;

        cachedDict = new Dictionary<int, CachedMethod>(count);

        for (var i = 0; i < count; i++)
        {
            MethodInfo methodInfo = methodInfos[i];

            var cachedMethod = new CachedMethod(methodInfo, _cachedTypes, threadSafe);
            int key = cachedMethod.ToCacheKey();

            cachedDict.Add(key, cachedMethod);
        }

        return cachedDict;
    }

    public CachedMethod[] GetCachedMethods()
    {
        return _cachedArray.Value;
    }

    public MethodInfo?[] GetMethods()
    {
        return _cachedMethodsInfos.Value;
    }
}