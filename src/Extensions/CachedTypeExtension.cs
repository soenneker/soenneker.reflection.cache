using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedTypeExtension
{
    public static bool IsDerivedFromType(this CachedType sourceCachedType, CachedType targetCachedType)
    {
        CachedType? tempCachedType = sourceCachedType;

        while (tempCachedType != null)
        {
            // Check if it's non-generic
            if (targetCachedType.IsAssignableFrom(tempCachedType))
                return true;

            // Check if it's generic type (collection<T>)
            if (tempCachedType.IsGenericType && tempCachedType.GetCachedGenericTypeDefinition() == targetCachedType)
                return true;

            tempCachedType = tempCachedType.CachedBaseType;

            if (tempCachedType?.Type == typeof(object))
                return false;
        }

        return false;
    }

    public static bool IsDerivedFromType(this CachedType sourceCachedType, CachedType t0, CachedType t1)
    {
        CachedType? tempCachedType = sourceCachedType;

        while (tempCachedType != null)
        {
            if (t0.IsAssignableFrom(tempCachedType) || t1.IsAssignableFrom(tempCachedType))
                return true;

            if (tempCachedType.IsGenericType)
            {
                CachedType? def = tempCachedType.GetCachedGenericTypeDefinition();
                if (def == t0 || def == t1)
                    return true;
            }

            tempCachedType = tempCachedType.CachedBaseType;

            if (tempCachedType?.Type == typeof(object))
                return false;
        }

        return false;
    }

    public static bool IsDerivedFromType(this CachedType sourceCachedType, CachedType t0, CachedType t1, CachedType t2)
    {
        CachedType? tempCachedType = sourceCachedType;

        while (tempCachedType != null)
        {
            if (t0.IsAssignableFrom(tempCachedType) || t1.IsAssignableFrom(tempCachedType) || t2.IsAssignableFrom(tempCachedType))
                return true;

            if (tempCachedType.IsGenericType)
            {
                CachedType? def = tempCachedType.GetCachedGenericTypeDefinition();
                if (def == t0 || def == t1 || def == t2)
                    return true;
            }

            tempCachedType = tempCachedType.CachedBaseType;

            if (tempCachedType?.Type == typeof(object))
                return false;
        }

        return false;
    }

    public static bool IsDerivedFromType(this CachedType sourceCachedType, CachedType t0, CachedType t1, CachedType t2, CachedType t3)
    {
        CachedType? tempCachedType = sourceCachedType;

        while (tempCachedType != null)
        {
            if (t0.IsAssignableFrom(tempCachedType) || t1.IsAssignableFrom(tempCachedType) || t2.IsAssignableFrom(tempCachedType) || t3.IsAssignableFrom(tempCachedType))
                return true;

            if (tempCachedType.IsGenericType)
            {
                CachedType? def = tempCachedType.GetCachedGenericTypeDefinition();
                if (def == t0 || def == t1 || def == t2 || def == t3)
                    return true;
            }

            tempCachedType = tempCachedType.CachedBaseType;

            if (tempCachedType?.Type == typeof(object))
                return false;
        }

        return false;
    }

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
                if (targetCachedType.IsAssignableFrom(tempCachedType))
                    return true;

                // Check if it's generic type (collection<T>)
                if (tempCachedType.IsGenericType && tempCachedType.GetCachedGenericTypeDefinition() == targetCachedType)
                    return true;
            }

            tempCachedType = tempCachedType.CachedBaseType;

            if (tempCachedType?.Type == typeof(object))
                return false;
        }

        return false;
    }
}