using System;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

public class IsAssignableFromBenchmarks
{
    private Type _derivedType = default!;
    private Type _sourceType = default!;

    private ReflectionCache _cache = default!;
    private CachedType _cachedType = default!;

    [GlobalSetup]
    public void Setup()
    {
        _derivedType = typeof(DerivedType);
        _sourceType = typeof(BaseType);

        _cache = new ReflectionCache();
        _cachedType = _cache.GetCachedType(_sourceType);
    }

    [Benchmark(Baseline = true)]
    public bool IsAssignableFrom_NoCache()
    {
        return _sourceType.IsAssignableFrom(_derivedType);
    }

    [Benchmark]
    public bool IsAssignableFrom_Cache()
    {
        return _cachedType.IsAssignableFrom(_derivedType);
    }
}
