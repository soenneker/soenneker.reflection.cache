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

        var hash = new HashCode();

        for (var i = 0; i < types.Length; i++)
        {
            Type type = types[i];
            hash.Add(type);
        }

        return hash.ToHashCode();
    }

    public static int ToCacheKey(this CachedType[]? cachedTypes)
    {
        if (cachedTypes == null || cachedTypes.Length == 0)
            return 0;

        var hash = new HashCode();

        for (var i = 0; i < cachedTypes.Length; i++)
        {
            CachedType type = cachedTypes[i];
            hash.Add(type);
        }

        return hash.ToHashCode();
    }
}