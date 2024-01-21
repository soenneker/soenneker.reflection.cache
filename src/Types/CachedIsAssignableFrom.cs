using System;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedIsAssignableFrom"/>
public class CachedIsAssignableFrom : ICachedIsAssignableFrom
{
    // Does not need worry about concurrency because the underlying call is fast and there aren't ref types being used
    private readonly Dictionary<int, bool> _cachedDict;

    private readonly CachedType _cachedType;

    public CachedIsAssignableFrom(CachedType cachedType)
    {
        _cachedType = cachedType;

        _cachedDict = new Dictionary<int, bool>();
    }

    public bool IsAssignableFrom(Type derivedType)
    {
        if (_cachedType.Type == null)
            return false;

        int key = _cachedType.GetCacheKey()!.Value + derivedType.GetHashCode();

        if (_cachedDict.TryGetValue(key, out bool from))
            return from;

        bool result = _cachedType.Type!.IsAssignableFrom(derivedType);
        _cachedDict.TryAdd(key, result);

        return result;
    }

    public bool IsAssignableFrom(CachedType derivedType)
    {
        if (derivedType.Type == null)
            return false;

        return IsAssignableFrom(derivedType.Type);
    }
}