using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Constructors.Abstract;

/// <summary>
/// Provides cached metadata and optimized invocation for a constructor.
/// </summary>
public interface ICachedConstructor
{
    /// <summary>
    /// Gets the <see cref="ConstructorInfo"/> associated with this cached constructor.
    /// </summary>
    [Pure]
    ConstructorInfo? ConstructorInfo { get; }

    /// <summary>
    /// Gets the cached parameter metadata for the constructor.
    /// </summary>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    CachedParameter[] GetCachedParameters();

    /// <summary>
    /// Gets the reflection parameter metadata for the constructor.
    /// </summary>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    ParameterInfo[] GetParameters();

    /// <summary>
    /// Gets the cached custom attributes applied to the constructor.
    /// </summary>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    CachedAttribute[] GetCachedCustomAttributes();

    /// <summary>
    /// Gets the custom attributes applied to the constructor.
    /// </summary>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    object[] GetCustomAttributes();

    /// <summary>
    /// Gets the first custom attribute of the specified type applied to the constructor.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="inherit">Whether to search inherited attribute definitions.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute;

    /// <summary>
    /// Gets the constructor parameter types.
    /// </summary>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    Type[] GetParametersTypes();

    /// <summary>
    /// Gets the cached constructor parameter types.
    /// </summary>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    CachedType[] GetCachedParameterTypes();

    /// <summary>
    /// Invokes the constructor with no parameters.
    /// </summary>
    /// <returns>The result of invoking the constructor.</returns>
    [Pure] 
    object? Invoke();

    /// <summary>
    /// Invokes the constructor with the specified parameters.
    /// </summary>
    /// <param name="param">The parameters for the constructor.</param>
    /// <returns>The result of invoking the constructor.</returns>
    [Pure]
    object? Invoke(params object[] param);

    /// <summary>
    /// Invokes the constructor.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    object? Invoke(object? arg0);

    /// <summary>
    /// Invokes the constructor.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    object? Invoke(object? arg0, object? arg1);

    /// <summary>
    /// Invokes the constructor.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    object? Invoke(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Invokes the constructor.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <param name="arg3">The fourth constructor argument.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    object? Invoke(object? arg0, object? arg1, object? arg2, object? arg3);

    /// <summary>
    /// Invokes the constructor and casts the result to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <returns>The result of invoking the constructor cast to type <typeparamref name="T"/>.</returns>
    [Pure]
    T? Invoke<T>();

    /// <summary>
    /// Invokes the constructor with the specified parameters and casts the result to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="param">The parameters for the constructor.</param>
    /// <returns>The result of invoking the constructor cast to type <typeparamref name="T"/>.</returns>
    [Pure]
    T? Invoke<T>(params object[] param);

    /// <summary>
    /// Invokes the constructor.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    T? Invoke<T>(object? arg0);

    /// <summary>
    /// Invokes the constructor.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    T? Invoke<T>(object? arg0, object? arg1);

    /// <summary>
    /// Invokes the constructor.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    T? Invoke<T>(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Invokes the constructor.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <param name="arg3">The fourth constructor argument.</param>
    /// <returns>The created instance, or <c>null</c> when no constructor is available.</returns>
    [Pure]
    T? Invoke<T>(object? arg0, object? arg1, object? arg2, object? arg3);
}