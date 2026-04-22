using AwesomeAssertions;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests;

public sealed class ReflectionCacheTests
{
    private readonly ReflectionCache _cache;

    public ReflectionCacheTests( output)
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void Cache_CreateInstance()
    {
        ICachedType result = _cache.GetCachedType("System.String, mscorlib");

        var str = result.CreateInstance<string>(new[] {'t'});
        str.Should().NotBeNull();
    }
}