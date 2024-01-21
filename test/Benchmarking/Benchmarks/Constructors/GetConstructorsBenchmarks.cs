using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

public class GetConstructorsBenchmarks
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
    public ConstructorInfo[] GetConstructors_NoCache()
    {
        return _type.GetConstructors(ReflectionCacheConstants.BindingFlags);
    }

    [Benchmark]
    public ConstructorInfo?[]? GetConstructors_Cache()
    {
        return _cachedType.GetConstructors();
    }
}