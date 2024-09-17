﻿using System;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Parameters;

public class GetParametersBenchmarks
{
    private CachedType _cachedType = default!;

    private Type _type = default!;
    private CachedConstructor _cachedConstructor = default!;

    private ConstructorInfo _constructorInfo = default!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();

        _type = typeof(TestType);

        _cachedType = cache.GetCachedType(_type);

        _cachedConstructor = _cachedType.GetCachedConstructors()!.First();
        _constructorInfo = _type.GetConstructors().First();
    }

    [Benchmark(Baseline = true)]
    public ParameterInfo[] GetParameters_NoCache()
    {
        return _constructorInfo.GetParameters();
    }

    [Benchmark]
    public ParameterInfo[] GetParameters_Cache()
    {
        return _cachedConstructor.GetParameters();
    }
}