using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Attributes.Abstract;

/// <summary>
/// Represents a cached attribute associated with a type, method, or constructor.
/// </summary>
public interface ICachedAttribute
{
    /// <summary>
    /// Gets the underlying attribute object.
    /// </summary>
    [Pure]
    object Attribute { get; }

    /// <summary>
    /// Gets the type of the attribute.
    /// </summary>
    [Pure]
    Type AttributeType { get; }
}