using System;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Interfaces.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Interfaces;

///<inheritdoc cref="ICachedInterfaces"/>
public class CachedInterfaces : ICachedInterfaces
{
    private readonly Lazy<Dictionary<int, CachedType?>> _cachedDict;
    private readonly Lazy<CachedType[]> _cachedArray;

    private readonly CachedType _cachedType;

    public CachedInterfaces(CachedType cachedType)
    {
        _cachedType = cachedType;

        _cachedDict = new Lazy<Dictionary<int, CachedType?>>(SetDict, true);
        _cachedArray = new Lazy<CachedType[]>(SetArray, true);
    }

    public CachedType GetCachedInterface(string typeName)
    {
        _cachedDict.Value.TryGetValue(typeName.GetHashCode(), out CachedType? result);

        if (result == null)
        {
            Type? type = _cachedType.Type!.GetInterface(typeName);
            result = new CachedType(type);
            _cachedDict.Value[typeName.GetHashCode()] = result;
        }

        return result;
    }

    public Type? GetInterface(string typeName)
    {
        return GetCachedInterface(typeName).Type;
    }

    private Dictionary<int, CachedType?> SetDict()
    {
        var dict = new Dictionary<int, CachedType?>();
        Type[] interfaces = _cachedType.Type!.GetInterfaces();

        // If the array is already populated, build the dictionary from the array
        if (_cachedArray.IsValueCreated)
        {
            foreach (CachedType cachedType in _cachedArray.Value)
            {
                int key = cachedType.Type!.FullName!.GetHashCode();
                dict[key] = cachedType;
            }
        }
        else
        {
            // If the array is not populated, build the dictionary directly
            foreach (Type type in interfaces)
            {
                var cachedType = new CachedType(type);
                int key = cachedType.Type!.FullName!.GetHashCode();
                dict[key] = cachedType;
            }
        }

        return dict;
    }

    private CachedType[] SetArray()
    {
        // If the dictionary is already populated, return its values as an array
        if (_cachedDict.IsValueCreated)
        {
            var resultArray = new CachedType[_cachedDict.Value.Count];
            var i = 0;
            foreach (CachedType? entry in _cachedDict.Value.Values)
            {
                resultArray[i++] = entry!;
            }
            return resultArray;
        }

        // If the dictionary is not populated, build the array directly
        Type[] interfaces = _cachedType.Type!.GetInterfaces();
        var result = new CachedType[interfaces.Length];

        for (var i = 0; i < interfaces.Length; i++)
        {
            CachedType cachedType = GetCachedInterface(interfaces[i].FullName!);
            result[i] = cachedType;
        }

        return result;
    }

    public CachedType[] GetCachedInterfaces()
    {
        return _cachedArray.Value;
    }

    public Type[] GetInterfaces()
    {
        CachedType[] cachedInterfaces = GetCachedInterfaces();
        var result = new Type[cachedInterfaces.Length];

        for (var i = 0; i < cachedInterfaces.Length; i++)
        {
            result[i] = cachedInterfaces[i].Type!;
        }

        return result;
    }
}