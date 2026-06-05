using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Constructors.Abstract;

/// <summary>
/// Represents a cached set of constructors for a specific type.
/// </summary>
public interface ICachedConstructors
{
    /// <summary>
    /// Gets the cached constructor for the specified parameter types.
    /// </summary>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>The cached constructor, or <c>null</c> if not found.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type[]? parameterTypes = null);

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0);

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1);

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets the constructor for the specified parameter types.
    /// </summary>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>The constructor, or <c>null</c> if not found.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type[]? parameterTypes = null);

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0);

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1);

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets an array of cached constructors.
    /// </summary>
    /// <returns>An array of cached constructors.</returns>
    [Pure]
    CachedConstructor[] GetCachedConstructors();

    /// <summary>
    /// Gets an array of constructors.
    /// </summary>
    /// <returns>An array of constructors.</returns>
    [Pure]
    ConstructorInfo?[] GetConstructors();

    /// <summary>
    /// Creates an instance of the type with default constructor parameters.
    /// </summary>
    /// <returns>An instance of the type.</returns>
    [Pure]
    object? CreateInstance();

    /// <summary>
    /// Creates an instance of the type with default constructor parameters.
    /// </summary>
    /// <returns>An instance of the type.</returns>
    [Pure]
    T? CreateInstance<T>();

    /// <summary>
    /// Creates an instance of the type with specified parameters.
    /// </summary>
    /// <param name="parameters">The parameters for the constructor.</param>
    /// <returns>An instance of the type.</returns>
    [Pure]
    object? CreateInstance(params object[] parameters);

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    object? CreateInstance(object? arg0);

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    object? CreateInstance(object? arg0, object? arg1);

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    object? CreateInstance(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    object? CreateInstance(object? arg0, object? arg1, object? arg2, object? arg3);

    /// <summary>
    /// Creates an instance of the type with specified parameters and casts it to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="parameters">The parameters for the constructor.</param>
    /// <returns>An instance of the type <typeparamref name="T"/>.</returns>
    [Pure]
    T? CreateInstance<T>(params object[] parameters);

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0);

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1);

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1, object? arg2, object? arg3);
}