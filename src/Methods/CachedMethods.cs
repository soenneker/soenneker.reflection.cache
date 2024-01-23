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

    public CachedMethods(CachedType cachedType, bool threadSafe = true)
    {
        _cachedType = cachedType;

        _cachedDict = new Lazy<Dictionary<int, CachedMethod>>(SetCachedMethodsDict, threadSafe);
        _cachedArray = new Lazy<CachedMethod[]>(SetCachedMethodsArray, threadSafe);
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

    public CachedMethod GetCachedMethod(string name, Type[] types)
    {
        int key = ReflectionCacheUtil.GetCacheKeyForMethod(name, types);

        return _cachedDict.Value.GetValueOrDefault(key);
    }

    public MethodInfo? GetMethod(string name, Type[] types)
    {
        return GetCachedMethod(name, types).MethodInfo;
    }

    private CachedMethod[] SetCachedMethodsArray()
    {
        if (_cachedDict.IsValueCreated && _cachedDict.Value.Count > 0)
        {
            Dictionary<int, CachedMethod>.ValueCollection cachedDictValues = _cachedDict.Value.Values;
            var result = new CachedMethod[cachedDictValues.Count];
            var i = 0;

            foreach (CachedMethod? method in cachedDictValues)
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
            cachedArray[i] = new CachedMethod(methodInfos[i]);
        }

        return cachedArray;
    }

    private Dictionary<int, CachedMethod> SetCachedMethodsDict()
    {
        Dictionary<int, CachedMethod> cachedDict;
        int count;

        // Don't recreate these objects if the dict is already created
        if (_cachedArray.IsValueCreated)
        {
            CachedMethod[]? cachedMethods = _cachedArray.Value;

            count = cachedMethods.Length;

            cachedDict = new Dictionary<int, CachedMethod>(count);

            for (var i = 0; i < count; i++)
            {
                CachedMethod cachedMethod = cachedMethods[i];
                int key = cachedMethod.GetCacheKey();

                cachedDict.Add(key, cachedMethod);
            }
        }
        else
        {
            MethodInfo[] methodInfos = _cachedType.Type!.GetMethods(ReflectionCacheConstants.BindingFlags);

            count = methodInfos.Length;

            cachedDict = new Dictionary<int, CachedMethod>(count);

            for (var i = 0; i < count; i++)
            {
                MethodInfo methodInfo = methodInfos[i];

                var cachedMethod = new CachedMethod(methodInfo);
                int key = cachedMethod.GetCacheKey();

                cachedDict.Add(key, cachedMethod);
            }
        }

        return cachedDict;
    }

    public CachedMethod[] GetCachedMethods()
    {
        return _cachedArray.Value;
    }

    public MethodInfo?[] GetMethods()
    {
        CachedMethod[] cachedMethods = _cachedArray.Value;
        int count = cachedMethods.Length;
        var methodInfoArray = new MethodInfo?[count];

        for (var i = 0; i < count; i++)
        {
            methodInfoArray[i] = cachedMethods[i].MethodInfo;
        }

        return methodInfoArray;
    }
}