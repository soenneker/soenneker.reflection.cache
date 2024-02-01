using System;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedConstructorExtension
{
    public static int ToCacheKey(this CachedConstructor cachedConstructor)
    {
        Type[] parameterTypes = cachedConstructor.GetParametersTypes();

        return parameterTypes.ToCacheKey();
    }
}