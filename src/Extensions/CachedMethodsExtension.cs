using Soenneker.Reflection.Cache.Methods;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedMethodsExtension
{
    public static MethodInfo?[] ToMethods(this CachedMethod[] cachedMethods)
    {
        int length = cachedMethods.Length;
        var methodInfoArray = new MethodInfo?[length];

        for (var i = 0; i < length; i++)
        {
            methodInfoArray[i] = cachedMethods[i].MethodInfo;
        }

        return methodInfoArray;
    }
}