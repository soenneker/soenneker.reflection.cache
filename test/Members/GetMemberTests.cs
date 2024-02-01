using FluentAssertions;
using System.Reflection;
using Xunit;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Members;

public class GetMemberTests
{
    private readonly ReflectionCache _cache = new();

    [Fact]
    public void GetMember_Cache_should_return_memberInfo()
    {
        ICachedType result = _cache.GetCachedType(TestType.Locator);

        MemberInfo? memberInfo = result.GetMember("PublicMember1");
        memberInfo.Should().NotBeNull();
    }
}
