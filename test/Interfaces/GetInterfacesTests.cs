﻿using System;
using FluentAssertions;
using Soenneker.Reflection.Cache.Interfaces;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Tests.Objects.Abstract;
using Soenneker.Reflection.Cache.Types;
using Xunit;

namespace Soenneker.Reflection.Cache.Tests.Interfaces;

public class CachedInterfacesTests
{
    [Fact]
    public void GetCachedInterface_ShouldReturnCachedType()
    {
        var cachedType = new CachedType(typeof(TestType));
        var cachedInterfaces = new CachedInterfaces(cachedType);
        CachedType? result = cachedInterfaces.GetCachedInterface(typeof(ITestType).FullName);
        result.Should().NotBeNull();
        result.Type.Should().Be(typeof(ITestType));
    }

    [Fact]
    public void GetInterface_ShouldReturnInterfaceType()
    {
        var cachedType = new CachedType(typeof(TestType));
        var cachedInterfaces = new CachedInterfaces(cachedType);
        Type? result = cachedInterfaces.GetInterface(typeof(ITestType).FullName);
        result.Should().Be(typeof(ITestType));
    }

    [Fact]
    public void GetCachedInterfaces_ShouldReturnCachedTypeArray()
    {
        var cachedType = new CachedType(typeof(TestType));
        var cachedInterfaces = new CachedInterfaces(cachedType);
        CachedType[]? result = cachedInterfaces.GetCachedInterfaces();
        result.Should().NotBeNull();
        result.Should().ContainSingle().Which.Type.Should().Be(typeof(ITestType));
    }

    [Fact]
    public void GetInterfaces_ShouldReturnInterfaceArray()
    {
        var cachedType = new CachedType(typeof(TestType));
        var cachedInterfaces = new CachedInterfaces(cachedType);
        Type[]? result = cachedInterfaces.GetInterfaces();
        result.Should().NotBeNull();
        result.Should().ContainSingle().Which.Should().Be(typeof(ITestType));
    }
}