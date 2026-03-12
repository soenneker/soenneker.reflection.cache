using Soenneker.Reflection.Cache.Types;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Parameters.Abstract;

/// <summary>
/// Represents a cached parameter for a method or constructor.
/// </summary>
public interface ICachedParameter
{
    /// <summary>
    /// Gets the <see cref="ParameterInfo"/> associated with this cached parameter.
    /// </summary>
    ParameterInfo ParameterInfo { get; }

    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the type of the parameter.
    /// </summary>
    Type ParameterType { get; }

    CachedType CachedParameterType { get; }
}