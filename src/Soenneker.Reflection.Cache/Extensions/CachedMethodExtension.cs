using System;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the cached method extension.
/// </summary>
public static class CachedMethodExtension
{
    /// <summary>
    /// Executes the to hash key operation.
    /// </summary>
    /// <param name="cachedMethod">The cached method.</param>
    /// <returns>The result of the operation.</returns>
    public static int ToHashKey(this CachedMethod cachedMethod)
    {
        Type[] parameterTypes = cachedMethod.GetCachedParameters().GetParameterTypes();

        return ReflectionCacheUtil.GetCacheKeyForMethod(cachedMethod.Name!, parameterTypes);
    }
}