using System;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

/// <inheritdoc cref="ICachedGetElementType"/>
public sealed class CachedGetElementType : ICachedGetElementType
{
    private readonly CachedType? _cachedElementType;

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedGetElementType(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        Type? elementType = cachedType.Type!.GetElementType();
        _cachedElementType = elementType is null ? null : cachedTypes.GetCachedType(elementType);
    }

    public CachedType? GetCachedElementType()
    {
        return _cachedElementType;
    }

    public Type? GetElementType()
    {
        return _cachedElementType?.Type;
    }
}
