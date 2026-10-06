using System;
using System.Diagnostics.Contracts;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Interfaces.Abstract;

/// <summary>
/// Represents a cached set of interfaces for a type.
/// </summary>
public interface ICachedInterfaces
{
    /// <summary>
    /// Gets a cached interface by name.
    /// </summary>
    /// <param name="typeName">The name of the interface.</param>
    /// <returns>The cached interface with the specified name.</returns>
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [Pure]
    CachedType GetCachedInterface(string typeName);

    /// <summary>
    /// Gets an interface by name.
    /// </summary>
    /// <param name="typeName">The name of the interface.</param>
    /// <returns>The interface with the specified name.</returns>
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [Pure]
    Type? GetInterface(string typeName);

    /// <summary>
    /// Gets an array of cached interfaces.
    /// </summary>
    /// <returns>An array of cached interfaces.</returns>
    [Pure]
    CachedType[] GetCachedInterfaces();

    /// <summary>
    /// Gets an array of interfaces.
    /// </summary>
    /// <returns>An array of interfaces.</returns>
    [Pure] 
    Type[] GetInterfaces();
}