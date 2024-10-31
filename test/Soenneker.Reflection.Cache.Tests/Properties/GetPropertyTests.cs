using System.Linq;
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

    [Fact]
    public void GetCachedProperty_should_return_internal_CachedProperty()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedProperty? cachedProperty = result.GetCachedProperty("ProtectedInternalProperty");
        cachedProperty.Should().NotBeNull();
    }

    [Fact]
    public void IsDelegate_ShouldReturnTrue_WhenPropertyTypeIsDelegate()
    {
        CachedType result = _cache.GetCachedType(typeof(ClassWithDelegateProperty));
        CachedProperty? cachedProperty = result.GetCachedProperty("DelegateProperty");
        cachedProperty!.IsDelegate.Should().BeTrue();
    }

    [Fact]
    public void IsDelegate_ShouldReturnFalse_WhenPropertyTypeIsNotDelegate()
    {
        CachedType result = _cache.GetCachedType(typeof(ClassWithDelegateProperty));
        CachedProperty? cachedProperty = result.GetCachedProperty("NonDelegateProperty");
        cachedProperty!.IsDelegate.Should().BeFalse();
    }

    [Fact]
    public void IsEqualityContract_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(TestRecord));

        CachedProperty[]? properties = result.GetCachedProperties();

        properties!.First().IsEqualityContract.Should().BeTrue();
    }
}