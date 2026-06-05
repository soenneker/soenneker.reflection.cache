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

    /// <summary>
    /// Gets cached method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0);

    /// <summary>
    /// Gets cached method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1);

    /// <summary>
    /// Gets cached method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets cached method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets cached method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="cachedParameterTypes">The cached parameter types.</param>
    /// <returns>The result of the operation.</returns>
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

    /// <summary>
    /// Gets method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type t0);

    /// <summary>
    /// Gets method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type t0, Type t1);

    /// <summary>
    /// Gets method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
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