using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Members;

[Outliers(OutlierMode.DontRemove)]
public class GetMembersBenchmarks
{
    private CachedType _cachedThreadSafeType = null!;
    private CachedType _cachedType = null!;
    private Type _type = null!;

    [GlobalSetup]
    public void Setup()
    {
        var threadSafeCache = new ReflectionCache();
        var cache = new ReflectionCache(null, false);
        _type = typeof(TestType);

        _cachedThreadSafeType = threadSafeCache.GetCachedType(_type);
        _cachedType = cache.GetCachedType(_type);
    }

    [Benchmark(Baseline = true)]
    public MemberInfo[] GetMembers_NoCache()
    {
        return _type.GetMembers(ReflectionCacheConstants.BindingFlags);
    }

    [Benchmark]
    public MemberInfo?[]? GetMembers_Cache()
    {
        return _cachedType.GetMembers();
    }

    [Benchmark]
    public MemberInfo?[]? GetMembers_ThreadSafe_Cache()
    {
        return _cachedThreadSafeType.GetMembers();
    }
}
