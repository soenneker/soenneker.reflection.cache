using System;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

/// <inheritdoc cref="ICachedGenericTypeDefinition"/>
public sealed class CachedGenericTypeDefinition : ICachedGenericTypeDefinition
{
    private readonly CachedType _cachedGenericTypeDefinition;

    public CachedGenericTypeDefinition(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        Type definitionType = cachedType.Type!.GetGenericTypeDefinition();
        _cachedGenericTypeDefinition = cachedTypes.GetCachedType(definitionType);
    }
    
    public CachedType GetCachedGenericTypeDefinition()
    {
        return _cachedGenericTypeDefinition;
    }

    public Type? GetGenericTypeDefinition()
    {
        return _cachedGenericTypeDefinition.Type;
    }
}
