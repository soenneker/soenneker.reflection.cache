using System;

namespace Soenneker.Reflection.Cache.Extensions;

public static class TypeArrayExtension
{
    public static int GetCacheKey(this Type[]? types)
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