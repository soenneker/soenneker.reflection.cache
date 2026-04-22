using System;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Types;


namespace Soenneker.Reflection.Cache.Tests.Types;

public class GetElementTypeTests
{
    private readonly ReflectionCache _cache;

    public GetElementTypeTests( output)
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void GetElementType_should_not_be_null()
    {
        CachedType result = _cache.GetCachedType(typeof(int[]));
        Type? type = result.GetElementType();
        type.Should().NotBeNull();
    }
}
