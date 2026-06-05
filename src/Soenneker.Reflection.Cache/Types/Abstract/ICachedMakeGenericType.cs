using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Defines the cached make generic type contract.
/// </summary>
public interface ICachedMakeGenericType
{
    /// <summary>
    /// Executes the make generic cached type operation.
    /// </summary>
    /// <param name="typeArguments">The type arguments.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(params Type[] typeArguments);

    /// <summary>
    /// CachedType overload. Avoids string building and other intermediate work, but still requires a Type[]
    /// for MakeGenericType.
    /// </summary>
    [Pure]
    CachedType? MakeGenericCachedType(params CachedType[] cachedTypeArguments);

    /// <summary>
    /// Executes the make generic cached type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0);

    /// <summary>
    /// Executes the make generic cached type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1);

    /// <summary>
    /// Executes the make generic cached type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1, CachedType t2);

    /// <summary>
    /// Executes the make generic cached type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1, CachedType t2, CachedType t3);

    /// <summary>
    /// Executes the make generic type operation.
    /// </summary>
    /// <param name="typeArguments">The type arguments.</param>
    /// <returns>The result of the operation.</returns>
    [Pure]
    Type? MakeGenericType(params Type[] typeArguments);
}