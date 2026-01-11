using System.Collections.Frozen;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Properties;

internal sealed class CachedPropertiesCache
{
    public readonly CachedProperty[] CachedArray;
    public readonly FrozenDictionary<string, CachedProperty> MapByName;
    public readonly PropertyInfo[] PropertyInfos;

    public CachedPropertiesCache(CachedProperty[] cachedArray, FrozenDictionary<string, CachedProperty> mapByName, PropertyInfo[] propertyInfos)
    {
        CachedArray = cachedArray;
        MapByName = mapByName;
        PropertyInfos = propertyInfos;
    }
}

