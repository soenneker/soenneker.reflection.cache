using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

public class CachedConstructorBenchmarks
{
    private ReflectionCache _cache = default!;

    CachedConstructor _cachedConstructor;

    [GlobalSetup]
    public void Setup()
    {
        _cache = new ReflectionCache(null, false);
        Type type = typeof(TestType);

        CachedType _cachedType = _cache.GetCachedType(type);

        _cachedConstructor = _cachedType.GetCachedConstructors().FirstOrDefault();
    }

    [Benchmark]
    public bool IsPublic_Cache()
    {
        return _cachedConstructor.IsPublic;
    }

    [Benchmark]
    public bool IsPublic_NoCache()
    {
        return _cachedConstructor.ConstructorInfo.IsPublic;
    }

    [Benchmark]
    public bool IsStatic_Cache()
    {
        return _cachedConstructor.IsStatic;
    }

    [Benchmark]
    public bool IsStatic_NoCache()
    {
        return _cachedConstructor.ConstructorInfo.IsStatic;
    }
}