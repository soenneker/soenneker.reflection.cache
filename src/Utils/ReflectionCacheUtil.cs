using System;
using Soenneker.Extensions.Type.Array;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Utils;

public static class ReflectionCacheUtil
{
    public static int GetCacheKeyForMethod(string methodName, Type[]? parameterTypes = null)
    {
        int methodNameKey = methodName.GetHashCode();

        if (parameterTypes == null || parameterTypes.Length == 0)
            return methodNameKey;

        int arrayKey = parameterTypes.ToHashKey();

        return methodNameKey + arrayKey;
    }

    public static int GetCacheKeyForMethod(string methodName, CachedType[]? parameterTypes)
    {
        int methodNameKey = methodName.GetHashCode();

        if (parameterTypes == null || parameterTypes.Length == 0)
            return methodNameKey;

        int arrayKey = parameterTypes.ToHashKey();

        return methodNameKey + arrayKey;
    }
}