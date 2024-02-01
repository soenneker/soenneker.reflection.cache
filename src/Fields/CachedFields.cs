using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Fields.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Fields;

///<inheritdoc cref="ICachedFields"/>
public class CachedFields : ICachedFields
{
    private readonly Lazy<Dictionary<int, FieldInfo?>> _cachedDict;
    private readonly Lazy<FieldInfo[]> _cachedArray;

    private readonly CachedType _cachedType;

    public CachedFields(CachedType cachedType, bool threadSafe = true)
    {
        _cachedType = cachedType;

        _cachedDict = new Lazy<Dictionary<int, FieldInfo?>>(SetDict, threadSafe);
        _cachedArray = new Lazy<FieldInfo[]>(SetArray, threadSafe);
    }

    public FieldInfo? GetField(string name)
    {
        return _cachedDict.Value.GetValueOrDefault(name.GetHashCode());
    }

    private Dictionary<int, FieldInfo?> SetDict()
    {
        var dict = new Dictionary<int, FieldInfo?>();

        // If the array is already populated, build the dictionary from the array
        if (_cachedArray.IsValueCreated)
        {
            foreach (FieldInfo Field in _cachedArray.Value)
            {
                dict[Field.Name.GetHashCode()] = Field;
            }
        }
        else
        {
            // If the array is not populated, build the dictionary directly
            FieldInfo[] fields = _cachedType.Type!.GetFields(ReflectionCacheConstants.BindingFlags);

            foreach (FieldInfo Field in fields)
            {
                dict[Field.Name.GetHashCode()] = Field;
            }
        }

        return dict;
    }

    private FieldInfo[] SetArray()
    {
        if (_cachedDict.IsValueCreated)
        {
            Dictionary<int, FieldInfo?>.ValueCollection values = _cachedDict.Value.Values;
            int count = values.Count;
            var result = new FieldInfo[count];
            values.CopyTo(result, 0);
            return result;
        }

        return _cachedType.Type!.GetFields(ReflectionCacheConstants.BindingFlags);
    }

    public FieldInfo[] GetFields()
    {
        return _cachedArray.Value;
    }
}
