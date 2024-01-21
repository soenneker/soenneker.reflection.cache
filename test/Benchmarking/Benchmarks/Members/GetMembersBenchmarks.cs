using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Constants;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Members;

public class GetMembersBenchmarks
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
    public MemberInfo[] GetMembers_NoCache()
    {
        return _type.GetMembers(ReflectionCacheConstants.BindingFlags);
    }

    [Benchmark]
    public MemberInfo?[]? GetMembers_Cache()
    {
        return _cachedType.GetMembers();
    }
}
