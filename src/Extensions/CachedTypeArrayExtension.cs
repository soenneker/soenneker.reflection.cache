using System;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedTypeArrayExtension
{
    public static int ToHashKey(this CachedType[]? cachedTypes)
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

    public static Type[] ToTypes(this CachedType[] cachedTypes)
    {
        ReadOnlySpan<CachedType> span = cachedTypes;

        var result = new Type[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            result[i] = span[i].Type!;
        }

        return result;
    }
}