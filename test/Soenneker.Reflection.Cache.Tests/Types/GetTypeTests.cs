using System;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Types;


namespace Soenneker.Reflection.Cache.Tests.Types;

public class GetTypeTests
{
    private readonly ReflectionCache _cache;

    public GetTypeTests()
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void GetCachedType_should_return_cached_type()
    {
        CachedType result = _cache.GetCachedType(typeof(string));
        result.Should().NotBeNull();
    }

    [Test]
    public void GetType_NoCache_should_return()
    {
        var type = Type.GetType("System.String, mscorlib");
        type.Should().NotBeNull();
    }

    [Test]
    public void GetCachedType_with_string_should_return_cached_type()
    {
        CachedType result = _cache.GetCachedType("System.String, mscorlib");
        result.Should().NotBeNull();
    }
}
