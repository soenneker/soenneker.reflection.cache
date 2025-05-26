using AwesomeAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using System;
using Soenneker.Reflection.Cache.Types;
using Xunit;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class IsAssignableFromTests
{
    private readonly CachedType _cachedType;

    public IsAssignableFromTests()
    {
        var cache = new ReflectionCache();
        _cachedType = cache.GetCachedType(typeof(BaseType));
    }

    [Fact]
    public void IsAssignableFrom_should_be_true()
    {
        Type derivedType = typeof(DerivedType);

        bool isAssignable = _cachedType.IsAssignableFrom(derivedType);

        isAssignable.Should().BeTrue();

        isAssignable = _cachedType.IsAssignableFrom(derivedType);

        isAssignable.Should().BeTrue();
    }
}