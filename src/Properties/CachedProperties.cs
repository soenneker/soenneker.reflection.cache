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

    public CachedProperties(CachedType cachedType)
    {
        _cachedType = cachedType;

        _cachedDict = new Lazy<Dictionary<int, PropertyInfo?>>(SetDict, true);
        _cachedArray = new Lazy<PropertyInfo[]>(SetArray, true);
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
        // Use the dictionary if it's already populated
        if (_cachedDict.IsValueCreated)
        {
            return new List<PropertyInfo>(_cachedDict.Value.Values).ToArray();
        }

        return _cachedType.Type!.GetProperties(ReflectionCacheConstants.BindingFlags);
    }

    public PropertyInfo[] GetProperties()
    {
        return _cachedArray.Value;
    }
}