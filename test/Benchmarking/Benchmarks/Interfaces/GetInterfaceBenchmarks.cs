using System;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Tests.Objects.Abstract;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Interfaces;

public class GetInterfaceBenchmarks
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
    public Type? GetInterface_NoCache()
    {
        return _type.GetInterface(nameof(ITestType));
    }

    [Benchmark]
    public Type? GetInterface_Cache()
    {
        return _cachedType.GetInterface(nameof(ITestType));
    }
}
