using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Properties;

[Outliers(OutlierMode.DontRemove)]
public class PropertyAccessorBenchmarks
{
    private PropertyInfo _namePropertyInfo = null!;
    private PropertyInfo _agePropertyInfo = null!;
    private CachedProperty _nameProperty = null!;
    private CachedProperty _ageProperty = null!;
    private Person _person = null!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();
        CachedType cachedType = cache.GetCachedType(typeof(Person));

        _namePropertyInfo = typeof(Person).GetProperty(nameof(Person.Name))!;
        _agePropertyInfo = typeof(Person).GetProperty(nameof(Person.Age))!;
        _nameProperty = cachedType.GetCachedProperty(nameof(Person.Name))!;
        _ageProperty = cachedType.GetCachedProperty(nameof(Person.Age))!;
        _person = new Person();

        _nameProperty.GetGetter();
        _nameProperty.GetSetter();
        _ageProperty.GetGetter();
        _ageProperty.GetSetter();
    }

    [Benchmark(Baseline = true)]
    public void PropertyInfo_SetValue_String()
    {
        _namePropertyInfo.SetValue(_person, "Jane");
    }

    [Benchmark]
    public void CachedProperty_SetValue_String()
    {
        _nameProperty.SetValue(_person, "Jane");
    }

    [Benchmark]
    public bool CachedProperty_TrySetValue_String()
    {
        return _nameProperty.TrySetValue(_person, "Jane");
    }

    [Benchmark]
    public object? PropertyInfo_GetValue_String()
    {
        return _namePropertyInfo.GetValue(_person);
    }

    [Benchmark]
    public object? CachedProperty_GetValue_String()
    {
        return _nameProperty.GetValue(_person);
    }

    [Benchmark]
    public bool CachedProperty_TryGetValue_String()
    {
        return _nameProperty.TryGetValue(_person, out _);
    }

    [Benchmark]
    public void PropertyInfo_SetValue_Int()
    {
        _agePropertyInfo.SetValue(_person, 42);
    }

    [Benchmark]
    public void CachedProperty_SetValue_Int()
    {
        _ageProperty.SetValue(_person, 42);
    }

    [Benchmark]
    public bool CachedProperty_TrySetValue_Int()
    {
        return _ageProperty.TrySetValue(_person, 42);
    }

    [Benchmark]
    public object? PropertyInfo_GetValue_Int()
    {
        return _agePropertyInfo.GetValue(_person);
    }

    [Benchmark]
    public object? CachedProperty_GetValue_Int()
    {
        return _ageProperty.GetValue(_person);
    }

    [Benchmark]
    public bool CachedProperty_TryGetValue_Int()
    {
        return _ageProperty.TryGetValue(_person, out _);
    }

    public sealed class Person
    {
        public string? Name { get; set; }

        public int Age { get; set; }
    }
}
