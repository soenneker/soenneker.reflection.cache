using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Properties.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Properties;

///<inheritdoc cref="ICachedProperties"/>
public class CachedProperties : ICachedProperties
{
    private readonly Lazy<Dictionary<int, PropertyInfo?>> _cachedDict;
    private readonly Lazy<PropertyInfo[]> _cachedArray;

    private readonly CachedType _cachedType;

    public CachedProperties(CachedType cachedType, bool threadSafe = true)
    {
        _cachedType = cachedType;

        _cachedDict = new Lazy<Dictionary<int, PropertyInfo?>>(SetDict, threadSafe);
        _cachedArray = new Lazy<PropertyInfo[]>(SetArray, threadSafe);
    }

    public PropertyInfo? GetProperty(string name)
    {
        return _cachedDict.Value.GetValueOrDefault(name.GetHashCode());
    }

    private Dictionary<int, PropertyInfo?> SetDict()
    {
        var dict = new Dictionary<int, PropertyInfo?>();

        // If the array is already populated, build the dictionary from the array
        if (_cachedArray.IsValueCreated)
        {
            foreach (PropertyInfo property in _cachedArray.Value)
            {
                dict[property.Name.GetHashCode()] = property;
            }
        }
        else
        {
            // If the array is not populated, build the dictionary directly
            PropertyInfo[] properties = _cachedType.Type!.GetProperties(ReflectionCacheConstants.BindingFlags);

            foreach (PropertyInfo property in properties)
            {
                dict[property.Name.GetHashCode()] = property;
            }
        }

        return dict;
    }

    private PropertyInfo[] SetArray()
    {
        if (_cachedDict.IsValueCreated)
        {
            Dictionary<int, PropertyInfo?>.ValueCollection values = _cachedDict.Value.Values;
            int count = values.Count;
            var result = new PropertyInfo[count];
            values.CopyTo(result, 0);
            return result;
        }

        return _cachedType.Type!.GetProperties(ReflectionCacheConstants.BindingFlags);
    }

    public PropertyInfo[] GetProperties()
    {
        return _cachedArray.Value;
    }
}