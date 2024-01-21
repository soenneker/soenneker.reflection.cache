using System;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedConstructorExtension
{
    public static int GetCacheKey(this CachedConstructor cachedConstructor)
    {
        Type[]? parameterTypes = cachedConstructor.Parameters?.GetParametersTypes();

        return parameterTypes.GetCacheKey();
    }
}