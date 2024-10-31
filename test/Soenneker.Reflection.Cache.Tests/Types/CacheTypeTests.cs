using FluentAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit.Abstractions;
using Xunit;
using System.Collections.Generic;
using System;
using System.Collections;

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
    public void MakeCachedGenericType_should_return_GenericType()
    {
        Type genericTypeDefinition = typeof(List<>);
        Type[] typeArguments = [typeof(int)];

        CachedType cachedType = _cache.GetCachedType(genericTypeDefinition);

        Type? genericTypeInstance = cachedType.MakeGenericType(typeArguments);

        genericTypeInstance.Should().NotBeNull();
        genericTypeInstance.Should().Be(typeof(List<int>));
    }

    [Fact]
    public void MakeCachedGenericType_with_CachedType_should_return_GenericType()
    {
        Type genericTypeDefinition = typeof(List<>);
        CachedType cachedType = _cache.GetCachedType(genericTypeDefinition);

        CachedType cachedTypeArgument = _cache.GetCachedType(typeof(int));

        CachedType? cachedGenericType = cachedType.MakeCachedGenericType(cachedTypeArgument);

        cachedGenericType.Should().NotBeNull();
        cachedGenericType.Should().Be(_cache.GetCachedType(typeof(List<int>)));
    }

    [Fact]
    public void IsFunc_should_be_true()
    {
        CachedType result = _cache.GetCachedType(typeof(Func<int>));
        result.IsFunc.Should().BeTrue();
    }
}