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
    private CachedType _doubleDerivedType = default!;
    private Type _type = default!;

    CachedConstructor[]? cachedConstructors;

    List<CachedConstructor> _cachedConstructorsList;

    private Type _genericTypeDefinition;

    private CachedType _cachedGenericType;
    readonly Type[] _typeArguments = [typeof(int)];

    [GlobalSetup]
    public void Setup()
    {
        _cache = new ReflectionCache(false);
        _type = typeof(TestType);

        _cachedType = _cache.GetCachedType(_type);
        _doubleDerivedType = _cache.GetCachedType(typeof(DoubleDerivedDictionary));
        cachedConstructors = _cachedType.GetCachedConstructors();
        _cachedConstructorsList = new List<CachedConstructor>(cachedConstructors);

        _genericTypeDefinition = typeof(List<>);
        _cachedGenericType = _cache.GetCachedType(_genericTypeDefinition);
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

    //[Benchmark]
    //public bool IsDictionary_NoCache()
    //{
    //    return _type.IsDictionary;
    //}

    //[Benchmark]
    //public bool IsDictionary_Cache()
    //{
    //    return _doubleDerivedType.IsDictionary;
    //}

    [Benchmark(Baseline = true)]
    public Type MakeGenericType_NoCache()
    {
        return _genericTypeDefinition.MakeGenericType(_typeArguments);
    }

    [Benchmark]
    public Type? MakeGenericType_Cache()
    {
        return _cachedGenericType.MakeGenericType(_typeArguments);
    }
}