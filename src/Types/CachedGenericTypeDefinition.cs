using System;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedGenericTypeDefinition"/>
public class CachedGenericTypeDefinition : ICachedGenericTypeDefinition
{
    private readonly Lazy<CachedType> _cachedGenericTypeDefinition;

    public CachedGenericTypeDefinition(CachedType cachedType)
    {
        _cachedGenericTypeDefinition = new Lazy<CachedType>(() => 
            new CachedType(cachedType.Type!.GetGenericTypeDefinition()), true);
    }
    
    public CachedType GetCachedGenericTypeDefinition()
    {
        return _cachedGenericTypeDefinition.Value;
    }

    public Type? GetGenericTypeDefinition()
    {
        return _cachedGenericTypeDefinition.Value.Type;
    }
}