using System;
using BenchmarkDotNet.Attributes;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

public class GetTypeBenchmarks
{
    private ReflectionCache _cache = default!;

    [GlobalSetup]
    public void Setup()
    {
        _cache = new ReflectionCache();
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
}