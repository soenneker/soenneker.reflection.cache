using FluentAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit.Abstractions;
using Xunit;
using System.Collections.Generic;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class CacheTypeTests
{
    private readonly ReflectionCache _cache;

    public CacheTypeTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void IsDictionary_DoubleDerived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DoubleDerivedDictionary));
        result.IsDictionary.Should().BeTrue();
    }

    [Fact]
    public void IsDictionary_Derived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DerivedDictionary));
        result.IsDictionary.Should().BeTrue();
    }

    [Fact]
    public void IsDictionary_direct_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(Dictionary<string, object>));
        result.IsDictionary.Should().BeTrue();
    }

    [Fact]
    public void IsDictionary_list_should_be_false()
    {
        CachedType result = _cache.GetCachedType(typeof(List<string>));
        result.IsDictionary.Should().BeFalse();
    }

    [Fact]
    public void IsReadOnlyDictionary_Derived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DerivedReadOnlyDictionary));
        result.IsReadOnlyDictionary.Should().BeTrue();
    }
}