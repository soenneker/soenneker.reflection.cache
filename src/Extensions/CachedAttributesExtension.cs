using Soenneker.Reflection.Cache.Attributes;
using System;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedAttributesExtension
{
    public static object[] ToObjects(this CachedAttribute[] cachedAttributes)
    {
        ReadOnlySpan<CachedAttribute> span = cachedAttributes;

        var result = new object[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            result[i] = span[i].Attribute;
        }

        return result;
    }
}