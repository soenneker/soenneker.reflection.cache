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

```charp
var cache = new ReflectionCache();
```

## Usage

```csharp
var type1 = cache.GetType("System.String");
var type2 = Type.GetType("System.String");
bool areEqual = type1 == type2; // true
```

Keep in mind:

```csharp
cache.GetType("System.String"); // <-- this is going to be as slow as regular Reflection
cache.GetType("System.String"); // <-- this will be very, very fast because the first call was cached
```

### ⚠️ Important ⚠️

Be mindful of the "cache chain". Use the `Cached` methods and types until you need to get the final Reflection type you need from the cache.

There are two methods for most operations like this:

```csharp
Type typeofString = cache.GetType("System.String"); // <-- Caches, stops the cache chain
CachedType cachedType = cache.GetCachedType("System.String"); // <-- Caches, continues the cache chain
```


#### Scenario: Retrieving parameters from a method

✅ Good:

```csharp
CachedType cachedType = cache.GetCachedType("System.String");
CachedMethod cachedMethodInfo = cachedType.GetCachedMethod("Intern");
ParameterInfo?[] parameters = cachedMethodInfo.GetParameters(); // < -- Cached, and subsequent use will be fast
```

❌ Bad: 

```csharp
CachedType cachedType = cache.GetCachedType("System.String");
MethodInfo methodInfo = cachedType.GetMethod("Intern"); // <-- Uh oh, a non-cached Reflection type
ParameterInfo?[] parameters = methodInfo.GetParameters(); // <-- Not cached and repeated calls will be slow
```

### Notes

- Be thoughtful of your memory footprint and where/when you dispose of the cache.
- A cache removal mechanism is needing to be built yet.
- Many Reflection functionalities are not yet implemented, and could benefit from caching.
- If you see something that could be improved (performance or allocation), please open an issue or PR.
- This library is not yet battle-tested. Please use with caution.

---

## Benchmarks (.NET 8.0)

### `GetType()` - 3,482% faster

| Method                 | Mean      | Error    | StdDev   | Ratio         | RatioSD |
|----------------------- |----------:|---------:|---------:|--------------:|--------:|
| GetType_string_NoCache | 950.07 ns | 3.089 ns | 2.738 ns |      baseline |         |
| GetType_string_Cache   |  26.60 ns | 0.589 ns | 0.678 ns | 35.82x faster |   1.02x |


### `GetProperties()` - 5,160% faster

| Method                | Mean      | Error     | StdDev    | Ratio         | RatioSD |
|---------------------- |----------:|----------:|----------:|--------------:|--------:|
| GetProperties_NoCache | 58.765 ns | 0.6131 ns | 0.5735 ns |      baseline |         |
| GetProperties_Cache   |  1.119 ns | 0.0481 ns | 0.0450 ns | 52.59x faster |   2.27x |

### `GetMethods()` - 599% faster

| Method             | Mean      | Error    | StdDev   | Ratio        | RatioSD |
|------------------- |----------:|---------:|---------:|-------------:|--------:|
| GetMethods_NoCache | 275.22 ns | 1.899 ns | 1.776 ns |     baseline |         |
| GetMethods_Cache   |  39.36 ns | 0.694 ns | 0.649 ns | 6.99x faster |   0.13x |

### `GetCustomAttributes()` - 1,319% faster

| Method                | Mean        | Error     | StdDev    | Ratio           | RatioSD |
|---------------------- |------------:|----------:|----------:|----------------:|--------:|
| GetAttributes_NoCache | 1,982.84 ns | 14.271 ns | 12.651 ns |        baseline |         |
| GetAttributes_Cache   |    14.87 ns |  0.358 ns |  0.351 ns | 132.928x faster |   3.33x |

### `GetMethod()` - 37% faster

| Method            | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|------------------ |---------:|---------:|---------:|-------------:|--------:|
| GetMethod_NoCache | 23.06 ns | 0.234 ns | 0.208 ns |     baseline |         |
| GetMethod_Cache   | 16.77 ns | 0.079 ns | 0.070 ns | 1.37x faster |   0.01x |


### `GetProperty()` - 52% faster

| Method              | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|-------------------- |---------:|---------:|---------:|-------------:|--------:|
| GetProperty_NoCache | 25.71 ns | 0.295 ns | 0.276 ns |     baseline |         |
| GetProperty_Cache   | 16.89 ns | 0.171 ns | 0.151 ns | 1.52x faster |   0.03x |

### `GetGenericTypeDefinition()` - 420% faster

| Method                           | Mean      | Error     | StdDev    | Ratio        | RatioSD |
|--------------------------------- |----------:|----------:|----------:|-------------:|--------:|
| GetGenericTypeDefinition_NoCache | 1.8214 ns | 0.0651 ns | 0.0577 ns |     baseline |         |
| GetGenericTypeDefinition_Cache   | 0.3505 ns | 0.0123 ns | 0.0109 ns | 5.20x faster |   0.26x |

### `IsAssignableFrom()` - 36% faster

| Method                   | Mean      | Error     | StdDev    | Ratio        | RatioSD |
|------------------------- |----------:|----------:|----------:|-------------:|--------:|
| IsAssignableFrom_NoCache | 11.054 ns | 0.2127 ns | 0.1989 ns |     baseline |         |
| IsAssignableFrom_Cache   |  8.133 ns | 0.1196 ns | 0.1119 ns | 1.36x faster |   0.03x |