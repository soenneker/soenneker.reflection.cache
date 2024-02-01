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
    /// Gets the constructor for the specified parameter types.
    /// </summary>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>The constructor, or <c>null</c> if not found.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type[]? parameterTypes = null);

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
    /// Creates an instance of the type with specified parameters and casts it to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="parameters">The parameters for the constructor.</param>
    /// <returns>An instance of the type <typeparamref name="T"/>.</returns>
    [Pure]
    T? CreateInstance<T>(params object[] parameters);
}