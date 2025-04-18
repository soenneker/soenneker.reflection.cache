using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Fields.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Fields;

///<inheritdoc cref="ICachedFields"/>
public sealed class CachedFields : ICachedFields
{
    private readonly Lazy<Dictionary<int, CachedField?>> _cachedDict;
    private readonly Lazy<CachedField[]> _cachedArray;

    private readonly Lazy<FieldInfo[]> _fieldsCache;

    private readonly CachedType _cachedType;

    private readonly CachedTypes _cachedTypes;

    private readonly bool _threadSafe;

    public CachedFields(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;

        _cachedDict = new Lazy<Dictionary<int, CachedField?>>(SetDict, threadSafe);
        _cachedArray = new Lazy<CachedField[]>(SetArray, threadSafe);
        _fieldsCache = new Lazy<FieldInfo[]>(() => GetCachedFields().ToFieldInfos(), threadSafe);
    }

    public FieldInfo? GetField(string name)
    {
        CachedField? cachedField = GetCachedField(name);
        return cachedField?.FieldInfo;
    }

    public CachedField? GetCachedField(string name)
    {
        return _cachedDict.Value.GetValueOrDefault(name.GetHashCode());
    }

    private Dictionary<int, CachedField?> SetDict()
    {
        var dict = new Dictionary<int, CachedField?>();

        // If the array is already populated, build the dictionary from the array
        if (_cachedArray.IsValueCreated)
        {
            int length = _cachedArray.Value.Length;

            for (var index = 0; index < length; index++)
            {
                CachedField cachedField = _cachedArray.Value[index];
                dict[cachedField.FieldInfo.Name.GetHashCode()] = cachedField;
            }
        }
        else
        {
            // If the array is not populated, build the dictionary directly
            FieldInfo[] fields = _cachedType.Type!.GetFields(_cachedTypes.Options.FieldFlags);

            int length = fields.Length;

            for (var i = 0; i < length; i++)
            {
                FieldInfo field = fields[i];
                var cachedField = new CachedField(field, _cachedTypes, _threadSafe);
                dict[field.Name.GetHashCode()] = cachedField;
            }
        }

        return dict;
    }

    private CachedField[] SetArray()
    {
        if (_cachedDict.IsValueCreated)
        {
            Dictionary<int, CachedField?>.ValueCollection values = _cachedDict.Value.Values;
            int count = values.Count;
            var result = new CachedField[count];
            values.CopyTo(result, 0);
            return result;
        }

        FieldInfo[] fields = _cachedType.Type!.GetFields(_cachedTypes.Options.FieldFlags);

        return fields.ToCachedFields(_cachedTypes, _threadSafe);
    }

    public FieldInfo[] GetFields()
    {
        return _fieldsCache.Value;
    }

    public CachedField[] GetCachedFields()
    {
        return _cachedArray.Value;
    }
}
