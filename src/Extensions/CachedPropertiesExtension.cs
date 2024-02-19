using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Properties;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedPropertiesExtension
{
    public static PropertyInfo[] ToPropertyInfos(this CachedProperty[] cachedProperties)
    {
        ReadOnlySpan<CachedProperty> span = cachedProperties;

        var propertyInfos = new PropertyInfo[cachedProperties.Length];

        for (var i = 0; i < span.Length; i++)
        {
            propertyInfos[i] = span[i].PropertyInfo;
        }

        return propertyInfos;
    }
}
