using AwesomeAssertions;
using System.Reflection;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;


namespace Soenneker.Reflection.Cache.Tests.Fields;

public class GetFieldsTests
{
    private readonly ReflectionCache _cache;

    public GetFieldsTests()
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void GetFields_should_return_fieldInfos()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        FieldInfo[]? fieldInfos = result.GetFields();
        fieldInfos.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void GetCachedFields_should_return_CachedFields()
    {
        CachedType result = _cache.GetCachedType(typeof(TestType));
        CachedField[]? cachedFields = result.GetCachedFields();
        cachedFields.Should().NotBeNullOrEmpty();
    }
}
