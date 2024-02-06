using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Represents an interface for caching element types of generic types.
/// </summary>
public interface ICachedGetElementType
{
    /// <summary>
    /// Retrieves the cached element type.
    /// </summary>
    /// <returns>The cached element type.</returns>
    [Pure]
    CachedType? GetCachedElementType();

    /// <summary>
    /// Retrieves the element type.
    /// </summary>
    /// <returns>The element type.</returns>
    [Pure]
    Type? GetElementType();
}