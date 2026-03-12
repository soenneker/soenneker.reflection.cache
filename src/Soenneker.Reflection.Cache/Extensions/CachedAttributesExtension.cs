using Soenneker.Reflection.Cache.Attributes;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedAttributesExtension
{
    public static object[] ToObjects(this CachedAttribute[] cachedAttributes)
    {
        int length = cachedAttributes.Length;
        var result = new object[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = cachedAttributes[i].Attribute;
        }

        return result;
    }
}