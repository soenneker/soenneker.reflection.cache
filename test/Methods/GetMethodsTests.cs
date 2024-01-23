using FluentAssertions;
using Soenneker.Reflection.Cache.Constants;
using System.Reflection;
using System;
using Xunit;
using Xunit.Abstractions;
using Soenneker.Reflection.Cache.Methods;
using System.Linq;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Methods;

public class GetMethodsTests
{
    private readonly ReflectionCache _cache;

    public GetMethodsTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetMethods_Cache()
    {
        CachedType result = _cache.GetCachedType(TestType.Locator);

        MethodInfo? methodInfo = result.GetMethod("PublicMethod1");
        methodInfo.Should().NotBeNull();
    }

    [Fact]
    public void GetMethods_NoCache()
    {
        var type = Type.GetType(TestType.Locator);

        MethodInfo[] result = type!.GetMethods(ReflectionCacheConstants.BindingFlags);
        result.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetMethods_NoCache_should_get_MethodInfo()
    {
        MethodInfo[] methodInfo = Type.GetType(TestType.Locator)!.GetMethods();
        methodInfo.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetMethods_Cache_should_get_MethodInfo()
    {
        CachedType result = _cache.GetCachedType(TestType.Locator);
        result.GetCachedMethods()!.ToList().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetMethods_should_return_methodInfos()
    {
        CachedType result = _cache.GetCachedType(typeof(string));

        CachedMethod[]? methods = result.GetCachedMethods();

        methods.Should().NotBeNullOrEmpty();
        methods.Should().OnlyContain(c => c.MethodInfo != null);
    }
}