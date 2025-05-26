using System;
using System.Reflection;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Constructors;

public class GetConstructorsTests
{
    private readonly ReflectionCache _cache;

    public GetConstructorsTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetConstructors_Cache()
    {
        CachedType type = _cache.GetCachedType(typeof(TestType));

        ConstructorInfo?[]? result = type.GetConstructors();
        result.Should().NotBeNull();
    }

    [Fact]
    public void GetConstructors_NoCache()
    {
        Type type = typeof(TestType);

        ConstructorInfo[] result = type.GetConstructors(ReflectionCacheConstants.BindingFlags);
        result.Should().NotBeNull();
    }
}