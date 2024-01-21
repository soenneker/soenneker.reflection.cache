using System;
using System.Collections.Concurrent;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedTypes"/>
public class CachedTypes : ICachedTypes
{
    private readonly ConcurrentDictionary<string, ICachedType> _cachedDict = [];

    public ICachedType GetCachedType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            throw new ArgumentException("typeName cannot be null or empty");

        ICachedType? result = _cachedDict.GetOrAdd(typeName, _ =>
        {
            var type = Type.GetType(typeName);

            var newCachedType = new CachedType(type);

            if (type == null)
                _cachedDict.TryAdd(typeName, newCachedType);
            else
                _cachedDict.TryAdd(type.FullName!, newCachedType);

            return newCachedType;
        });

        return result;
    }

    public ICachedType GetCachedType(Type type)
    {
        if (string.IsNullOrEmpty(type.FullName))
            throw new ArgumentException("The type's FullName cannot be null or empty");

        return _cachedDict.GetOrAdd(type.FullName, _ => new CachedType(type));
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