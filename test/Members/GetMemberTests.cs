using System;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Soenneker.Reflection.Cache.Tests.Objects;

namespace Soenneker.Reflection.Cache.Tests.Members;

public class GetMemberTests
{
    private readonly ReflectionCache _cache = new();

    [Fact]
    public void GetMember_NoCache_should_return_memberInfo()
    {
        Type? type = typeof(TestType);

        MemberInfo[]? memberInfo = type.GetMember("PublicProperty1");
        memberInfo.Should().NotBeNull();
    }

    //[Fact]
    //public void GetMember_Cache_should_return_memberInfo()
    //{
    //    ICachedType result = _cache.GetCachedType(TestType.Locator);

    //    MemberInfo? memberInfo = result.GetMember("PublicProperty1");
    //    memberInfo.Should().NotBeNull();
    //}
}
