using System;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

public class ConstructorInvokeParametersBenchmarks
{
    private ICachedType _cachedType = default!;
    private Type _type = default!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();
        _type = typeof(TestType);
        _cachedType = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public object? Activator_Create_with_parameters()
    {
        return Activator.CreateInstance(_type, 0, "", "", 1.0);
    }

    [Benchmark]
    public object? Cache_CreateInstance_with_parameters()
    {
        return _cachedType.CreateInstance(0, "", "", 1.0);
    }
}