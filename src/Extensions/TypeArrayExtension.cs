using System;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class TypeArrayExtension
{
    // TODO: consider using a more efficient hash algorithm
    public static int ToCacheKey(this Type[]? types)
    {
        if (types == null || types.Length == 0)
            return 0;

        ReadOnlySpan<Type> span = types;

        var hash = 17;

        for (var i = 0; i < span.Length; i++)
        {
            hash = hash * 31 + span[i].GetHashCode();
        }

        return hash;
    }

    public static int ToCacheKey(this CachedType[]? cachedTypes)
    {
        if (cachedTypes == null || cachedTypes.Length == 0)
            return 0;

        ReadOnlySpan<CachedType> span = cachedTypes;

        var hash = 17;

        for (var i = 0; i < span.Length; i++)
        {
            hash = hash * 31 + span[i].GetHashCode();
        }

        return hash;
    }
}