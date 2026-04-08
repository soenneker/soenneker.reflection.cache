using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Attributes.Abstract;

/// <summary>
/// Represents a cached set of custom attributes for a type, method, or constructor.
/// </summary>
public interface ICachedCustomAttributes
{
    /// <summary>
    /// Gets an array of cached attributes associated with the type, method, or constructor.
    /// </summary>
    /// <returns>An array of cached attributes.</returns>
    [Pure]
    CachedAttribute[] GetCachedCustomAttributes();

    /// <summary>
    /// Gets an array of custom attributes associated with the type, method, or constructor.
    /// </summary>
    /// <returns>An array of custom attributes.</returns>
    [Pure]
    object[] GetCustomAttributes();

    /// <summary>
    /// Gets the first attribute of type <typeparamref name="T"/>.
    /// When <paramref name="inherit"/> is <see langword="true"/> (the default), uses the cached attribute list built with <c>GetCustomAttributes(inherit: true)</c> for this member.
    /// When <see langword="false"/>, uses reflection with <c>inherit: false</c> (not cached).
    /// </summary>
    [Pure]
    T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute;
}
