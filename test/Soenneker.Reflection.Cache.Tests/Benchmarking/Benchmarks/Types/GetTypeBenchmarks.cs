using System;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Tests.Objects;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

public class GetTypeBenchmarks
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

    //[Benchmark]
    //public CachedType GetCachedType_type_Cache()
    //{
    //    return _cache.GetCachedType(_type);
    //}

    //[Benchmark]
    //public CachedType GetCachedType_type_ThreadSafe_Cache()
    //{
    //    return _threadSafeCache.GetCachedType(_type);
    //}
}