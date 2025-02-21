using FluentAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using Soenneker.Reflection.Cache.Tests.Enums;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class CachedTypePropertiesTests
{
    private readonly ReflectionCache _cache;

    public CachedTypePropertiesTests(ITestOutputHelper output)
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
    public void IsDictionary_should_be_true_for_generic_IDictionary()
    {
        CachedType result = _cache.GetCachedType(typeof(IDictionary<string, string>));
        result.IsDictionary.Should().BeTrue();
    }

    [Fact]
    public void IsDictionary_should_be_true_for_IDictionary()
    {
        CachedType result = _cache.GetCachedType(typeof(IDictionary));
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

    [Fact]
    public void IsReadOnlyDictionary_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(ReadOnlyDictionary<string, string>));
        result.IsReadOnlyDictionary.Should().BeTrue();
    }

    [Fact]
    public void IsCollection_Derived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DerivedCollection));
        result.IsCollection.Should().BeTrue();
    }

    [Fact]
    public void IsCollection_direct_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(ICollection<string>));
        result.IsCollection.Should().BeTrue();
    }

    [Fact]
    public void IsEnumerable_derived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DerivedEnumerable));
        result.IsEnumerable.Should().BeTrue();
    }

    [Fact]
    public void IsRecord_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(TestRecord));
        result.IsRecord.Should().BeTrue();
    }

    [Fact]
    public void IsClass_should_be_false()
    {
        CachedType result = _cache.GetCachedType(typeof(TestRecord));
        result.IsClass.Should().BeTrue();
    }

    [Fact]
    public void IsEnumerable_direct_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(IEnumerable<string>));
        result.IsEnumerable.Should().BeTrue();
    }

    [Fact]
    public void IsWeakReferenceT_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(WeakReference<string>));
        result.IsWeakReference.Should().BeTrue();
    }

    [Fact]
    public void IsWeakReference_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(WeakReference));
        result.IsWeakReference.Should().BeTrue();
    }

    [Fact]
    public void IsFunc_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(Func<int>));
        result.IsFunc.Should().BeTrue();
    }

    [Fact]
    public void IsIntellenum_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DayOfWeekTypeIntellenum));
        result.IsIntellenum.Should().BeTrue();
    }

    [Fact]
    public void IsSmartEnum_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DayOfWeekTypeSmartEnum));
        result.IsSmartEnum.Should().BeTrue();
    }
}