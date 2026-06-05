using Soenneker.Reflection.Cache.Attributes;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the cached attributes extension.
/// </summary>
public static class CachedAttributesExtension
{
    /// <summary>
    /// Executes the to objects operation.
    /// </summary>
    /// <param name="cachedAttributes">The cached attributes.</param>
    /// <returns>The result of the operation.</returns>
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