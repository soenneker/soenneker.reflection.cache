using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

public static class PropertyInfoExtension
{
    // TODO: Move to CachedProperty
    [Pure]
    public static bool IsConstant(this PropertyInfo property)
    {
        MethodInfo? getMethod = property.GetMethod;

        if (getMethod == null)
        {
            // If there's no getter, it's not a constant property
            return false;
        }

        if (!getMethod.IsStatic)
        {
            return false;
        }

        // Check if the property is readonly
        if (!getMethod.IsPublic || property.SetMethod != null)
        {
            return false;
        }

        // If the property meets the above conditions, it's likely a constant
        return true;
    }
}