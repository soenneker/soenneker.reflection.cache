using System.Reflection;
using Soenneker.Reflection.Cache.Properties;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides conversions for arrays of cached properties.
/// </summary>
public static class CachedPropertiesExtension
{
    /// <summary>
    /// Extracts the underlying property metadata.
    /// </summary>
    /// <param name="cachedProperties">The cached properties.</param>
    /// <returns>The property metadata in the original order.</returns>
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
