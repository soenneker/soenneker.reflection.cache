using System.Diagnostics.Contracts;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Parameters;

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
    /// Gets the cached custom attributes for this constructor.
    /// </summary>
    [Pure]
    CachedCustomAttributes? Attributes { get; }

    /// <summary>
    /// Gets the cached parameters for this constructor.
    /// </summary>
    [Pure]
    CachedParameters? Parameters { get; }

    [Pure]
    CachedParameter[] GetCachedParameters();

    [Pure]
    ParameterInfo[] GetParameters();

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
}