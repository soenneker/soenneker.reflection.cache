using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Represents a cached generic type definition.
/// </summary>
public interface ICachedGenericTypeDefinition
{
    /// <summary>
    /// Gets the cached generic type definition.
    /// </summary>
    /// <returns>The cached generic type definition.</returns>
    [Pure]
    CachedType GetCachedGenericTypeDefinition();

    /// <summary>
    /// Gets the generic type definition.
    /// </summary>
    /// <returns>The generic type definition.</returns>
    [Pure]
    Type? GetGenericTypeDefinition();
}