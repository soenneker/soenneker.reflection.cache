using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

[Outliers(OutlierMode.DontRemove)]
public class GetConstructorBenchmarks
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
    public ConstructorInfo? GetConstructor_NoCache()
    {
        return _type.GetConstructor(ReflectionCacheConstants.BindingFlags, Type.EmptyTypes);
    }

    [Benchmark]
    public ConstructorInfo? GetConstructor_NoCache_Parameters()
    {
        return _type.GetConstructor(ReflectionCacheConstants.BindingFlags, [typeof(int), typeof(int), typeof(string), typeof(double)]);
    }

    [Benchmark]
    public ConstructorInfo? GetConstructor_Cache()
    {
        return _cachedType.GetConstructor();
    }

    [Benchmark]
    public ConstructorInfo? GetConstructor_Cache_Parameters()
    {
        return _cachedType.GetConstructor([typeof(int), typeof(int), typeof(string), typeof(double)]);
    }
}