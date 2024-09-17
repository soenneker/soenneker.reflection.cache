using System.Reflection;
using FluentAssertions;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Reflection.Cache.Tests.Fields;

public class GetFieldTests
{
    private readonly ReflectionCache _cache;

    public GetFieldTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetField_should_return_fieldInfo()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        FieldInfo? fieldInfo = result.GetField("PublicField");
        fieldInfo.Should().NotBeNull();
    }

    [Fact]
    public void GetCachedField_should_return_CachedField()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedField? cachedField = result.GetCachedField("PublicField");
        cachedField.Should().NotBeNull();
    }

    [Fact]
    public void GetCachedField_should_return_internal_CachedField()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedField? cachedField = result.GetCachedField("_internalField");
        cachedField.Should().NotBeNull();
    }
}