using Soenneker.Reflection.Cache.Types;
using System;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedTypesExtension
{
    public static Type[] ToTypes(this CachedType[] cachedInterfaces)
    {
        var result = new Type[cachedInterfaces.Length];

        for (var i = 0; i < cachedInterfaces.Length; i++)
        {
            result[i] = cachedInterfaces[i].Type!;
        }

        return result;
    }
}