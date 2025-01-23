using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Methods;

[Outliers(OutlierMode.DontRemove)]
public class GetMethodsBenchmarks
{
    private CachedType _cachedType = null!;
    private Type _type = null!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();
        _type = typeof(TestType);

        _cachedType = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public MethodInfo[] GetMethods_NoCache()
    {
        return _type.GetMethods(ReflectionCacheConstants.BindingFlags);
    }

    [Benchmark]
    public MethodInfo?[]? GetMethods_Cache()
    {
        return _cachedType.GetMethods();
    }
}