using Soenneker.Reflection.Cache.Constants;
using System;
using System.Reflection;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;


namespace Soenneker.Reflection.Cache.Tests.Constructors;

public class GetConstructorTests
{
    private readonly ReflectionCache _cache;

    public GetConstructorTests()
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void GetConstructor_NoCache()
    {
        Type type = typeof(TestType);

        ConstructorInfo? result = type.GetConstructor(ReflectionCacheConstants.BindingFlags, Type.EmptyTypes);
        result.Should().NotBeNull();
    }

    [Test]
    public void GetConstructor_Cache()
    {
        CachedType type = _cache.GetCachedType(typeof(TestType));


        ConstructorInfo? result = type.GetConstructor();
        result.Should().NotBeNull();
    }
}
