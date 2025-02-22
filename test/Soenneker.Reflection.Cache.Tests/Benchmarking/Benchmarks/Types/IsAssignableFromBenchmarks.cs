using System;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

[Outliers(OutlierMode.DontRemove)]
public class IsAssignableFromBenchmarks
{
    private Type _derivedType = null!;
    private Type _sourceType = null!;

    private ReflectionCache _cache = null!;
    private CachedType _cachedType = null!;

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
