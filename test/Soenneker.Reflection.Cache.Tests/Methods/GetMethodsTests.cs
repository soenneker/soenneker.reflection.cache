using AwesomeAssertions;
using Soenneker.Reflection.Cache.Constants;
using System.Reflection;
using System;

using Soenneker.Reflection.Cache.Methods;
using System.Linq;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Methods;

public class GetMethodsTests
{
    private readonly ReflectionCache _cache;

    public GetMethodsTests( output)
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void GetMethods_NoCache()
    {
        var type = Type.GetType(TestType.Locator);

        MethodInfo[] result = type!.GetMethods(ReflectionCacheConstants.BindingFlags);
        result.Length.Should().BeGreaterThan(0);
    }

    [Test]
    public void GetMethods_NoCache_should_get_MethodInfo()
    {
        MethodInfo[] methodInfo = Type.GetType(TestType.Locator)!.GetMethods();
        methodInfo.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void GetMethods_Cache_should_get_MethodInfo()
    {
        CachedType result = _cache.GetCachedType(TestType.Locator);
        result.GetCachedMethods()!.ToList().Should().NotBeNullOrEmpty();
    }

    [Test]
    public void GetMethods_should_return_methodInfos()
    {
        CachedType result = _cache.GetCachedType(typeof(string));

        CachedMethod[]? methods = result.GetCachedMethods();

        methods.Should().NotBeNullOrEmpty();
        methods.Should().OnlyContain(c => c.MethodInfo != null);
    }
}