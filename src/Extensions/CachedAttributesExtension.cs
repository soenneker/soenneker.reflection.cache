using Soenneker.Reflection.Cache.Attributes;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedAttributesExtension
{
    public static object[] ToObjects(this CachedAttribute[] cachedAttributes)
    {
        var result = new object[cachedAttributes.Length];

        for (var i = 0; i < cachedAttributes.Length; i++)
        {
            result[i] = cachedAttributes[i].Attribute;
        }

        return result;
    }
}