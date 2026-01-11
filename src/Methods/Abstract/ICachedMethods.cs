using Soenneker.Reflection.Cache.Types;
using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Methods.Abstract;

/// <summary>
/// Interface for CachedMethods providing access to cached and regular method information.
/// </summary>
public interface ICachedMethods
{
    /// <summary>
    /// Gets a CachedMethod by name.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <returns>The CachedMethod corresponding to the given name.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name);

    /// <summary>
    /// Gets a regular MethodInfo by name.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <returns>The MethodInfo corresponding to the given name.</returns>
    [Pure]
    MethodInfo? GetMethod(string name);

    /// <summary>
    /// Gets a CachedMethod by name and parameter types.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterTypes">An array of parameter types.</param>
    /// <returns>The CachedMethod corresponding to the given name and parameter types.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type[] parameterTypes);

    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0);

    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1);

    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1, Type t2);

    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1, Type t2, Type t3);

    [Pure]
    CachedMethod? GetCachedMethod(string name, CachedType[] cachedParameterTypes);

    /// <summary>
    /// Gets a regular MethodInfo by name and parameter types.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="types">An array of parameter types.</param>
    /// <returns>The MethodInfo corresponding to the given name and parameter types.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type[] types);

    [Pure]
    MethodInfo? GetMethod(string name, Type t0);

    [Pure]
    MethodInfo? GetMethod(string name, Type t0, Type t1);

    [Pure]
    MethodInfo? GetMethod(string name, Type t0, Type t1, Type t2);

    [Pure]
    MethodInfo? GetMethod(string name, Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets an array of CachedMethods.
    /// </summary>
    /// <returns>An array of CachedMethods.</returns>
    [Pure]
    CachedMethod[] GetCachedMethods();

    /// <summary>
    /// Gets an array of regular MethodInfos.
    /// </summary>
    /// <returns>An array of regular MethodInfos.</returns>
    [Pure]
    MethodInfo?[] GetMethods();
}