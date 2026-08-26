using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Provides canonical cached wrappers for reflection types and cached type-name resolution.
/// </summary>
public interface ICachedTypes
{
    /// <summary>
    /// Gets or creates a cached type for an assembly-qualified or runtime-resolvable type name.
    /// </summary>
    /// <param name="typeName">The case-sensitive type name passed to <see cref="Type.GetType(string)"/>.</param>
    /// <returns>The cached wrapper. Its underlying type is <c>null</c> when the name cannot be resolved.</returns>
    [Pure]
    CachedType GetCachedType(string typeName);

    /// <summary>
    /// Gets or creates the canonical cached wrapper for a reflection type.
    /// </summary>
    /// <param name="type">The reflection type to cache.</param>
    /// <returns>The canonical cached wrapper for <paramref name="type"/>.</returns>
    [Pure]
    CachedType GetCachedType(Type type);

    /// <summary>
    /// Resolves a type name and caches the result, including unsuccessful lookups.
    /// </summary>
    /// <param name="typeName">The case-sensitive type name passed to <see cref="Type.GetType(string)"/>.</param>
    /// <returns>The resolved type, or <c>null</c> when the name cannot be resolved.</returns>
    [Pure]
    Type? GetType(string typeName);

    /// <summary>
    /// Caches and returns the supplied reflection type.
    /// </summary>
    /// <param name="type">The reflection type to cache.</param>
    /// <returns>The supplied reflection type.</returns>
    [Pure]
    Type? GetType(Type type);
}
