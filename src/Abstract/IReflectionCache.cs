using System.Diagnostics.Contracts;
using System;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Abstract;

/// <summary>
/// The fastest .NET Reflection cache
/// </summary>
public interface IReflectionCache
{
    /// <summary>
    /// Gets a cached type by name.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <returns>The cached type corresponding to the given type name.</returns>
    [Pure]
    ICachedType GetCachedType(string typeName);

    /// <summary>
    /// Gets a cached type by <see cref="Type"/> object.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> object.</param>
    /// <returns>The cached type corresponding to the given <see cref="Type"/> object.</returns>
    [Pure]
    ICachedType GetCachedType(Type type);

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