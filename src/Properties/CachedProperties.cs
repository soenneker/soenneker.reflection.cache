using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Properties.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Properties;

///<inheritdoc cref="ICachedProperties"/>
public class CachedProperties : ICachedProperties
{
    private readonly Lazy<Dictionary<int, CachedProperty?>> _cachedDict;
    private readonly Lazy<CachedProperty[]> _cachedArray;

    private readonly Lazy<PropertyInfo[]> _propertiesCache;

    private readonly CachedType _cachedType;

    public CachedProperties(CachedType cachedType, bool threadSafe = true)
    {
        _cachedType = cachedType;

        _cachedDict = new Lazy<Dictionary<int, CachedProperty?>>(SetDict, threadSafe);
        _cachedArray = new Lazy<CachedProperty[]>(SetArray, threadSafe);
        _propertiesCache = new Lazy<PropertyInfo[]>(() => GetCachedProperties().ToPropertyInfos(), threadSafe);
    }

    public PropertyInfo? GetProperty(string name)
    {
        CachedProperty? cachedProperty = GetCachedProperty(name);
        return cachedProperty?.PropertyInfo;
    }

    public CachedProperty? GetCachedProperty(string name)
    {
        return _cachedDict.Value.GetValueOrDefault(name.GetHashCode());
    }

    private Dictionary<int, CachedProperty?> SetDict()
    {
        var dict = new Dictionary<int, CachedProperty?>();

        // If the array is already populated, build the dictionary from the array
        if (_cachedArray.IsValueCreated)
        {
            foreach (CachedProperty cachedProperty in _cachedArray.Value)
            {
                dict[cachedProperty.PropertyInfo.Name.GetHashCode()] = cachedProperty;
            }
        }
        else
        {
            // If the array is not populated, build the dictionary directly
            PropertyInfo[] properties = _cachedType.Type!.GetProperties(ReflectionCacheConstants.BindingFlags);

            foreach (PropertyInfo property in properties)
            {
                var cachedProperty = new CachedProperty(property);
                dict[property.Name.GetHashCode()] = cachedProperty;
            }
        }

        return dict;
    }

    private CachedProperty[] SetArray()
    {
        if (_cachedDict.IsValueCreated)
        {
            Dictionary<int, CachedProperty?>.ValueCollection values = _cachedDict.Value.Values;
            int count = values.Count;
            var result = new CachedProperty[count];
            values.CopyTo(result, 0);
            return result;
        }

        PropertyInfo[] properties = _cachedType.Type!.GetProperties(ReflectionCacheConstants.BindingFlags);

        return properties.ToCachedProperties();
    }

    public PropertyInfo[] GetProperties()
    {
        return _propertiesCache.Value;
    }

    public CachedProperty[] GetCachedProperties()
    {
        return _cachedArray.Value;
    }
}
