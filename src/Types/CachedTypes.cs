using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Options;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedTypes"/>
public class CachedTypes : ICachedTypes
{
    // We'll use two sets of dictionaries - one for fast integer lookups (hopefully, common), and the other for slower string lookups
    private readonly ConcurrentDictionary<string, CachedType>? _concurrentDict;
    private readonly ConcurrentDictionary<int, CachedType>? _concurrentDictByType;

    private readonly Dictionary<string, CachedType>? _dict;
    private readonly Dictionary<int, CachedType>? _dictByType;

    private readonly bool _threadSafe;

    public ReflectionCacheOptions Options { get; private set; }

    public CachedTypes(ReflectionCacheOptions? options = null, bool threadSafe = true)
    {
        options ??= new ReflectionCacheOptions();
        Options = options;

        _threadSafe = threadSafe;

        if (_threadSafe)
        {
            _concurrentDict = [];
            _concurrentDictByType = [];
        }
        else
        {
            _dict = [];
            _dictByType = [];
        }
    }

    public CachedType GetCachedType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            throw new ArgumentException("typeName cannot be null or empty");

        if (_threadSafe)
        {
            return _concurrentDict!.GetOrAdd(typeName, _ =>
            {
                var type = Type.GetType(typeName);

                var newCachedType = new CachedType(type, this, _threadSafe);

                if (type == null)
                    _concurrentDict.TryAdd(typeName, newCachedType);
                else
                    _concurrentDict.TryAdd(type.FullName!, newCachedType);

                return newCachedType;
            });
        }

        if (_dict!.TryGetValue(typeName, out CachedType? result))
            return result;

        var type = Type.GetType(typeName);

        var newCachedType = new CachedType(type, this, _threadSafe);

        if (type == null)
            _dict.TryAdd(typeName, newCachedType);
        else
            _dict.TryAdd(type.FullName!, newCachedType);

        return newCachedType;
    }

    public CachedType GetCachedType(Type type)
    {
        int key = type.GetHashCode();

        if (_threadSafe)
            return _concurrentDictByType!.GetOrAdd(key, _ => new CachedType(type, this, _threadSafe));

        if (_dictByType!.TryGetValue(key, out CachedType? result))
            return result;

        var newCachedType = new CachedType(type, this, _threadSafe);

        _dictByType.TryAdd(key, newCachedType);

        return newCachedType;
    }

    public Type? GetType(string typeName)
    {
        return GetCachedType(typeName).Type;
    }

    public Type? GetType(Type type)
    {
        return GetCachedType(type).Type;
    }
}