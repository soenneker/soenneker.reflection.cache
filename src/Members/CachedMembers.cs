using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Members.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Members;

///<inheritdoc cref="ICachedMembers"/>
public class CachedMembers : ICachedMembers
{
    private readonly Lazy<Dictionary<int, CachedMember>> _cachedDict;
    private readonly Lazy<CachedMember[]> _cachedArray;

    private readonly CachedType _cachedType;

    private readonly Lazy<MemberInfo?[]> _cachedMemberInfos;

    private readonly CachedTypes _cachedTypes;

    public CachedMembers(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedType = cachedType;
        _cachedDict = new Lazy<Dictionary<int, CachedMember>>(() => SetDict(threadSafe), threadSafe);
        _cachedArray = new Lazy<CachedMember[]>(() => SetArray(threadSafe), threadSafe);

        _cachedMemberInfos = new Lazy<MemberInfo?[]>(_cachedArray.Value.ToMemberInfos, threadSafe);
    }

    public CachedMember? GetCachedMember(string name)
    {
        _cachedDict.Value.TryGetValue(name.GetHashCode(), out CachedMember? result);

        return result;
    }

    public MemberInfo? GetMember(string name)
    {
        return GetCachedMember(name)?.MemberInfo;
    }

    private CachedMember[] SetArray(bool threadSafe)
    {
        if (_cachedDict.IsValueCreated && _cachedDict.Value.Count > 0)
        {
            Dictionary<int, CachedMember>.ValueCollection cachedDictValues = _cachedDict.Value.Values;
            var result = new CachedMember[cachedDictValues.Count];
            var i = 0;

            foreach (CachedMember member in cachedDictValues)
            {
                result[i++] = member;
            }

            return result;
        }

        MethodInfo[] memberInfos = _cachedType.Type!.GetMethods(ReflectionCacheConstants.BindingFlags);
        int count = memberInfos.Length;

        var cachedArray = new CachedMember[count];

        for (var i = 0; i < count; i++)
        {
            cachedArray[i] = new CachedMember(memberInfos[i], _cachedTypes, threadSafe);
        }

        return cachedArray;
    }

    private Dictionary<int, CachedMember> SetDict(bool threadSafe)
    {
        Dictionary<int, CachedMember> cachedDict;
        int count;

        // Don't recreate these objects if the dict is already created
        if (_cachedArray.IsValueCreated)
        {
            CachedMember[] cachedMembers = _cachedArray.Value;

            count = cachedMembers.Length;

            cachedDict = new Dictionary<int, CachedMember>(count);

            for (var i = 0; i < count; i++)
            {
                CachedMember cachedMember = cachedMembers[i];
                int key = cachedMember.CacheKey;

                cachedDict.Add(key, cachedMember);
            }

            return cachedDict;
        }

        MethodInfo[] memberInfos = _cachedType.Type!.GetMethods(ReflectionCacheConstants.BindingFlags);

        count = memberInfos.Length;

        cachedDict = new Dictionary<int, CachedMember>(count);

        for (var i = 0; i < count; i++)
        {
            MethodInfo memberInfo = memberInfos[i];

            var cachedMember = new CachedMember(memberInfo, _cachedTypes, threadSafe);
            int key = cachedMember.CacheKey;

            cachedDict.Add(key, cachedMember);
        }

        return cachedDict;
    }

    public CachedMember[] GetCachedMembers()
    {
        return _cachedArray.Value;
    }

    public MemberInfo[] GetMembers()
    {
        return _cachedMemberInfos.Value;
    }
}