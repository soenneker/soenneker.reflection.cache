using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Types.Abstract;

public interface ICachedMakeGenericType
{
    [Pure]
    CachedType? MakeGenericCachedType(params Type[] typeArguments);

    [Pure]
    CachedType? MakeGenericCachedType(params CachedType[] cachedTypeArguments);

    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0);

    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1);

    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1, CachedType t2);

    [Pure]
    CachedType? MakeGenericCachedType(CachedType t0, CachedType t1, CachedType t2, CachedType t3);

    [Pure]
    Type? MakeGenericType(params Type[] typeArguments);
}