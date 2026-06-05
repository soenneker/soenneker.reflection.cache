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

    /// <summary>
    /// Gets cached type.
    /// </summary>
    CachedType CachedType { get; }

    /// <summary>
    /// Gets name.
    /// </summary>
    string Name { get; }
}