using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Constructors.Abstract;

/// <summary>
/// Provides cached constructor lookup and optimized instance creation for a type.
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
    /// Gets the cached constructor whose parameter types match the supplied signature.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0);

    /// <summary>
    /// Gets the cached constructor whose parameter types match the supplied signature.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1);

    /// <summary>
    /// Gets the cached constructor whose parameter types match the supplied signature.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets the cached constructor whose parameter types match the supplied signature.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <param name="t3">The fourth parameter type.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
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
    /// Gets the reflection metadata for the constructor whose parameter types match the supplied signature.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0);

    /// <summary>
    /// Gets the reflection metadata for the constructor whose parameter types match the supplied signature.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1);

    /// <summary>
    /// Gets the reflection metadata for the constructor whose parameter types match the supplied signature.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets the reflection metadata for the constructor whose parameter types match the supplied signature.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <param name="t3">The fourth parameter type.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets all cached constructors in the configured reflection scope.
    /// </summary>
    /// <returns>The cached constructors.</returns>
    [Pure]
    CachedConstructor[] GetCachedConstructors();

    /// <summary>
    /// Gets the reflection metadata for all constructors in the configured reflection scope.
    /// </summary>
    /// <returns>The constructor metadata.</returns>
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
    /// Creates an instance using the constructor that matches the supplied arguments.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    object? CreateInstance(object? arg0);

    /// <summary>
    /// Creates an instance using the constructor that matches the supplied arguments.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    object? CreateInstance(object? arg0, object? arg1);

    /// <summary>
    /// Creates an instance using the constructor that matches the supplied arguments.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    object? CreateInstance(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Creates an instance using the constructor that matches the supplied arguments.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <param name="arg3">The fourth constructor argument.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
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
    /// Creates an instance using the constructor that matches the supplied arguments.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0);

    /// <summary>
    /// Creates an instance using the constructor that matches the supplied arguments.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1);

    /// <summary>
    /// Creates an instance using the constructor that matches the supplied arguments.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Creates an instance using the constructor that matches the supplied arguments.
    /// </summary>
    /// <typeparam name="T">The type to which the created instance is cast.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <param name="arg3">The fourth constructor argument.</param>
    /// <returns>The matching constructor or created instance; <c>null</c> when no match is available.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1, object? arg2, object? arg3);
}