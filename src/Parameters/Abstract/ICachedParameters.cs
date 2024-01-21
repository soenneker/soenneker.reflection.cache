using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Parameters.Abstract;

/// <summary>
/// Represents a cached set of parameters for a method or constructor.
/// </summary>
public interface ICachedParameters
{
    /// <summary>
    /// Gets an array of <see cref="ParameterInfo"/> associated with the cached parameters.
    /// </summary>
    /// <returns>An array of <see cref="ParameterInfo"/> objects.</returns>
    [Pure]
    ParameterInfo?[] GetParameters();

    /// <summary>
    /// Gets an array of parameter types associated with the cached parameters.
    /// </summary>
    /// <returns>An array of parameter types.</returns>
    [Pure]
    Type[] GetParametersTypes();
}