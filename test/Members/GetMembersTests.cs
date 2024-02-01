using FluentAssertions;
using Soenneker.Reflection.Cache.Constants;
using System.Reflection;
using System;
using Xunit;
using Xunit.Abstractions;
using Soenneker.Reflection.Cache.Members;
using System.Linq;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Members;

public class GetMembersTests
{
    private readonly ReflectionCache _cache;

    public GetMembersTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetMembers_NoCache()
    {
        var type = Type.GetType(TestType.Locator);

        MemberInfo[] result = type!.GetMembers(ReflectionCacheConstants.BindingFlags);
        result.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetMembers_NoCache_should_get_MemberInfo()
    {
        MemberInfo[] memberInfo = Type.GetType(TestType.Locator)!.GetMembers();
        memberInfo.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetMembers_Cache_should_get_MemberInfo()
    {
        CachedType result = _cache.GetCachedType(TestType.Locator);
        result.GetCachedMembers()!.ToList().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetMembers_should_return_memberInfos()
    {
        CachedType result = _cache.GetCachedType(typeof(string));

        CachedMember[]? members = result.GetCachedMembers();

        members.Should().NotBeNullOrEmpty();
        members.Should().OnlyContain(c => c.MemberInfo != null);
    }
}
