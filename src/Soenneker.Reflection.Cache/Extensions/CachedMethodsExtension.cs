using Soenneker.Reflection.Cache.Methods;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides conversions for arrays of cached methods.
/// </summary>
public static class CachedMethodsExtension
{
    /// <summary>
    /// Extracts the underlying method metadata.
    /// </summary>
    /// <param name="cachedMethods">The cached methods.</param>
    /// <returns>The method metadata in the original order; entries may be <c>null</c>.</returns>
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
