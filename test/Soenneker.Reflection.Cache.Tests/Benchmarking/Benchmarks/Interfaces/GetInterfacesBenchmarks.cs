using System;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Interfaces;

[Outliers(OutlierMode.DontRemove)]
public class GetInterfacesBenchmarks
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
    public Type[] GetInterfaces_NoCache()
    {
        return _type.GetInterfaces();
    }

    [Benchmark]
    public Type?[]? GetInterfaces_Cache()
    {
        return _cachedType.GetInterfaces();
    }
}