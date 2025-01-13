using System;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Attributes;

[Outliers(OutlierMode.DontRemove)]
public class GetAttributesBenchmarks
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
    public object[] GetAttributes_NoCache()
    {
        return _type.GetCustomAttributes(true);
    }

    [Benchmark]
    public object[]? GetAttributes_Cache()
    {
        return _cachedType.GetCustomAttributes();
    }
}
