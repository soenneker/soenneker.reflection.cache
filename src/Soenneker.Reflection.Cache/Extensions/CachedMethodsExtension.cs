using Soenneker.Reflection.Cache.Methods;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the cached methods extension.
/// </summary>
public static class CachedMethodsExtension
{
    /// <summary>
    /// Executes the to methods operation.
    /// </summary>
    /// <param name="cachedMethods">The cached methods.</param>
    /// <returns>The result of the operation.</returns>
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