using System;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Constructors;

public class ConstructorInvokeTests
{
    [Test]
    public void Activator_Create_ShouldCreateInstance()
    {
        Type type = typeof(TestType);
        object? result = Activator.CreateInstance(type);
        result.Should().NotBeNull().And.BeOfType<TestType>();
    }

    [Test]
    public void CreateInstance_ShouldCreateInstance()
    {
        var cache = new ReflectionCache();
        Type type = typeof(TestType);
        CachedType cachedType = cache.GetCachedType(type);
        object? result = cachedType.CreateInstance();
        result.Should().NotBeNull().And.BeOfType<TestType>();
    }

    [Test]
    public void Activator_Create_with_parameters_ShouldCreateInstanceWithParameters()
    {
        Type type = typeof(TestType);
        object? result = Activator.CreateInstance(type, 0, "", "", 1.0);
        result.Should().NotBeNull().And.BeOfType<TestType>();
    }

    [Test]
    public void CreateInstance_with_parameters_ShouldCreateInstanceWithParameters()
    {
        var cache = new ReflectionCache();
        Type type = typeof(TestType);
        CachedType cachedType = cache.GetCachedType(type);
        object? result = cachedType.CreateInstance(0, "", "", 1.0);
        result.Should().NotBeNull().And.BeOfType<TestType>();
    }
}