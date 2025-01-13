using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedTypeExtension
{
    public static bool IsDerivedFromType(this CachedType sourceCachedType, params CachedType[] targetCachedTypes)
    {
        CachedType? tempCachedType = sourceCachedType;

        int length = targetCachedTypes.Length;

        while (tempCachedType != null)
        {
            for (var index = 0; index < length; index++)
            {
                CachedType targetCachedType = targetCachedTypes[index];
                // Check if it's non-generic
                if (targetCachedType.IsAssignableFrom(sourceCachedType))
                    return true;

                // Check if it's generic type (collection<T>)
                if (sourceCachedType.IsGenericType && sourceCachedType.GetCachedGenericTypeDefinition() == targetCachedType)
                    return true;
            }

            tempCachedType = sourceCachedType.CachedBaseType;

            if (tempCachedType.Type == typeof(object))
                return false;
        }

        return false;
    }
}