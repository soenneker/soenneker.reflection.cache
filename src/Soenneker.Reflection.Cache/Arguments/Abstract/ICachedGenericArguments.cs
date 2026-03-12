using System;
using System.Diagnostics.Contracts;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Arguments.Abstract;

/// <summary>
/// Represents a cached set of generic arguments for a type.
/// </summary>
public interface ICachedGenericArguments
{
    /// <summary>
    /// Gets an array of cached generic arguments associated with the type.
    /// </summary>
    /// <returns>An array of cached generic arguments.</returns>
    [Pure]
    CachedType[] GetCachedGenericArguments();

    /// <summary>
    /// Gets an array of generic arguments associated with the type.
    /// </summary>
    /// <returns>An array of generic arguments.</returns>
    [Pure]
    Type[] GetGenericArguments();
}