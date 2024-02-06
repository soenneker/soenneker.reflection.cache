using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedIsAssignableFrom"/>
public class CachedMakeGenericType : ICachedMakeGenericType
{
    private readonly ConcurrentDictionary<int, CachedType>? _cachedConcurrentDict;
    private readonly Dictionary<int, CachedType>? _cachedDict;

    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    public CachedMakeGenericType(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _threadSafe = threadSafe;

        if (threadSafe)
            _cachedConcurrentDict = new ConcurrentDictionary<int, CachedType>();
        else
            _cachedDict = [];

        _cachedType = cachedType;
        _cachedTypes = cachedTypes;
    }

    public CachedType? MakeGenericCachedType(params Type[] typeArguments)
    {
        if (_cachedType.Type == null)
            return null;

        int key = _cachedType.CacheKey.GetValueOrDefault() + typeArguments.ToCacheKey();

        Type genericType;

        CachedType newCachedType;

        if (_threadSafe)
        {
            if (_cachedConcurrentDict!.TryGetValue(key, out CachedType? result))
                return result;

            genericType = _cachedType.Type!.MakeGenericType(typeArguments);

            newCachedType = _cachedTypes.GetCachedType(genericType);

            _cachedConcurrentDict.TryAdd(key, newCachedType);
            return newCachedType;
        }

        if (_cachedDict!.TryGetValue(key, out CachedType? from))
            return from;

        genericType = _cachedType.Type!.MakeGenericType(typeArguments);

        newCachedType = _cachedTypes.GetCachedType(genericType);

        _cachedDict.TryAdd(key, newCachedType);

        return newCachedType;
    }

    public CachedType? MakeGenericCachedType(params CachedType[] cachedTypeArguments)
    {
        return MakeGenericCachedType(cachedTypeArguments.ToTypes());
    }

    public Type? MakeGenericType(params Type[] typeArguments)
    {
        return MakeGenericCachedType(typeArguments)?.Type;
    }
}