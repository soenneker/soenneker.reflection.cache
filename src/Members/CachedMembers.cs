using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Members.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Members;

///<inheritdoc cref="ICachedMembers"/>
public class CachedMembers : ICachedMembers
{
    private readonly Lazy<Dictionary<int, MemberInfo?>> _cachedDict;
    private readonly Lazy<MemberInfo[]> _cachedArray;

    private readonly CachedType _cachedType;

    public CachedMembers(CachedType cachedType)
    {
        _cachedType = cachedType;
        _cachedDict = new Lazy<Dictionary<int, MemberInfo?>>(SetCachedDict, true);
        _cachedArray = new Lazy<MemberInfo[]>(SetArray, true);
    }

    public MemberInfo? GetMember(string name)
    {
        _cachedDict.Value.TryGetValue(name.GetHashCode(), out MemberInfo? result);

        return result;
    }

    private Dictionary<int, MemberInfo?> SetCachedDict()
    {
        var dict = new Dictionary<int, MemberInfo?>();

        // If the array is already populated, build the dictionary from the array
        if (_cachedArray.IsValueCreated)
        {
            foreach (MemberInfo member in _cachedArray.Value)
            {
                dict[member.GetHashCode()] = member;
            }
        }
        else
        {
            // If the array is not populated, build the dictionary directly
            MemberInfo[] members = _cachedType.Type!.GetMembers(ReflectionCacheConstants.BindingFlags);
            foreach (MemberInfo member in members)
            {
                dict[member.GetHashCode()] = member;
            }
        }

        return dict;
    }

    private MemberInfo[] SetArray()
    {
        // If the dictionary is already populated, return its values as an array
        if (_cachedDict.IsValueCreated)
        {
            var result = new MemberInfo[_cachedDict.Value.Count];
            var i = 0;
            foreach (MemberInfo? entry in _cachedDict.Value.Values)
            {
                result[i++] = entry;
            }
            return result;
        }

        // If the dictionary is not populated, build the array directly
        return _cachedType.Type!.GetMembers(ReflectionCacheConstants.BindingFlags);
    }

    public MemberInfo[] GetMembers()
    {
        return _cachedArray.Value;
    }
}