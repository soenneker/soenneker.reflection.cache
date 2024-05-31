using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

[MemoryDiagnoser(false)]
public class GetConstructorsBenchmarks
{
    private CachedType _cachedType = default!;
    private CachedType _threadSafeCachedType = default!;
    private Type _type = default!;

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
    public ConstructorInfo[] GetConstructors_NoCache()
    {
        return _type.GetConstructors(ReflectionCacheConstants.BindingFlags);
    }

    [Benchmark]
    public ConstructorInfo?[]? GetConstructors_Cache()
    {
        return _cachedType.GetConstructors();
    }

    //[Benchmark]
    //public ConstructorInfo?[]? GetConstructors_ThreadSafe_Cache()
    //{
    //    return _threadSafeCachedType.GetConstructors();
    //}
}