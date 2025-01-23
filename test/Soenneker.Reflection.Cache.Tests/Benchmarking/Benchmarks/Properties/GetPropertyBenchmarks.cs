using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Properties;

[Outliers(OutlierMode.DontRemove)]
public class GetPropertyBenchmarks
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
    public PropertyInfo? GetProperty_NoCache()
    {
        return _type.GetProperty("PublicProperty1", ReflectionCacheConstants.BindingFlags);
    }

    [Benchmark]
    public PropertyInfo? GetProperty_Cache()
    {
        return _cachedType.GetProperty("PublicProperty1");
    }
}