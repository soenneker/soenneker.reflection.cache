using Soenneker.Reflection.Cache.Types;
using System;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedTypesExtension
{
    public static Type[] ToTypes(this CachedType[] cachedInterfaces)
    {
        ReadOnlySpan<CachedType> span = cachedInterfaces;

        var result = new Type[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            result[i] = span[i].Type!;
        }

        return result;
    }
}