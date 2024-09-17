using FluentAssertions;
using Soenneker.Reflection.Cache.Types.Abstract;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Reflection.Cache.Tests;

public class ReflectionCacheTests
{
    private readonly ReflectionCache _cache;

    public ReflectionCacheTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void Cache_CreateInstance()
    {
        ICachedType result = _cache.GetCachedType("System.String, mscorlib");

        var str = result.CreateInstance<string>(new[] {'t'});
        str.Should().NotBeNull();
    }
}