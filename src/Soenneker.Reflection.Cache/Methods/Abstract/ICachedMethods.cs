using Soenneker.Reflection.Cache.Types;
using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Methods.Abstract;

/// <summary>
/// Provides cached lookup of methods declared on or inherited by a type.
/// </summary>
public interface ICachedMethods
{
    /// <summary>
    /// Gets the first cached method with the specified name.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <returns>The CachedMethod corresponding to the given name.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name);

    /// <summary>
    /// Gets the reflection metadata for the first method with the specified name.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <returns>The MethodInfo corresponding to the given name.</returns>
    [Pure]
    MethodInfo? GetMethod(string name);

    /// <summary>
    /// Gets the cached method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameterTypes">An array of parameter types.</param>
    /// <returns>The CachedMethod corresponding to the given name and parameter types.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type[] parameterTypes);

    /// <summary>
    /// Gets the cached method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="t0">The first parameter type.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0);

    /// <summary>
    /// Gets the cached method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1);

    /// <summary>
    /// Gets the cached method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets the cached method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <param name="t3">The fourth parameter type.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets the cached method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="cachedParameterTypes">The cached parameter types.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string name, CachedType[] cachedParameterTypes);

    /// <summary>
    /// Gets the reflection metadata for the method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="types">An array of parameter types.</param>
    /// <returns>The MethodInfo corresponding to the given name and parameter types.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type[] types);

    /// <summary>
    /// Gets the reflection metadata for the method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="t0">The first parameter type.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type t0);

    /// <summary>
    /// Gets the reflection metadata for the method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type t0, Type t1);

    /// <summary>
    /// Gets the reflection metadata for the method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets the reflection metadata for the method whose name and parameter types match the supplied signature.
    /// </summary>
    /// <param name="name">The method name.</param>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <param name="t3">The fourth parameter type.</param>
    /// <returns>The matching method, or <c>null</c> when no method has that signature.</returns>
    [Pure]
    MethodInfo? GetMethod(string name, Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets all cached methods in the configured reflection scope.
    /// </summary>
    /// <returns>The cached methods.</returns>
    [Pure]
    CachedMethod[] GetCachedMethods();

    /// <summary>
    /// Gets the reflection metadata for all methods in the configured reflection scope.
    /// </summary>
    /// <returns>The method metadata.</returns>
    [Pure]
    MethodInfo?[] GetMethods();
}