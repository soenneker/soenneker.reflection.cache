using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Parameters;

namespace Soenneker.Reflection.Cache.Methods.Abstract;

/// <summary>
/// Represents a cached method for a type.
/// </summary>
public interface ICachedMethod
{
    /// <summary>
    /// Gets the <see cref="MethodInfo"/> associated with this cached method.
    /// </summary>
    MethodInfo? MethodInfo { get; }

    /// <summary>
    /// Gets the name of the method.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the return type of the method.
    /// </summary>
    Type? ReturnType { get; }

    /// <summary>
    /// Gets the cached parameters for this method.
    /// </summary>
    CachedParameters? Parameters { get; }

    /// <summary>
    /// Gets the cached custom attributes for this method.
    /// </summary>
    CachedCustomAttributes? Attributes { get; }
}