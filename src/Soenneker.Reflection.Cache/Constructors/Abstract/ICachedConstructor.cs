using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Constructors.Abstract;

/// <summary>
/// Represents a cached constructor for a type.
/// </summary>
public interface ICachedConstructor
{
    /// <summary>
    /// Gets the <see cref="ConstructorInfo"/> associated with this cached constructor.
    /// </summary>
    [Pure]
    ConstructorInfo? ConstructorInfo { get; }

    /// <summary>
    /// Gets cached parameters.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedParameter[] GetCachedParameters();

    /// <summary>
    /// Gets parameters.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    [Pure]
    ParameterInfo[] GetParameters();

    /// <summary>
    /// Gets cached custom attributes.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedAttribute[] GetCachedCustomAttributes();

    /// <summary>
    /// Gets custom attributes.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    [Pure]
    object[] GetCustomAttributes();

    /// <summary>
    /// Gets cached custom attribute.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="inherit">The inherit.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute;

    /// <summary>
    /// Gets parameters types.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    [Pure]
    Type[] GetParametersTypes();

    /// <summary>
    /// Gets cached parameter types.
    /// </summary>
    /// <returns>The result of the operation.</returns>
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
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    object? Invoke(object? arg0);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    object? Invoke(object? arg0, object? arg1);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    object? Invoke(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns>The result of the operation.</returns>
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
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? Invoke<T>(object? arg0);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? Invoke<T>(object? arg0, object? arg1);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? Invoke<T>(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? Invoke<T>(object? arg0, object? arg1, object? arg2, object? arg3);
}