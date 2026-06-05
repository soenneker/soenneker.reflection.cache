using System.Reflection;
using Soenneker.Reflection.Cache.Properties;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the cached properties extension.
/// </summary>
public static class CachedPropertiesExtension
{
    /// <summary>
    /// Executes the to property infos operation.
    /// </summary>
    /// <param name="cachedProperties">The cached properties.</param>
    /// <returns>The result of the operation.</returns>
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
