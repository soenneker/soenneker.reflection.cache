using System;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

public class CachedTypeBenchmarks
{
    private ReflectionCache _cache = default!;

    private CachedType _cachedType = default!;
    private Type _type = default!;

    [GlobalSetup]
    public void Setup()
    {
        _cache = new ReflectionCache(false);
        _type = typeof(TestType);

        _cachedType = _cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public bool IsAbstract_NoCache()
    {
        return _type.IsAbstract;
    }

    [Benchmark]
    public bool IsAbstract_Cache()
    {
        return _cachedType.IsAbstract;
    }

    [Benchmark]
    public bool IsInterface_NoCache()
    {
        return _type.IsInterface;
    }

    [Benchmark]
    public bool IsInterface_Cache()
    {
        return _cachedType.IsInterface;
    }

    [Benchmark]
    public bool IsGenericType_NoCache()
    {
        return _type.IsGenericType;
    }

    [Benchmark]
    public bool IsGenericType_Cache()
    {
        return _cachedType.IsGenericType;
    }

    [Benchmark]
    public bool IsEnum_NoCache()
    {
        return _type.IsEnum;
    }

    [Benchmark]
    public bool IsEnum_Cache()
    {
        return _cachedType.IsEnum;
    }

    [Benchmark]
    public bool IsNullable_NoCache()
    {
        return Nullable.GetUnderlyingType(_type) != null;
    }

    [Benchmark]
    public bool IsNullable_Cache()
    {
        return _cachedType.IsNullable;
    }
}