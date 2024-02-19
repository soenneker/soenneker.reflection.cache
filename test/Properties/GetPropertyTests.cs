using System.Reflection;
using FluentAssertions;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Reflection.Cache.Tests.Properties;

public class GetPropertyTests
{
    private readonly ReflectionCache _cache;

    public GetPropertyTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetProperty_should_return_propertyInfo()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        PropertyInfo? propertyInfo = result.GetProperty("PublicProperty1");
        propertyInfo.Should().NotBeNull();
    }

    [Fact]
    public void GetCachedProperty_should_return_CachedProperty()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedProperty? cachedProperty = result.GetCachedProperty("PublicProperty1");
        cachedProperty.Should().NotBeNull();
    }
}