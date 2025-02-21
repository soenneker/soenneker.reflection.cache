using FluentAssertions;
using Soenneker.Reflection.Cache.Types;
using Xunit;
using System.Collections.Generic;
using System;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class CachedTypeTests
{
    private readonly ReflectionCache _cache;

    public CachedTypeTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
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
}