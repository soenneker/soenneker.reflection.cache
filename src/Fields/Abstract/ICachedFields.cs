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
    /// Gets an array of cached fields.
    /// </summary>
    /// <returns>An array of cached fields.</returns>
    [Pure]
    FieldInfo[] GetFields();
}
