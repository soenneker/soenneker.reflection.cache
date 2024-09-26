using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class MethodInfoExtension
{
    /// <summary>
    /// Converts the <see cref="MethodInfo"/> instance's name to its original member name,
    /// removing prefixes like "get_" or "set_" if the method represents a property accessor.
    /// </summary>
    /// <param name="methodInfo">The <see cref="MethodInfo"/> instance representing the method to extract the original name from.</param>
    /// <returns>
    /// A <see cref="string"/> containing the original member name, with any accessor prefixes removed.
    /// </returns>
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