using Soenneker.Reflection.Cache.Attributes;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides conversions for arrays of cached attributes.
/// </summary>
public static class CachedAttributesExtension
{
    /// <summary>
    /// Extracts the underlying attribute instances.
    /// </summary>
    /// <param name="cachedAttributes">The cached attributes.</param>
    /// <returns>An array containing each cached attribute instance in the original order.</returns>
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
