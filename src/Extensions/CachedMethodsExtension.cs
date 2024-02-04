using Soenneker.Reflection.Cache.Methods;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedMethodsExtension
{
    public static MethodInfo?[] ToMethods(this CachedMethod[] cachedMethods)
    {
        ReadOnlySpan<CachedMethod> span = cachedMethods;

        var methodInfoArray = new MethodInfo?[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            methodInfoArray[i] = span[i].MethodInfo;
        }

        return methodInfoArray;
    }
}