using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedIsAssignableFrom"/>
public class CachedIsAssignableFrom : ICachedIsAssignableFrom
{
    private readonly Dictionary<int, bool>? _cachedDict;
    private readonly ConcurrentDictionary<int, bool>? _cachedConcurrentDict;

    private readonly CachedType _cachedType;
    private readonly bool _threadSafe;

    public CachedIsAssignableFrom(CachedType cachedType, bool threadSafe = true)
    {
        _threadSafe = threadSafe;

        if (threadSafe)
            _cachedConcurrentDict = new ConcurrentDictionary<int, bool>();
        else
            _cachedDict = new Dictionary<int, bool>();

        _cachedType = cachedType;
    }

    public bool IsAssignableFrom(Type derivedType)
    {
        if (_cachedType.Type == null)
            return false;

        int key = _cachedType.CacheKey.GetValueOrDefault() + derivedType.GetHashCode();

        if (_threadSafe)
        {
            if (_cachedConcurrentDict!.TryGetValue(key, out bool result))
                return result;

            bool isAssignableThreadSafe = _cachedType.Type!.IsAssignableFrom(derivedType);

            _cachedConcurrentDict.TryAdd(key, isAssignableThreadSafe);
            return isAssignableThreadSafe;
        }

        if (_cachedDict!.TryGetValue(key, out bool from))
            return from;

        bool isAssignable = _cachedType.Type!.IsAssignableFrom(derivedType);
        _cachedDict.TryAdd(key, isAssignable);

        return isAssignable;
    }

    public bool IsAssignableFrom(CachedType derivedType)
    {
        if (derivedType.Type == null)
            return false;

        return IsAssignableFrom(derivedType.Type);
    }
}