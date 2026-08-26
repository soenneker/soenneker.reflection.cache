using System.Reflection;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides conversion helpers for property metadata.
/// </summary>
public static class PropertyInfosExtension
{
    /// <summary>
    /// Creates cached property wrappers for the supplied metadata.
    /// </summary>
    /// <param name="properties">The property metadata to wrap.</param>
    /// <param name="cachedTypes">The type cache shared by the wrappers.</param>
    /// <param name="threadSafe">Whether lazily initialized wrapper state must be thread-safe.</param>
    /// <returns>The cached property wrappers in the original order.</returns>
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
