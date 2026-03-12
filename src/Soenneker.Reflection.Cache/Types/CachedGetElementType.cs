using System;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedGetElementType"/>
public sealed class CachedGetElementType : ICachedGetElementType
{
    private readonly Lazy<CachedType?> _cachedElementType;

    public CachedGetElementType(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedElementType = new Lazy<CachedType?>(() =>
        {
            Type? elementType = cachedType.Type!.GetElementType();

            if (elementType == null)
                return null;

            CachedType cachedElementType = cachedTypes.GetCachedType(elementType);

            return cachedElementType;
        }, threadSafe);
    }

    public CachedType? GetCachedElementType()
    {
        return _cachedElementType.Value;
    }

    public Type? GetElementType()
    {
        return _cachedElementType.Value.Type;
    }
}