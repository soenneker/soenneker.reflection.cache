using System;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Attributes;

public class GetAttributesBenchmarks
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
