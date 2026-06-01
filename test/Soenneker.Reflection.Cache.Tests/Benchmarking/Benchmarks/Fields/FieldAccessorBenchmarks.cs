using System.Reflection;
using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Fields;

[Outliers(OutlierMode.DontRemove)]
public class FieldAccessorBenchmarks
{
    private FieldInfo _nameFieldInfo = null!;
    private FieldInfo _ageFieldInfo = null!;
    private CachedField _nameField = null!;
    private CachedField _ageField = null!;
    private Person _person = null!;

    [GlobalSetup]
    public void Setup()
    {
        var cache = new ReflectionCache();
        CachedType cachedType = cache.GetCachedType(typeof(Person));

        _nameFieldInfo = typeof(Person).GetField(nameof(Person.Name))!;
        _ageFieldInfo = typeof(Person).GetField(nameof(Person.Age))!;
        _nameField = cachedType.GetCachedField(nameof(Person.Name))!;
        _ageField = cachedType.GetCachedField(nameof(Person.Age))!;
        _person = new Person();

        _nameField.GetGetter();
        _nameField.GetSetter();
        _ageField.GetGetter();
        _ageField.GetSetter();
    }

    [Benchmark(Baseline = true)]
    public void FieldInfo_SetValue_String()
    {
        _nameFieldInfo.SetValue(_person, "Jane");
    }

    [Benchmark]
    public void CachedField_SetValue_String()
    {
        _nameField.SetValue(_person, "Jane");
    }

    [Benchmark]
    public bool CachedField_TrySetValue_String()
    {
        return _nameField.TrySetValue(_person, "Jane");
    }

    [Benchmark]
    public object? FieldInfo_GetValue_String()
    {
        return _nameFieldInfo.GetValue(_person);
    }

    [Benchmark]
    public object? CachedField_GetValue_String()
    {
        return _nameField.GetValue(_person);
    }

    [Benchmark]
    public bool CachedField_TryGetValue_String()
    {
        return _nameField.TryGetValue(_person, out _);
    }

    [Benchmark]
    public void FieldInfo_SetValue_Int()
    {
        _ageFieldInfo.SetValue(_person, 42);
    }

    [Benchmark]
    public void CachedField_SetValue_Int()
    {
        _ageField.SetValue(_person, 42);
    }

    [Benchmark]
    public bool CachedField_TrySetValue_Int()
    {
        return _ageField.TrySetValue(_person, 42);
    }

    [Benchmark]
    public object? FieldInfo_GetValue_Int()
    {
        return _ageFieldInfo.GetValue(_person);
    }

    [Benchmark]
    public object? CachedField_GetValue_Int()
    {
        return _ageField.GetValue(_person);
    }

    [Benchmark]
    public bool CachedField_TryGetValue_Int()
    {
        return _ageField.TryGetValue(_person, out _);
    }

    public sealed class Person
    {
        public string? Name;

        public int Age;
    }
}
