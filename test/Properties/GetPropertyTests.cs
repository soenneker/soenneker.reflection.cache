using System.Reflection;

using FluentAssertions;

using Soenneker.Reflection.Cache.Constants;
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
        ReflectionCacheConstants.BindingFlagsProperties = ReflectionCacheConstants.BindingFlags;
        
        CachedType result = _cache.GetCachedType(typeof(TestType));
        PropertyInfo? propertyInfo = result.GetProperty("PublicProperty1");
        propertyInfo.Should().NotBeNull();
    }

    [Fact]
    public void GetCachedProperty_should_return_CachedProperty()
    {
        ReflectionCacheConstants.BindingFlagsProperties = ReflectionCacheConstants.BindingFlags;

        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedProperty? cachedProperty = result.GetCachedProperty("PublicProperty1");
        cachedProperty.Should().NotBeNull();
    }

    [Fact]
    public void GetCachedProperty_should_return_internal_CachedProperty()
    {
        ReflectionCacheConstants.BindingFlagsProperties = ReflectionCacheConstants.BindingFlags;

        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedProperty? cachedProperty = result.GetCachedProperty("ProtectedInternalProperty");
        cachedProperty.Should().NotBeNull();
    }

    [Fact]
    public void GetCachedProperty_should_return_private_CachedProperty()
    {
        ReflectionCacheConstants.BindingFlagsProperties = ReflectionCacheConstants.BindingFlags;

        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedProperty? cachedProperty = result.GetCachedProperty("PrivateProperty");
        cachedProperty.Should().NotBeNull();
    }

    [Fact]
    public void GetCachedProperty_should_not_return_internal_CachedProperty_when_BindingFlagsNonPublic_is_not_used()
    {
        ReflectionCacheConstants.BindingFlagsProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedProperty? cachedProperty = result.GetCachedProperty("ProtectedInternalProperty");
        cachedProperty.Should().BeNull();
    }

    [Fact]
    public void GetCachedProperty_should_not_return_private_CachedProperty_when_BindingFlagsNonPublic_is_not_used()
    {
        ReflectionCacheConstants.BindingFlagsProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedProperty? cachedProperty = result.GetCachedProperty("PrivateProperty");
        cachedProperty.Should().BeNull();
    }
}