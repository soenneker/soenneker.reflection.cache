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
}