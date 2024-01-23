using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedTypes"/>
public class CachedTypes : ICachedTypes
{
    private readonly ConcurrentDictionary<string, CachedType>? _concurrentDict;
    private readonly Dictionary<string, CachedType>? _dict;

    private readonly bool _threadSafe;

    public CachedTypes(bool threadSafe)
    {
        _threadSafe = threadSafe;

        if (_threadSafe)
            _concurrentDict =[];
        else
            _dict = [];
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

                var newCachedType = new CachedType(type);

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

        var newCachedType = new CachedType(type);

        if (type == null)
            _dict.TryAdd(typeName, newCachedType);
        else
            _dict.TryAdd(type.FullName!, newCachedType);

        return newCachedType;
    }

    public CachedType GetCachedType(Type type)
    {
        if (string.IsNullOrEmpty(type.FullName))
            throw new ArgumentException("The type's FullName cannot be null or empty");

        if (_threadSafe)
            return _concurrentDict!.GetOrAdd(type.FullName, _ => new CachedType(type));

        if (_dict!.TryGetValue(type.FullName!, out CachedType? result))
            return result;

        var newCachedType = new CachedType(type);

        _dict.TryAdd(type.FullName, newCachedType);

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