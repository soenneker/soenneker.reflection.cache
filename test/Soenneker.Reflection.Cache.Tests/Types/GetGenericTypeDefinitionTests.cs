using System;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;


namespace Soenneker.Reflection.Cache.Tests.Types;

public class GetGenericTypeDefinitionTests
{
    private readonly ReflectionCache _cache;

    public GetGenericTypeDefinitionTests( output)
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void GetGenericTypeDefinition_should_not_be_null()
    {
        CachedType result = _cache.GetCachedType(typeof(GenericType<int>));
        Type? type = result.GetGenericTypeDefinition();
        type.Should().NotBeNull();
    }
}