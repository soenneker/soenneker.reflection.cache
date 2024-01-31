using System.Reflection;
using Soenneker.Reflection.Cache.Members;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class CachedMembersExtension
{
    public static MemberInfo?[] ToMemberInfos(this CachedMember[] cachedMembers)
    {
        int count = cachedMembers.Length;

        var memberInfos = new MemberInfo?[count];

        for (var i = 0; i < count; i++)
        {
            memberInfos[i] = cachedMembers[i].MemberInfo;
        }

        return memberInfos;
    }
}