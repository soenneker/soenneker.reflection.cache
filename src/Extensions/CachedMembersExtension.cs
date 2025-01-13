using System.Reflection;
using Soenneker.Reflection.Cache.Members;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class CachedMembersExtension
{
    public static MemberInfo?[] ToMemberInfos(this CachedMember[] cachedMembers)
    {
        int length = cachedMembers.Length;
        var memberInfos = new MemberInfo?[length];

        for (var i = 0; i < length; i++)
        {
            memberInfos[i] = cachedMembers[i].MemberInfo;
        }

        return memberInfos;
    }
}