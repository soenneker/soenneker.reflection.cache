using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Methods;

public class GetMethodsBenchmarks
{
    private ICachedType _cachedType = default!;
    private Type _type = default!;

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