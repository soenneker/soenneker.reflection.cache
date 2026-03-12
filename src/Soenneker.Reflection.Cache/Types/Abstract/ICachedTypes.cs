using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Interface for CachedTypes providing access to cached type information.
/// </summary>
public interface ICachedTypes
{
    /// <summary>
    /// Gets a <see cref="CachedType"/> by type name.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <returns>The <see cref="CachedType"/> corresponding to the given type name.</returns>
    [Pure]
    CachedType GetCachedType(string typeName);

    /// <summary>
    /// Gets a <see cref="CachedType"/> by <see cref="Type"/> object.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> object.</param>
    /// <returns>The <see cref="CachedType"/> corresponding to the given <see cref="Type"/> object.</returns>
    [Pure]
    CachedType GetCachedType(Type type);

    /// <summary>
    /// Gets a <see cref="Type"/> by type name.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <returns>The <see cref="Type"/> corresponding to the given type name.</returns>
    [Pure]
    Type? GetType(string typeName);

    /// <summary>
    /// Gets a <see cref="Type"/> by <see cref="Type"/> object.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> object.</param>
    /// <returns>The <see cref="Type"/> corresponding to the given <see cref="Type"/> object.</returns>
    [Pure]
    Type? GetType(Type type);
}