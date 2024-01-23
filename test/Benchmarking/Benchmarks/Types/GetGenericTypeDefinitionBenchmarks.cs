using System;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

public class GetGenericTypeDefinitionBenchmarks
{
    private CachedType _cachedType = default!;
    private Type _type = default!;

    [GlobalSetup]
    public void Setup()
    {
        _type = typeof(GenericType<TestType>);
        var cache = new ReflectionCache();
        _cachedType = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public Type GetGenericTypeDefinition_NoCache()
    {
        return _type.GetGenericTypeDefinition();
    }

    [Benchmark]
    public Type? GetGenericTypeDefinition_Cache()
    {
        return _cachedType.GetGenericTypeDefinition();
    }
}