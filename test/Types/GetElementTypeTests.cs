using System;
using FluentAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit.Abstractions;
using Xunit;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class GetElementTypeTests
{
    private readonly ReflectionCache _cache;

    public GetElementTypeTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetElementType_should_not_be_null()
    {
        CachedType result = _cache.GetCachedType(typeof(int[]));
        Type? type = result.GetElementType();
        type.Should().NotBeNull();
    }
}
