using AwesomeAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using Soenneker.Reflection.Cache.Tests.Enums;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class CachedTypePropertiesTests
{
    private readonly ReflectionCache _cache;

    public CachedTypePropertiesTests()
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void IsDictionary_DoubleDerived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DoubleDerivedDictionary));
        result.IsDictionary.Should().BeTrue();
    }

    [Test]
    public void IsDictionary_Derived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DerivedDictionary));
        result.IsDictionary.Should().BeTrue();
    }

    [Test]
    public void IsDictionary_direct_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(Dictionary<string, object>));
        result.IsDictionary.Should().BeTrue();
    }

    [Test]
    public void IsDictionary_should_be_true_for_generic_IDictionary()
    {
        CachedType result = _cache.GetCachedType(typeof(IDictionary<string, string>));
        result.IsDictionary.Should().BeTrue();
    }

    [Test]
    public void IsDictionary_should_be_true_for_IDictionary()
    {
        CachedType result = _cache.GetCachedType(typeof(IDictionary));
        result.IsDictionary.Should().BeTrue();
    }

    [Test]
    public void IsDictionary_list_should_be_false()
    {
        CachedType result = _cache.GetCachedType(typeof(List<string>));
        result.IsDictionary.Should().BeFalse();
    }

    [Test]
    public void IsReadOnlyDictionary_Derived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DerivedReadOnlyDictionary));
        result.IsReadOnlyDictionary.Should().BeTrue();
    }

    [Test]
    public void IsReadOnlyDictionary_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(ReadOnlyDictionary<string, string>));
        result.IsReadOnlyDictionary.Should().BeTrue();
    }

    [Test]
    public void IsCollection_Derived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DerivedCollection));
        result.IsCollection.Should().BeTrue();
    }

    [Test]
    public void IsCollection_direct_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(ICollection<string>));
        result.IsCollection.Should().BeTrue();
    }

    [Test]
    public void IsEnumerable_derived_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DerivedEnumerable));
        result.IsEnumerable.Should().BeTrue();
    }

    [Test]
    public void IsRecord_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(TestRecord));
        result.IsRecord.Should().BeTrue();
    }

    [Test]
    public void IsClass_should_be_false()
    {
        CachedType result = _cache.GetCachedType(typeof(TestRecord));
        result.IsClass.Should().BeTrue();
    }

    [Test]
    public void IsEnumerable_direct_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(IEnumerable<string>));
        result.IsEnumerable.Should().BeTrue();
    }

    [Test]
    public void IsWeakReferenceT_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(WeakReference<string>));
        result.IsWeakReference.Should().BeTrue();
    }

    [Test]
    public void IsWeakReference_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(WeakReference));
        result.IsWeakReference.Should().BeTrue();
    }

    [Test]
    public void IsFunc_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(Func<int>));
        result.IsFunc.Should().BeTrue();
    }

    [Test]
    public void IsEnumValue_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DayOfWeekTypeEnumValue));
        result.IsEnumValue.Should().BeTrue();
    }

    [Test]
    public void IsIntellenum_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DayOfWeekTypeIntellenum));
        result.IsIntellenum.Should().BeTrue();
    }

    [Test]
    public void IsSmartEnum_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(DayOfWeekTypeSmartEnum));
        result.IsSmartEnum.Should().BeTrue();
    }
}
