using FluentAssertions;
using System.Reflection;
using Xunit;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Methods;

public class GetMethodTests
{
    private readonly ReflectionCache _cache = new();

    [Fact]
    public void GetMethod_Cache_should_return_methodInfo()
    {
        ICachedType result = _cache.GetCachedType(TestType.Locator);

        MethodInfo? methodInfo = result.GetMethod("PublicMethod1");
        methodInfo.Should().NotBeNull();
    }
}