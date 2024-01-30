﻿using System;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedMethodExtension
{
    public static int GetCacheKey(this CachedMethod cachedMethod)
    {
        Type[] parameterTypes = cachedMethod.GetCachedParameters().GetParametersTypes();

        return ReflectionCacheUtil.GetCacheKeyForMethod(cachedMethod.Name!, parameterTypes);
    }
}