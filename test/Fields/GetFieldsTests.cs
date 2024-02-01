using FluentAssertions;
using System.Reflection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Reflection.Cache.Tests.Fields;

public class GetFieldsTests
{
    private readonly ReflectionCache _cache;

    public GetFieldsTests(ITestOutputHelper output)
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
    public void GetFields_should_return_fieldInfos()
    {
        CachedType result = _cache.GetCachedType(typeof(string));
        FieldInfo[]? fieldInfos = result.GetFields();
        fieldInfos.Should().NotBeNullOrEmpty();
    }
}
