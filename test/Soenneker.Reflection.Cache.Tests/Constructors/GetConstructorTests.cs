using Soenneker.Reflection.Cache.Constants;
using System;
using System.Reflection;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Constructors;

public class GetConstructorTests
{
    private readonly ReflectionCache _cache;

    public GetConstructorTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetConstructor_NoCache()
    {
        Type type = typeof(TestType);

        ConstructorInfo? result = type.GetConstructor(ReflectionCacheConstants.BindingFlags, Type.EmptyTypes);
        result.Should().NotBeNull();
    }

    [Fact]
    public void GetConstructor_Cache()
    {
        CachedType type = _cache.GetCachedType(typeof(TestType));


        ConstructorInfo? result = type.GetConstructor();
        result.Should().NotBeNull();
    }
}