using System;
using FluentAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Xunit.Abstractions;
using Xunit;
using Soenneker.Reflection.Cache.Types.Abstract;

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
        ICachedType result = _cache.GetCachedType(typeof(GenericType<int>));
        Type? type = result.GetGenericTypeDefinition();
        type.Should().NotBeNull();
    }
}