using System;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

[Outliers(OutlierMode.DontRemove)]
public class GetCachedTypeBenchmarks
{
    private ReflectionCache _cache = default!;
    private ReflectionCache _threadSafeCache = default!;
    private Type _type = default!;

    [GlobalSetup]
    public void Setup()
    {
        _cache = new ReflectionCache(null, false);
        _threadSafeCache = new ReflectionCache();
        _type = typeof(TestType);
    }

    [Benchmark]
    public CachedType GetCachedType_type_Cache()
    {
        return _cache.GetCachedType(_type);
    }

    [Benchmark]
    public CachedType GetCachedType_type_ThreadSafe_Cache()
    {
        return _threadSafeCache.GetCachedType(_type);
    }
}