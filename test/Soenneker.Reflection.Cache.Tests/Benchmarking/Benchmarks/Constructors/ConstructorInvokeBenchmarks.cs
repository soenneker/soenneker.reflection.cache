﻿using System;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

public class ConstructorInvokeBenchmarks
{
    private CachedType _cachedType = default!;
    private Type _type = default!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();
        _type = typeof(TestType);
        _cachedType = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public object? Activator_CreateInstance()
    {
        return Activator.CreateInstance(_type);
    }

    [Benchmark]
    public object? Cache_CreateInstance()
    {
        return _cachedType.CreateInstance();
    }
}