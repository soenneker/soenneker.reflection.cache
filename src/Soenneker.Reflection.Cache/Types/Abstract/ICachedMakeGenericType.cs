using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Provides cached construction of closed generic types from a generic type definition.
/// </summary>
public interface ICachedMakeGenericType
{
    /// <summary>
    /// Constructs or retrieves a cached closed generic type using reflection type arguments.
    /// </summary>
    /// <param name="typeArguments">The type arguments.</param>
    /// <returns>The cached closed generic type, or <c>null</c> when the source is not a generic type definition.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(params Type[] typeArguments);

    /// <summary>
    /// Constructs or retrieves a cached closed generic type using cached type arguments.
    /// </summary>
    /// <param name="cachedTypeArguments">The cached generic type arguments.</param>
    /// <returns>The cached closed generic type, or <c>null</c> when the source is not a generic type definition.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(params CachedType[] cachedTypeArguments);

    /// <summary>
    /// Constructs or retrieves a cached closed generic type with one generic argument.
    /// </summary>
    /// <param name="t0">The first generic type argument.</param>
    /// <returns>The cached closed generic type, or <c>null</c> when construction is unavailable.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0);

    /// <summary>
    /// Constructs or retrieves a cached closed generic type with two generic arguments.
    /// </summary>
    /// <param name="t0">The first generic type argument.</param>
    /// <param name="t1">The second generic type argument.</param>
    /// <returns>The cached closed generic type, or <c>null</c> when construction is unavailable.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1);

    /// <summary>
    /// Constructs or retrieves a cached closed generic type with three generic arguments.
    /// </summary>
    /// <param name="t0">The first generic type argument.</param>
    /// <param name="t1">The second generic type argument.</param>
    /// <param name="t2">The third generic type argument.</param>
    /// <returns>The cached closed generic type, or <c>null</c> when construction is unavailable.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1, CachedType t2);

    /// <summary>
    /// Constructs or retrieves a cached closed generic type with four generic arguments.
    /// </summary>
    /// <param name="t0">The first generic type argument.</param>
    /// <param name="t1">The second generic type argument.</param>
    /// <param name="t2">The third generic type argument.</param>
    /// <param name="t3">The fourth generic type argument.</param>
    /// <returns>The cached closed generic type, or <c>null</c> when construction is unavailable.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1, CachedType t2, CachedType t3);

    /// <summary>
    /// Constructs or retrieves a closed generic reflection type.
    /// </summary>
    /// <param name="typeArguments">The type arguments.</param>
    /// <returns>The closed generic type, or <c>null</c> when the source is not a generic type definition.</returns>
    [Pure]
    Type? MakeGenericType(params Type[] typeArguments);
}
