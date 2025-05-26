using System;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

using Xunit;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class GetGenericTypeDefinitionTests
{
    private readonly ReflectionCache _cache;

    public GetGenericTypeDefinitionTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetGenericTypeDefinition_should_not_be_null()
    {
        CachedType result = _cache.GetCachedType(typeof(GenericType<int>));
        Type? type = result.GetGenericTypeDefinition();
        type.Should().NotBeNull();
    }
}