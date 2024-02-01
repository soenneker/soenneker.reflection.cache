using System;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Interfaces.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Interfaces;

///<inheritdoc cref="ICachedInterfaces"/>
public class CachedInterfaces : ICachedInterfaces
{
    private readonly Lazy<Dictionary<int, CachedType?>> _cachedDict;
    private readonly Lazy<CachedType[]> _cachedArray;

    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;

    private readonly Lazy<Type[]> _cachedTypesArray;

    public CachedInterfaces(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType;
        _cachedTypes = cachedTypes;
        _cachedDict = new Lazy<Dictionary<int, CachedType?>>(SetDict, threadSafe);
        _cachedArray = new Lazy<CachedType[]>(SetArray, threadSafe);

        _cachedTypesArray = new Lazy<Type[]>(() => GetCachedInterfaces().ToTypes(), threadSafe);
    }

    public CachedType GetCachedInterface(string typeName)
    {
        int hashCode = typeName.GetHashCode();

        if (_cachedDict.Value.TryGetValue(hashCode, out CachedType? result))
            return result;

        Type? type = _cachedType.Type!.GetInterface(typeName);

        result = _cachedTypes.GetCachedType(type);
        _cachedDict.Value[hashCode] = result;

        return result;
    }

    public Type? GetInterface(string typeName)
    {
        return GetCachedInterface(typeName).Type;
    }

    private Dictionary<int, CachedType?> SetDict()
    {
        var dict = new Dictionary<int, CachedType?>();

        if (_cachedArray.IsValueCreated)
        {
            CachedType[]? cachedArrayValue = _cachedArray.Value;

            for (var i = 0; i < cachedArrayValue.Length; i++)
            {
                CachedType cachedType = cachedArrayValue[i];
                int key = cachedType.Type!.FullName!.GetHashCode();
                dict[key] = cachedType;
            }

            return dict;
        }

        Type[] interfaces = _cachedType.Type!.GetInterfaces();

        for (var i = 0; i < interfaces.Length; i++)
        {
            CachedType cachedType = _cachedTypes.GetCachedType(interfaces[i]);

            int key = cachedType.Type.FullName.GetHashCode();
            dict[key] = cachedType;
        }

        return dict;
    }

    private CachedType[] SetArray()
    {
        if (_cachedDict.IsValueCreated)
        {
            Dictionary<int, CachedType?>.ValueCollection cachedDictValues = _cachedDict.Value.Values;
            var resultArray = new CachedType[cachedDictValues.Count];
            var i = 0;

            foreach (CachedType? entry in cachedDictValues)
            {
                if (entry != null)
                    resultArray[i++] = entry;
            }

            return resultArray;
        }

        Type[] interfaces = _cachedType.Type!.GetInterfaces();
        var result = new CachedType[interfaces.Length];

        for (var i = 0; i < interfaces.Length; i++)
        {
            result[i] = _cachedTypes.GetCachedType(interfaces[i]);
        }

        return result;
    }

    public CachedType[] GetCachedInterfaces()
    {
        return _cachedArray.Value;
    }

    public Type[] GetInterfaces()
    {
        return _cachedTypesArray.Value;
    }
}