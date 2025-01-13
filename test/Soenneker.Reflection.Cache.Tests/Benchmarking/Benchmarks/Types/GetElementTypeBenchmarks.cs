using System;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

[Outliers(OutlierMode.DontRemove)]
public class GetElementTypeBenchmarks
{
    private CachedType _cachedType = default!;
    private Type _type = default!;

    [GlobalSetup]
    public void Setup()
    {
        _type = typeof(int[]);
        var cache = new ReflectionCache();
        _cachedType = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public Type GetElementType_NoCache()
    {
        return _type.GetElementType();
    }

    [Benchmark]
    public Type? GetElementType_Cache()
    {
        return _cachedType.GetElementType();
    }
}
