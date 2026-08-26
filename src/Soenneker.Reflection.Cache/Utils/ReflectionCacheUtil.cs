using System;
using Soenneker.Extensions.Spans.Readonly.Types;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Utils;

/// <summary>
/// Provides deterministic-in-process keys for cached method signatures.
/// </summary>
public static class ReflectionCacheUtil
{
    /// <summary>
    /// Computes a cache key from a method name and reflection parameter types.
    /// </summary>
    /// <param name="methodName">The method name included in the signature.</param>
    /// <param name="parameterTypes">The ordered parameter types, or <c>null</c> for a parameterless signature.</param>
    /// <returns>The combined in-process hash of the method name and parameter sequence.</returns>
    public static int GetCacheKeyForMethod(string methodName, Type[]? parameterTypes = null)
    {
        int methodNameKey = methodName.GetHashCode();

        if (parameterTypes == null || parameterTypes.Length == 0)
            return methodNameKey;

        int arrayKey = parameterTypes.ToHashKey();

        return methodNameKey + arrayKey;
    }

    /// <summary>
    /// Computes a cache key from a method name and cached parameter types.
    /// </summary>
    /// <param name="methodName">The method name included in the signature.</param>
    /// <param name="parameterTypes">The ordered cached parameter types, or <c>null</c> for a parameterless signature.</param>
    /// <returns>The combined in-process hash of the method name and parameter sequence.</returns>
    public static int GetCacheKeyForMethodWithCachedParameterTypes(string methodName, CachedType[]? parameterTypes = null)
    {
        int methodNameKey = methodName.GetHashCode();

        if (parameterTypes == null || parameterTypes.Length == 0)
            return methodNameKey;

        int arrayKey = parameterTypes.ToHashKey();

        return methodNameKey + arrayKey;
    }
}
