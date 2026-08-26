using AwesomeAssertions;
using Soenneker.Reflection.Cache.Types;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Soenneker.Reflection.Cache.Tests.Types;

public class CachedTypeTests
{
    private readonly ReflectionCache _cache;

    public CachedTypeTests()
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void MakeCachedGenericType_should_return_GenericType()
    {
        Type genericTypeDefinition = typeof(List<>);
        Type[] typeArguments = [typeof(int)];

        CachedType cachedType = _cache.GetCachedType(genericTypeDefinition);

        Type? genericTypeInstance = cachedType.MakeGenericType(typeArguments);

        genericTypeInstance.Should().NotBeNull();
        genericTypeInstance.Should().Be(typeof(List<int>));
    }

    [Test]
    public void MakeCachedGenericType_with_CachedType_should_return_GenericType()
    {
        Type genericTypeDefinition = typeof(List<>);
        CachedType cachedType = _cache.GetCachedType(genericTypeDefinition);

        CachedType cachedTypeArgument = _cache.GetCachedType(typeof(int));

        CachedType? cachedGenericType = cachedType.MakeCachedGenericType(cachedTypeArgument);

        cachedGenericType.Should().NotBeNull();
        cachedGenericType.Should().Be(_cache.GetCachedType(typeof(List<int>)));
    }

    [Test]
    public void Thread_safe_initialization_should_publish_single_member_graphs()
    {
        CachedType cachedType = _cache.GetCachedType(typeof(Objects.TestType));
        var properties = new object?[128];
        var methods = new object?[128];
        var fields = new object?[128];
        var constructors = new object?[128];

        Parallel.For(0, properties.Length, i =>
        {
            properties[i] = cachedType.GetCachedProperties();
            methods[i] = cachedType.GetCachedMethods();
            fields[i] = cachedType.GetCachedFields();
            constructors[i] = cachedType.GetCachedConstructors();
        });

        for (var i = 1; i < properties.Length; i++)
        {
            ReferenceEquals(properties[0], properties[i]).Should().BeTrue();
            ReferenceEquals(methods[0], methods[i]).Should().BeTrue();
            ReferenceEquals(fields[0], fields[i]).Should().BeTrue();
            ReferenceEquals(constructors[0], constructors[i]).Should().BeTrue();
        }
    }
}
