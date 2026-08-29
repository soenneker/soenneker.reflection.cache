using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Fields.Abstract;

/// <summary>
/// Represents a cached set of fields for a type.
/// </summary>
public interface ICachedFields
{
    /// <summary>
    /// Gets a field by name.
    /// </summary>
    /// <param name="name">The name of the field.</param>
    /// <returns>The field with the specified name, or <c>null</c> if not found.</returns>
    [Pure]
    FieldInfo? GetField(string name);

    /// <summary>
    /// Gets cached field.
    /// </summary>
    /// <param name="name">Name of the Cached Fields value to target.</param>
    /// <returns>The matching cached field, or <c>null</c> when no field has that name.</returns>
    [Pure]
    CachedField? GetCachedField(string name);

    /// <summary>
    /// Gets an array of cached fields.
    /// </summary>
    /// <returns>An array of cached fields.</returns>
    [Pure]
    FieldInfo[] GetFields();

    /// <summary>
    /// Gets cached fields.
    /// </summary>
    /// <returns>The cached fields in the configured reflection scope.</returns>
    [Pure]
    CachedField[] GetCachedFields();
}
