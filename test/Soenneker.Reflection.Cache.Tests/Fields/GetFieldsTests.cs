using FluentAssertions;
using System.Reflection;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Fields;

public class GetFieldsTests
{
    private readonly ReflectionCache _cache;

    public GetFieldsTests(ITestOutputHelper output)
    {
        _cache = new ReflectionCache();
    }

    [Fact]
    public void GetFields_should_return_fieldInfos()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        FieldInfo[]? fieldInfos = result.GetFields();
        fieldInfos.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetCachedFields_should_return_CachedFields()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedField[]? cachedFields = result.GetCachedFields();
        cachedFields.Should().NotBeNullOrEmpty();
    }
}
