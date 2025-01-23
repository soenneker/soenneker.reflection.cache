using System;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Tests.Objects;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

[Outliers(OutlierMode.DontRemove)]
public class GetTypeBenchmarks
{
    private ReflectionCache _cache = null!;
    private ReflectionCache _threadSafeCache = null!;
    private Type _type = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cache = new ReflectionCache(null, false);
        _threadSafeCache = new ReflectionCache();
        _type = typeof(TestType);
    }

    [Benchmark(Baseline = true)]
    public Type? GetType_string_NoCache()
    {
        return Type.GetType("TestType");
    }

    [Benchmark]
    public Type? GetType_string_Cache()
    {
        return _cache.GetType("TestType");
    }

    [Benchmark]
    public Type? GetType_string_ThreadSafe_Cache()
    {
        return _threadSafeCache.GetType("TestType");
    }
}