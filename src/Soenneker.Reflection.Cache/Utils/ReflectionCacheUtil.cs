using System;
using Soenneker.Extensions.Spans.Readonly.Types;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Utils;

/// <summary>
/// Represents the reflection cache util.
/// </summary>
public static class ReflectionCacheUtil
{
    /// <summary>
    /// Gets cache key for method.
    /// </summary>
    /// <param name="methodName">The method name.</param>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>The result of the operation.</returns>
    public static int GetCacheKeyForMethod(string methodName, Type[]? parameterTypes = null)
    {
        int methodNameKey = methodName.GetHashCode();

        if (parameterTypes == null || parameterTypes.Length == 0)
            return methodNameKey;

        int arrayKey = parameterTypes.ToHashKey();

        return methodNameKey + arrayKey;
    }

    /// <summary>
    /// Gets cache key for method with cached parameter types.
    /// </summary>
    /// <param name="methodName">The method name.</param>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>The result of the operation.</returns>
    public static int GetCacheKeyForMethodWithCachedParameterTypes(string methodName, CachedType[]? parameterTypes = null)
    {
        int methodNameKey = methodName.GetHashCode();

        if (parameterTypes == null || parameterTypes.Length == 0)
            return methodNameKey;

        int arrayKey = parameterTypes.ToHashKey();

        return methodNameKey + arrayKey;
    }
}