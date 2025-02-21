using Soenneker.Reflection.Cache.Types;
using System;

namespace Soenneker.Reflection.Cache.Attributes.Abstract;

/// <summary>
/// Represents a cached attribute associated with a type, method, or constructor.
/// </summary>
public interface ICachedAttribute
{
    /// <summary>
    /// Gets the underlying attribute object.
    /// </summary>
    object Attribute { get; }

    /// <summary>
    /// Gets the type of the attribute.
    /// </summary>
    Type Type { get; }

    CachedType CachedType { get; }

    string Name { get; }
}