using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Members.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Members;

///<inheritdoc cref="ICachedMembers"/>
public class CachedMembers : ICachedMembers
{
    private readonly Lazy<CachedMember[]> _cachedArray;

    private readonly CachedType _cachedType;

    private readonly Lazy<MemberInfo?[]> _cachedMemberInfos;

    private readonly CachedTypes _cachedTypes;

    public CachedMembers(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedType = cachedType;
        _cachedArray = new Lazy<CachedMember[]>(() => SetArray(threadSafe), threadSafe);

        _cachedMemberInfos = new Lazy<MemberInfo?[]>(() => _cachedArray.Value.ToMemberInfos(), threadSafe);
    }

    private CachedMember[] SetArray(bool threadSafe)
    {
        MemberInfo[] memberInfos = _cachedType.Type!.GetMembers(_cachedTypes.Options.MemberFlags);
        int count = memberInfos.Length;

        var cachedArray = new CachedMember[count];

        for (var i = 0; i < count; i++)
        {
            cachedArray[i] = new CachedMember(memberInfos[i], _cachedTypes, threadSafe);
        }

        return cachedArray;
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