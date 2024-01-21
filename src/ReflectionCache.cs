using System;
using Soenneker.Reflection.Cache.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache;

///<inheritdoc cref="IReflectionCache"/>
public class ReflectionCache : IReflectionCache
{
    private readonly ICachedTypes _cachedTypes;

    public ReflectionCache()
    {
        _cachedTypes = new CachedTypes();
    }

    public ICachedType GetCachedType(string typeName)
    {
        ICachedType result = _cachedTypes.GetCachedType(typeName);
        return result;
    }

    public ICachedType GetCachedType(Type type)
    {
        ICachedType result = _cachedTypes.GetCachedType(type);
        return result;
    }

    public Type? GetType(string typeName)
    {
        Type? result = _cachedTypes.GetType(typeName);
        return result;
    }

    public Type? GetType(Type type)
    {
        Type? result = _cachedTypes.GetType(type);
        return result;
    }
}