using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Methods;

[Outliers(OutlierMode.DontRemove)]
public class GetMethodBenchmarks
{
    private CachedType _cachedTyped = null!;
    private Type _type = null!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();
        _type = typeof(TestType);
        _cachedTyped = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public MethodInfo? GetMethod_NoCache()
    {
        return _type.GetMethod("PublicMethod1");
    }

    [Benchmark]
    public MethodInfo? GetMethod_Cache()
    {
        return _cachedTyped.GetMethod("PublicMethod1");
    }
}
