using System;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedGenericTypeDefinition"/>
public class CachedGenericTypeDefinition : ICachedGenericTypeDefinition
{
    private readonly Lazy<CachedType> _cachedGenericTypeDefinition;

    public CachedGenericTypeDefinition(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedGenericTypeDefinition = new Lazy<CachedType>(() =>
        {
            Type definitionType = cachedType.Type!.GetGenericTypeDefinition();

            CachedType genericCachedTyped = cachedTypes.GetCachedType(definitionType);

            return genericCachedTyped;
        }, threadSafe);
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