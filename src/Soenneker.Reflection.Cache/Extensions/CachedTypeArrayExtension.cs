using System;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides hashing and conversion helpers for arrays of cached types.
/// </summary>
public static class CachedTypeArrayExtension
{
    /// <summary>
    /// Computes an order-sensitive hash for the cached type sequence.
    /// </summary>
    /// <param name="cachedTypes">The cached types.</param>
    /// <returns>The sequence hash, or <c>0</c> for a null or empty array.</returns>
    public static int ToHashKey(this CachedType[]? cachedTypes)
    {
        if (cachedTypes == null)
            return 0;

        int length = cachedTypes.Length;

        if (length == 0)
            return 0;

        var hash = new HashCode();

        for (var i = 0; i < length; i++)
        {
            CachedType type = cachedTypes[i];
            hash.Add(type);
        }

        return hash.ToHashCode();
    }

    /// <summary>
    /// Extracts the underlying reflection types.
    /// </summary>
    /// <param name="cachedTypes">The cached types.</param>
    /// <returns>The reflection types in the original order.</returns>
    public static Type[] ToTypes(this CachedType[] cachedTypes)
    {
        int length = cachedTypes.Length;

        if (length == 0)
            return [];

        var result = new Type[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = cachedTypes[i].Type!;
        }

        return result;
    }
}
