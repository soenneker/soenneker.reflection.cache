using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Members.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Members;

///<inheritdoc cref="ICachedMembers"/>
public class CachedMembers : ICachedMembers
{
    private readonly CachedMember[] _cachedArray;
    private readonly MemberInfo[] _memberInfos;

    public CachedMembers(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _memberInfos = cachedType.Type!.GetMembers(cachedTypes.Options.MemberFlags);
        int length = _memberInfos.Length;

        _cachedArray = new CachedMember[length];

        for (var i = 0; i < length; i++)
        {
            _cachedArray[i] = new CachedMember(_memberInfos[i], cachedTypes, threadSafe);
        }
    }

    public CachedMember[] GetCachedMembers()
    {
        return _cachedArray;
    }

    public MemberInfo[] GetMembers()
    {
        return _memberInfos;
    }
}
