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
    Type? MakeGenericType(params Type[] typeArguments);
}