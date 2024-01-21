using System;
using FluentAssertions;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types.Abstract;
using Xunit;

namespace Soenneker.Reflection.Cache.Tests.Constructors;

public class ConstructorInvokeTests
{
    [Fact]
    public void Activator_Create_ShouldCreateInstance()
    {
        Type type = typeof(TestType);
        object? result = Activator.CreateInstance(type);
        result.Should().NotBeNull().And.BeOfType<TestType>();
    }

    [Fact]
    public void CreateInstance_ShouldCreateInstance()
    {
        var cache = new ReflectionCache();
        Type type = typeof(TestType);
        ICachedType cachedType = cache.GetCachedType(type);
        object? result = cachedType.CreateInstance();
        result.Should().NotBeNull().And.BeOfType<TestType>();
    }

    [Fact]
    public void Activator_Create_with_parameters_ShouldCreateInstanceWithParameters()
    {
        Type type = typeof(TestType);
        object? result = Activator.CreateInstance(type, 0, "", "", 1.0);
        result.Should().NotBeNull().And.BeOfType<TestType>();
    }

    [Fact]
    public void CreateInstance_with_parameters_ShouldCreateInstanceWithParameters()
    {
        var cache = new ReflectionCache();
        Type type = typeof(TestType);
        ICachedType cachedType = cache.GetCachedType(type);
        object? result = cachedType.CreateInstance(0, "", "", 1.0);
        result.Should().NotBeNull().And.BeOfType<TestType>();
    }
}