using System;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the cached type array extension.
/// </summary>
public static class CachedTypeArrayExtension
{
    /// <summary>
    /// Executes the to hash key operation.
    /// </summary>
    /// <param name="cachedTypes">The cached types.</param>
    /// <returns>The result of the operation.</returns>
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
    /// Executes the to types operation.
    /// </summary>
    /// <param name="cachedTypes">The cached types.</param>
    /// <returns>The result of the operation.</returns>
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