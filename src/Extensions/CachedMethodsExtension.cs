using Soenneker.Reflection.Cache.Methods;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedMethodsExtension
{
    public static MethodInfo?[] ToMethods(this CachedMethod[] cachedMethods)
    {
        int count = cachedMethods.Length;
        var methodInfoArray = new MethodInfo?[count];

        for (var i = 0; i < count; i++)
        {
            methodInfoArray[i] = cachedMethods[i].MethodInfo;
        }

        return methodInfoArray;
    }
}