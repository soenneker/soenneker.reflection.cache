using FluentAssertions;
using System.Reflection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Xunit;
using Xunit.Abstractions;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Properties;

public class GetPropertiesTests
{
    private readonly ReflectionCache _cache;

    public GetPropertiesTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetProperty_should_return_propertyInfo()
    {
        ICachedType result = _cache.GetCachedType(typeof(TestType));
        PropertyInfo? propertyInfo = result.GetProperty("PublicProperty1");
        propertyInfo.Should().NotBeNull();
    }

    [Fact]
    public void GetProperties_should_return_propertyInfos()
    {
        ICachedType result = _cache.GetCachedType(typeof(string));
        PropertyInfo[]? propertyInfos = result.GetProperties();
        propertyInfos.Should().NotBeNullOrEmpty();
    }
}