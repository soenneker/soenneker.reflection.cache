using System;
using Soenneker.Reflection.Cache.Extensions;

namespace Soenneker.Reflection.Cache.Utils;

public static class ReflectionCacheUtil
{
    public static int GetCacheKeyForMethod(string methodName, Type[]? parameterTypes = null)
    {
        int methodNameKey = methodName.GetHashCode();

        if (parameterTypes == null || parameterTypes.Length == 0)
            return methodNameKey;

        int arrayKey = parameterTypes.GetCacheKey();

        return methodNameKey + arrayKey;
    }
}