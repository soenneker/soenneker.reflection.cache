using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Properties;

[Outliers(OutlierMode.DontRemove)]
public class GetPropertiesBenchmarks
{
    private CachedType _cachedType = null!;
    private CachedType _threadSafeCachedType = null!;
    private Type _type = null!;


    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache(null, false);
        var threadSafeCache = new ReflectionCache();

        _type = typeof(TestType);

        _cachedType = cache.GetCachedType(_type);
        _threadSafeCachedType = threadSafeCache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public PropertyInfo[] GetProperties_NoCache()
    {
        return _type.GetProperties(ReflectionCacheConstants.BindingFlags);
    }

    [Benchmark]
    public PropertyInfo[]? GetProperties_Cache()
    {
        return _cachedType.GetProperties();
    }

    [Benchmark]
    public PropertyInfo[]? GetProperties_ThreadSafe_Cache()
    {
        return _threadSafeCachedType.GetProperties();
    }
}