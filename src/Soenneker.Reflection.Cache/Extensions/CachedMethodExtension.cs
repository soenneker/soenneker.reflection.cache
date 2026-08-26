using System;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides signature helpers for cached methods.
/// </summary>
public static class CachedMethodExtension
{
    /// <summary>
    /// Computes a hash key from the method name and parameter types.
    /// </summary>
    /// <param name="cachedMethod">The cached method.</param>
    /// <returns>A hash representing the method signature.</returns>
    public static int ToHashKey(this CachedMethod cachedMethod)
    {
        Type[] parameterTypes = cachedMethod.GetCachedParameters().GetParameterTypes();

        return ReflectionCacheUtil.GetCacheKeyForMethod(cachedMethod.Name!, parameterTypes);
    }
}
