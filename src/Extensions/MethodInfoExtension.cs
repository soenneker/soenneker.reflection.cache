using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class MethodInfoExtension
{
    internal static string ToOriginalMemberName(this MethodInfo methodInfo)
    {
        string methodName = methodInfo.Name;

        // Check if it's a property getter or setter and remove "get_" or "set_"
        if (methodInfo.IsSpecialName)
        {
            ReadOnlySpan<char> methodSpan = methodName.AsSpan();
            int underscoreIndex = methodSpan.IndexOf('_');

            if (underscoreIndex >= 0 && underscoreIndex < methodSpan.Length - 1)
            {
                methodName = methodSpan.Slice(underscoreIndex + 1).ToString();
            }
        }

        return methodName;
    }
}