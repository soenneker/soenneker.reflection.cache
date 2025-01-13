using System.Reflection;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class PropertyInfosExtension
{
    public static CachedProperty[] ToCachedProperties(this PropertyInfo[] properties, CachedTypes cachedTypes, bool threadSafe)
    {
        int length = properties.Length;
        var cachedProperties = new CachedProperty[length];

        for (var i = 0; i < length; i++)
        {
            cachedProperties[i] = new CachedProperty(properties[i], cachedTypes, threadSafe);
        }

        return cachedProperties;
    }
}
