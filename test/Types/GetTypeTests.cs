using System;
using FluentAssertions;
using Xunit.Abstractions;
using Xunit;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class GetTypeTests
{
    private readonly ReflectionCache _cache;

    public GetTypeTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetCachedType_should_return_cached_type()
    {
        ICachedType result = _cache.GetCachedType(typeof(string));
        result.Should().NotBeNull();
    }

    [Fact]
    public void GetType_NoCache_should_return()
    {
        var type = Type.GetType("System.String, mscorlib");
        type.Should().NotBeNull();
    }

    [Fact]
    public void GetCachedType_with_string_should_return_cached_type()
    {
        ICachedType result = _cache.GetCachedType("System.String, mscorlib");
        result.Should().NotBeNull();
    }
}