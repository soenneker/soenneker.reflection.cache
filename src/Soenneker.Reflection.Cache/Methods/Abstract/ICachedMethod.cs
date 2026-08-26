using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Methods.Abstract;

/// <summary>
/// Provides cached metadata, attributes, generic construction, and invocation for a method.
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
    /// Gets the cached parameter metadata for the method.
    /// </summary>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    CachedParameters? GetCachedParameters();

    /// <summary>
    /// Gets the reflection parameter metadata for the method.
    /// </summary>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    ParameterInfo[] GetParameters();

    /// <summary>
    /// Gets the cached custom attributes applied to the method.
    /// </summary>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    CachedCustomAttributes? GetCachedCustomAttributes();

    /// <summary>
    /// Gets the first custom attribute of the specified type applied to the method.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result is cast.</typeparam>
    /// <param name="inherit">Whether to search the method inheritance chain.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute;

    /// <summary>
    /// Constructs and caches a closed generic method using the specified generic type arguments.
    /// </summary>
    /// <param name="cachedTypes">The cached generic type arguments.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    CachedMethod? MakeCachedGenericMethod(params CachedType[] cachedTypes);

    /// <summary>
    /// Constructs and caches a closed generic method using the specified generic type arguments.
    /// </summary>
    /// <param name="t0">The first generic type argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    CachedMethod? MakeCachedGenericMethod(CachedType t0);

    /// <summary>
    /// Constructs and caches a closed generic method using the specified generic type arguments.
    /// </summary>
    /// <param name="t0">The first generic type argument.</param>
    /// <param name="t1">The second generic type argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1);

    /// <summary>
    /// Constructs and caches a closed generic method using the specified generic type arguments.
    /// </summary>
    /// <param name="t0">The first generic type argument.</param>
    /// <param name="t1">The second generic type argument.</param>
    /// <param name="t2">The third generic type argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1, CachedType t2);

    /// <summary>
    /// Constructs and caches a closed generic method using the specified generic type arguments.
    /// </summary>
    /// <param name="t0">The first generic type argument.</param>
    /// <param name="t1">The second generic type argument.</param>
    /// <param name="t2">The third generic type argument.</param>
    /// <param name="t3">The fourth generic type argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1, CachedType t2, CachedType t3);

    /// <summary>
    /// Gets the custom attributes applied to the method.
    /// </summary>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    object[] GetCustomAttributes();

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    object? Invoke(object instance);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="param">The arguments to pass to the method.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    object? Invoke(object instance, params object[] param);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="arg0">The first method argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    object? Invoke(object instance, object? arg0);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="arg0">The first method argument.</param>
    /// <param name="arg1">The second method argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    object? Invoke(object instance, object? arg0, object? arg1);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="arg0">The first method argument.</param>
    /// <param name="arg1">The second method argument.</param>
    /// <param name="arg2">The third method argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    object? Invoke(object instance, object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="arg0">The first method argument.</param>
    /// <param name="arg1">The second method argument.</param>
    /// <param name="arg2">The third method argument.</param>
    /// <param name="arg3">The fourth method argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    object? Invoke(object instance, object? arg0, object? arg1, object? arg2, object? arg3);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result is cast.</typeparam>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    T? Invoke<T>(object instance);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result is cast.</typeparam>
    /// <param name="param">The arguments to pass to the method.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    T? Invoke<T>(params object[] param);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result is cast.</typeparam>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="arg0">The first method argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    T? Invoke<T>(object instance, object? arg0);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result is cast.</typeparam>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="arg0">The first method argument.</param>
    /// <param name="arg1">The second method argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    T? Invoke<T>(object instance, object? arg0, object? arg1);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result is cast.</typeparam>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="arg0">The first method argument.</param>
    /// <param name="arg1">The second method argument.</param>
    /// <param name="arg2">The third method argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    T? Invoke<T>(object instance, object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Invokes the cached method.
    /// </summary>
    /// <typeparam name="T">The type to which the invocation result is cast.</typeparam>
    /// <param name="instance">The target instance, or <c>null</c> for a static method.</param>
    /// <param name="arg0">The first method argument.</param>
    /// <param name="arg1">The second method argument.</param>
    /// <param name="arg2">The third method argument.</param>
    /// <param name="arg3">The fourth method argument.</param>
    /// <returns>The requested cached value or invocation result; <c>null</c> when no value is available.</returns>
    T? Invoke<T>(object instance, object? arg0, object? arg1, object? arg2, object? arg3);
}