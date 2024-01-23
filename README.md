[![](https://img.shields.io/nuget/v/soenneker.reflection.cache.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.reflection.cache/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.reflection.cache/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.reflection.cache/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.reflection.cache.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.reflection.cache/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Reflection.Cache
### The fastest .NET Reflection cache

Reflection is [slow](https://learn.microsoft.com/en-us/archive/msdn-magazine/2005/july/using-net-avoid-common-performance-pitfalls-for-speedier-apps).

- If you're calling some Reflection code **once**, consider if creating a cache is necessary.
- If you need to call Reflection repeatedly, this library can help speed things up.

This library is attempting to be a drop-in replacement for `System.Reflection` and caches the results of Reflection calls (so it's going to allocate more memory). It's thread-safe and supports concurrency.

## Installation

```
dotnet add package Soenneker.Reflection.Cache
```

This cache can either be added to DI like so:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddReflectionCacheAsSingleton(); // or AddReflectionCacheAsScoped()
}
```

and you could access it like:

```csharp
public class MyService
{
    private readonly IReflectionCache _cache;

    public MyService(IReflectionCache cache)
    {
        _cache = cache;
    }
}
```

or you can instantiate it manually:

```csharp
var cache = new ReflectionCache();
```

## Usage

```csharp
var type1 = Type.GetType("System.String"); // <-- regular Reflection
var type2 = cache.GetType("System.String"); // <-- cached Reflection
bool areEqual = type1 == type2; // true
```

Keep in mind:

```csharp
cache.GetType("System.String"); // <-- as slow as regular Reflection
cache.GetType("System.String"); // <-- very fast because the first call was cached
```

### ⚠️ Important ⚠️

Be mindful of the "cache chain". Use the `Cached` methods and types until you need to get the final Reflection type you need from the cache.

There are two methods for most operations like this:

```csharp
Type typeofString = cache.GetType("System.String"); // <-- caches, stops the cache chain
CachedType type = cache.GetCachedType("System.String"); // <-- caches, continues the cache chain
```


#### Scenario: Retrieving parameters from a method

✅ Good:

```csharp
CachedType cachedType = cache.GetCachedType("System.String");
CachedMethod cachedMethodInfo = cachedType.GetCachedMethod("Intern");
ParameterInfo?[] parameters = cachedMethodInfo.GetParameters(); // < -- parameters are now cached
```

❌ Bad: 

```csharp
CachedType cachedType = cache.GetCachedType("System.String");
MethodInfo methodInfo = cachedType.GetMethod("Intern"); // <-- uh oh, a non-cached Reflection type
ParameterInfo?[] parameters = methodInfo.GetParameters(); // <-- not cached, repeat calls are slow
```

### Notes

- Be thoughtful of your memory footprint and where/when you dispose of the cache.
- A cache removal mechanism is needing to be built yet.
- Many Reflection functionalities are not yet implemented, and could benefit from caching.
- If you see something that could be improved (performance or allocation), please open an issue or PR.
- This library is not yet battle-tested. Please use with caution.

---

## Benchmarks (.NET 8.0)

### `GetType()` 5,772% faster

| Method                          | Mean      | Error    | StdDev   | Ratio         | RatioSD |
|-------------------------------- |----------:|---------:|---------:|--------------:|--------:|
| GetType_string_NoCache          | 955.27 ns | 2.216 ns | 2.073 ns |      baseline |         |
| GetType_string_Cache            |  16.27 ns | 0.102 ns | 0.091 ns | 58.72x faster |   0.38x |
| GetType_string_ThreadSafe_Cache |  23.99 ns | 0.402 ns | 0.376 ns | 39.83x faster |   0.64x |


### `GetProperties()` 8,960% faster

| Method                         | Mean       | Error     | StdDev    | Ratio         | RatioSD |
|------------------------------- |-----------:|----------:|----------:|--------------:|--------:|
| GetProperties_NoCache          | 58.5363 ns | 0.3463 ns | 0.3070 ns |      baseline |         |
| GetProperties_Cache            |  0.6502 ns | 0.0370 ns | 0.0328 ns | 90.25x faster |   4.59x |
| GetProperties_ThreadSafe_Cache |  0.7169 ns | 0.0129 ns | 0.0108 ns | 81.72x faster |   1.36x |

### `GetMethods()` 599% faster

| Method             | Mean      | Error    | StdDev   | Ratio        | RatioSD |
|------------------- |----------:|---------:|---------:|-------------:|--------:|
| GetMethods_NoCache | 275.22 ns | 1.899 ns | 1.776 ns |     baseline |         |
| GetMethods_Cache   |  39.36 ns | 0.694 ns | 0.649 ns | 6.99x faster |   0.13x |

### `GetCustomAttributes()` 1,319% faster

| Method                | Mean        | Error     | StdDev    | Ratio           | RatioSD |
|---------------------- |------------:|----------:|----------:|----------------:|--------:|
| GetAttributes_NoCache | 1,982.84 ns | 14.271 ns | 12.651 ns |        baseline |         |
| GetAttributes_Cache   |    14.87 ns |  0.358 ns |  0.351 ns | 132.928x faster |   3.33x |

### `GetMethod()` 37% faster

| Method            | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|------------------ |---------:|---------:|---------:|-------------:|--------:|
| GetMethod_NoCache | 23.06 ns | 0.234 ns | 0.208 ns |     baseline |         |
| GetMethod_Cache   | 16.77 ns | 0.079 ns | 0.070 ns | 1.37x faster |   0.01x |

### `GetMembers()` 71,130% faster

| Method             | Mean        | Error     | StdDev    | Ratio           | RatioSD |
|------------------- |------------:|----------:|----------:|----------------:|--------:|
| GetMembers_NoCache | 519.4286 ns | 1.7392 ns | 1.5417 ns |        baseline |         |
| GetMembers_Cache   |   0.7297 ns | 0.0089 ns | 0.0083 ns | 712.321x faster |   7.66x |


### `GetProperty()` 52% faster

| Method              | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|-------------------- |---------:|---------:|---------:|-------------:|--------:|
| GetProperty_NoCache | 25.71 ns | 0.295 ns | 0.276 ns |     baseline |         |
| GetProperty_Cache   | 16.89 ns | 0.171 ns | 0.151 ns | 1.52x faster |   0.03x |

### `GetGenericTypeDefinition()` 420% faster

| Method                           | Mean      | Error     | StdDev    | Ratio        | RatioSD |
|--------------------------------- |----------:|----------:|----------:|-------------:|--------:|
| GetGenericTypeDefinition_NoCache | 1.8214 ns | 0.0651 ns | 0.0577 ns |     baseline |         |
| GetGenericTypeDefinition_Cache   | 0.3505 ns | 0.0123 ns | 0.0109 ns | 5.20x faster |   0.26x |

### `IsAssignableFrom()` 36% faster

| Method                   | Mean      | Error     | StdDev    | Ratio        | RatioSD |
|------------------------- |----------:|----------:|----------:|-------------:|--------:|
| IsAssignableFrom_NoCache | 11.054 ns | 0.2127 ns | 0.1989 ns |     baseline |         |
| IsAssignableFrom_Cache   |  8.133 ns | 0.1196 ns | 0.1119 ns | 1.36x faster |   0.03x |

Notes:
- These are averages over many iterations. The first operation is going to be as slow as the Reflection it sits in front of. 
- Outliers have been removed in cases BenchmarkDotnet deems necessary.