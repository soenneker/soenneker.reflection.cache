using System.Reflection;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the property infos extension.
/// </summary>
public static class PropertyInfosExtension
{
    /// <summary>
    /// Executes the to cached properties operation.
    /// </summary>
    /// <param name="properties">The properties.</param>
    /// <param name="cachedTypes">The cached types.</param>
    /// <param name="threadSafe">The thread safe.</param>
    /// <returns>The result of the operation.</returns>
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
