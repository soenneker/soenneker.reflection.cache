using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Parameters.Abstract;

/// <summary>
/// Provides cached parameter metadata for a method or constructor.
/// </summary>
public interface ICachedParameters
{
    /// <summary>
    /// Gets all cached parameters in declaration order.
    /// </summary>
    /// <returns>The cached parameters.</returns>
    [Pure]
    CachedParameter[] GetCachedParameters();

    /// <summary>
    /// Gets an array of <see cref="ParameterInfo"/> associated with the cached parameters.
    /// </summary>
    /// <returns>An array of <see cref="ParameterInfo"/> objects.</returns>
    [Pure]
    ParameterInfo[] GetParameters();

    /// <summary>
    /// Gets an array of parameter types associated with the cached parameters.
    /// </summary>
    /// <returns>An array of parameter types.</returns>
    [Pure]
    Type[] GetParameterTypes();
}
