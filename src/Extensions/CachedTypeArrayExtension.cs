using System;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedTypeArrayExtension
{
    public static int ToHashKey(this CachedType[]? cachedTypes)
    {
        if (cachedTypes == null)
            return 0;

        int length = cachedTypes.Length;

        if (length == 0)
            return 0;

        var hash = new HashCode();

        for (var i = 0; i < length; i++)
        {
            CachedType type = cachedTypes[i];
            hash.Add(type);
        }

        return hash.ToHashCode();
    }

    public static Type[] ToTypes(this CachedType[] cachedTypes)
    {
        int length = cachedTypes.Length;
        var result = new Type[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = cachedTypes[i].Type!;
        }

        return result;
    }
}