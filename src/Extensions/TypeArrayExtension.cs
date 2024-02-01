using System;

namespace Soenneker.Reflection.Cache.Extensions;

public static class TypeArrayExtension
{
    // TODO: consider using a more efficient hash algorithm
    public static int ToCacheKey(this Type[]? types)
    {
        if (types == null || types.Length == 0)
            return 0;

        var hash = 17;

        for (var i = 0; i < types.Length; i++)
        {
            hash = hash * 31 + types[i].GetHashCode();
        }

        return hash;
    }
}