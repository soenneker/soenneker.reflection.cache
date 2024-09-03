using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class PropertyInfosExtension
{
    public static CachedProperty[] ToCachedProperties(this PropertyInfo[] properties, CachedTypes cachedTypes, bool threadSafe)
    {
        ReadOnlySpan<PropertyInfo> propertiesArray = properties;

        var cachedProperties = new CachedProperty[propertiesArray.Length];

        for (var i = 0; i < properties.Length; i++)
        {
            cachedProperties[i] = new CachedProperty(properties[i], cachedTypes, threadSafe);
        }

        return cachedProperties;
    }
}
