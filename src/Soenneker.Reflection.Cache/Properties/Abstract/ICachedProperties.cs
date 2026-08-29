using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Properties.Abstract;

/// <summary>
/// Represents a cached set of properties for a type.
/// </summary>
public interface ICachedProperties
{
    /// <summary>
    /// Gets a property by name.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The property with the specified name, or <c>null</c> if not found.</returns>
    [Pure]
    PropertyInfo? GetProperty(string name);

    /// <summary>
    /// Gets cached property.
    /// </summary>
    /// <param name="name">Name of the Cached Properties value to target.</param>
    /// <returns>The matching cached property, or <c>null</c> when no property has that name.</returns>
    [Pure]
    CachedProperty? GetCachedProperty(string name);

    /// <summary>
    /// Gets an array of cached properties.
    /// </summary>
    /// <returns>An array of cached properties.</returns>
    [Pure]
    PropertyInfo[] GetProperties();

    /// <summary>
    /// Gets cached properties.
    /// </summary>
    /// <returns>The cached properties in the configured reflection scope.</returns>
    [Pure]
    CachedProperty[] GetCachedProperties();
}
