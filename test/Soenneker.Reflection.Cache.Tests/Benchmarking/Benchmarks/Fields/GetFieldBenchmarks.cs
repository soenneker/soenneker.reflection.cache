using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Fields;

[Outliers(OutlierMode.DontRemove)]
public class GetFieldBenchmarks
{
    private CachedType _cachedType = default!;
    private Type _type = default!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();
        _type = typeof(TestType);

        _cachedType = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public FieldInfo? GetField_NoCache()
    {
        return _type.GetField("PublicField", ReflectionCacheConstants.BindingFlags);
    }

    [Benchmark]
    public FieldInfo? GetField_Cache()
    {
        return _cachedType.GetField("PublicField");
    }
}
