using FluentAssertions;
using System.Reflection;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Reflection.Cache.Tests.Properties;

public class GetPropertiesTests
{
    private readonly ReflectionCache _cache;

    public GetPropertiesTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetProperties_should_return_propertyInfos()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        PropertyInfo[]? propertyInfos = result.GetProperties();
        propertyInfos.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetCachedProperties_should_return_CachedProperties()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedProperty[]? cachedProperties = result.GetCachedProperties();
        cachedProperties.Should().NotBeNullOrEmpty();
    }
}