using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Members;

[Outliers(OutlierMode.DontRemove)]
public class GetMemberBenchmarks
{
    private CachedType _cachedTyped = null!;
    private Type _type = null!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();
        _type = typeof(TestType);
        _cachedTyped = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public MemberInfo[] GetMember_NoCache()
    {
        return _type.GetMember("PublicMember1");
    }

    //[Benchmark]
    //public MemberInfo? GetMember_Cache()
    //{
    //    return _cachedTyped.GetMember("PublicMember1");
    //}
}
