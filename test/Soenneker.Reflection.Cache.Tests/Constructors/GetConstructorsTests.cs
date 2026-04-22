using System;
using System.Reflection;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;


namespace Soenneker.Reflection.Cache.Tests.Constructors;

public class GetConstructorsTests
{
    private readonly ReflectionCache _cache;

    public GetConstructorsTests( output)
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void GetConstructors_Cache()
    {
        CachedType type = _cache.GetCachedType(typeof(TestType));

        ConstructorInfo?[]? result = type.GetConstructors();
        result.Should().NotBeNull();
    }

    [Test]
    public void GetConstructors_NoCache()
    {
        Type type = typeof(TestType);

        ConstructorInfo[] result = type.GetConstructors(ReflectionCacheConstants.BindingFlags);
        result.Should().NotBeNull();
    }
}