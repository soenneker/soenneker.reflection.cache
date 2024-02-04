using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Members;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class CachedMembersExtension
{
    public static MemberInfo?[] ToMemberInfos(this CachedMember[] cachedMembers)
    {
        ReadOnlySpan<CachedMember> span = cachedMembers;

        var memberInfos = new MemberInfo?[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            memberInfos[i] = span[i].MemberInfo;
        }

        return memberInfos;
    }
}