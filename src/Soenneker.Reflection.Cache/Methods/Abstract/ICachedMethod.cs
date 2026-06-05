using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Methods.Abstract;

/// <summary>
/// Represents a cached method for a type.
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
    /// Gets cached parameters.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    CachedParameters? GetCachedParameters();

    /// <summary>
    /// Gets parameters.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    ParameterInfo[] GetParameters();

    /// <summary>
    /// Gets cached custom attributes.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    CachedCustomAttributes? GetCachedCustomAttributes();

    /// <summary>
    /// Gets cached custom attribute.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="inherit">The inherit.</param>
    /// <returns>The result of the operation.</returns>
    T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute;

    /// <summary>
    /// Executes the make cached generic method operation.
    /// </summary>
    /// <param name="cachedTypes">The cached types.</param>
    /// <returns>The result of the operation.</returns>
    CachedMethod? MakeCachedGenericMethod(params CachedType[] cachedTypes);

    /// <summary>
    /// Executes the make cached generic method operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    CachedMethod? MakeCachedGenericMethod(CachedType t0);

    /// <summary>
    /// Executes the make cached generic method operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1);

    /// <summary>
    /// Executes the make cached generic method operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1, CachedType t2);

    /// <summary>
    /// Executes the make cached generic method operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1, CachedType t2, CachedType t3);

    /// <summary>
    /// Gets custom attributes.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    object[] GetCustomAttributes();

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <returns>The result of the operation.</returns>
    object? Invoke(object instance);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="param">The param.</param>
    /// <returns>The result of the operation.</returns>
    object? Invoke(object instance, params object[] param);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="arg0">The arg0.</param>
    /// <returns>The result of the operation.</returns>
    object? Invoke(object instance, object? arg0);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The result of the operation.</returns>
    object? Invoke(object instance, object? arg0, object? arg1);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns>The result of the operation.</returns>
    object? Invoke(object instance, object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns>The result of the operation.</returns>
    object? Invoke(object instance, object? arg0, object? arg1, object? arg2, object? arg3);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <returns>The result of the operation.</returns>
    T? Invoke<T>(object instance);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="param">The param.</param>
    /// <returns>The result of the operation.</returns>
    T? Invoke<T>(params object[] param);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="arg0">The arg0.</param>
    /// <returns>The result of the operation.</returns>
    T? Invoke<T>(object instance, object? arg0);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The result of the operation.</returns>
    T? Invoke<T>(object instance, object? arg0, object? arg1);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns>The result of the operation.</returns>
    T? Invoke<T>(object instance, object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Executes the invoke operation.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns>The result of the operation.</returns>
    T? Invoke<T>(object instance, object? arg0, object? arg1, object? arg2, object? arg3);
}