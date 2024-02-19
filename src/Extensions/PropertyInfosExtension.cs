using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Properties;

namespace Soenneker.Reflection.Cache.Extensions;

public static class PropertyInfosExtension
{
    public static CachedProperty[] ToCachedProperties(this PropertyInfo[] properties)
    {
        ReadOnlySpan<PropertyInfo> propertiesArray = properties;

        var cachedProperties = new CachedProperty[propertiesArray.Length];

        for (int i = 0; i < properties.Length; i++)
        {
            cachedProperties[i] = new CachedProperty(properties[i]);
        }

        return cachedProperties;
    }
}
