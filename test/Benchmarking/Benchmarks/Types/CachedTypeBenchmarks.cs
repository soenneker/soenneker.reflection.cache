using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Tests.Objects;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

public class CachedTypeBenchmarks
{
    private ReflectionCache _cache = default!;

    private CachedType _cachedType = default!;
    private Type _type = default!;

    CachedConstructor[]? cachedConstructors;

    List<CachedConstructor> cachedConstructorsList;

    [GlobalSetup]
    public void Setup()
    {
        _cache = new ReflectionCache(false);
        _type = typeof(TestType);

        _cachedType = _cache.GetCachedType(_type);
        cachedConstructors = _cachedType.GetCachedConstructors();
        cachedConstructorsList = new List<CachedConstructor>(cachedConstructors);
    }

    //[Benchmark]
    //public bool IsAbstract_Cache()
    //{
    //    return _cachedType.IsAbstract;
    //}

    //[Benchmark]
    //public bool IsInterface_NoCache()
    //{
    //    return _type.IsInterface;
    //}

    //[Benchmark]
    //public bool IsInterface_Cache()
    //{
    //    return _cachedType.IsInterface;
    //}

    //[Benchmark]
    //public bool IsGenericType_NoCache()
    //{
    //    return _type.IsGenericType;
    //}

    //[Benchmark]
    //public bool IsGenericType_Cache()
    //{
    //    return _cachedType.IsGenericType;
    //}

    //[Benchmark]
    //public bool IsEnum_NoCache()
    //{
    //    return _type.IsEnum;
    //}

    //[Benchmark]
    //public bool IsEnum_Cache()
    //{
    //    return _cachedType.IsEnum;
    //}

    //[Benchmark]
    //public bool IsNullable_NoCache()
    //{
    //    return Nullable.GetUnderlyingType(_type) != null;
    //}

    //[Benchmark]
    //public bool IsNullable_Cache()
    //{
    //    return _cachedType.IsNullable;
    //}

    //[Benchmark]
    //public bool IsByRef_NoCache()
    //{
    //    return _type.IsByRef;
    //}

    //[Benchmark]
    //public bool IsByRef_Cache()
    //{
    //    return _cachedType.IsByRef;
    //}

    //[Benchmark]
    //public bool IsArray_NoCache()
    //{
    //    return _type.IsArray;
    //}

    //[Benchmark]
    //public bool IsArray_Cache()
    //{
    //    return _cachedType.IsArray;
    //}
}
