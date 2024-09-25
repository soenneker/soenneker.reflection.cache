using FluentAssertions;
using System.Reflection;
using Soenneker.Reflection.Cache.Methods;
using Xunit;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Methods;

public class GetMethodTests
{
    private readonly ReflectionCache _cache = new();

    [Fact]
    public void GetMethod_Cache_should_return_methodInfo()
    {
        CachedType result = _cache.GetCachedType(TestType.Locator);

        MethodInfo? methodInfo = result.GetMethod("PublicMethod1");
        methodInfo.Should().NotBeNull();
    }

    [Fact]
    public void MakeCachedGenericMethod_should_result()
    {
        CachedType result = _cache.GetCachedType(typeof(ClassWithGenericMethod));
        CachedMethod? method = result.GetCachedMethod("GenericMethod");
        CachedMethod? genericMethod = method!.MakeCachedGenericMethod(_cache.GetCachedType(typeof(int)));
        genericMethod.Should().NotBeNull();
    }
}