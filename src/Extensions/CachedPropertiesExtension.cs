using System.Reflection;
using Soenneker.Reflection.Cache.Properties;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedPropertiesExtension
{
    public static PropertyInfo[] ToPropertyInfos(this CachedProperty[] cachedProperties)
    {
        int length = cachedProperties.Length;
        var propertyInfos = new PropertyInfo[length];  // Directly allocate the array

        for (var i = 0; i < length; i++)
        {
            propertyInfos[i] = cachedProperties[i].PropertyInfo;
        }

        return propertyInfos;
    }
}
