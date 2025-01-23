using System;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

[Outliers(OutlierMode.DontRemove)]
public class ConstructorInvokeParametersBenchmarks
{
    private CachedType _cachedType = null!;
    private Type _type = null!;

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