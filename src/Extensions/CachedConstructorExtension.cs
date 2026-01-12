using System;
using Soenneker.Extensions.Spans.Readonly.Types;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedConstructorExtension
{
    public static int ToHashKey(this CachedConstructor cachedConstructor)
    {
        Type[] parameterTypes = cachedConstructor.GetParametersTypes();

        return parameterTypes.ToHashKey();
    }
}