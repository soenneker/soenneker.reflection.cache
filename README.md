[![](https://img.shields.io/nuget/v/soenneker.reflection.cache.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.reflection.cache/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.reflection.cache/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.reflection.cache/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.reflection.cache.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.reflection.cache/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.reflection.cache/build-and-test.yml?label=build%20and%20test&style=for-the-badge)](https://github.com/soenneker/soenneker.reflection.cache/actions/workflows/build-and-test.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.reflection.cache/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.reflection.cache/actions/workflows/codeql.yml)

# Soenneker.Reflection.Cache

Caches reflection metadata, lookups, constructed generics, custom attributes, and compiled member accessors for repeated use.

## Installation

```bash
dotnet add package Soenneker.Reflection.Cache
```

## Registration

```csharp
using Soenneker.Reflection.Cache.Registrars;

services.AddReflectionCacheAsSingleton();
```

A singleton gives the application one shared metadata graph and the highest cache reuse. Use `AddReflectionCacheAsScoped()` when metadata must stop being retained with a scope. The cache has no eviction API; its cached `Type`, member, attribute, and delegate references live as long as the cache does. Avoid a long-lived cache for types loaded into collectible `AssemblyLoadContext` instances.

For manual construction:

```csharp
var cache = new ReflectionCache();
```

Thread safety is enabled by default. `new ReflectionCache(threadSafe: false)` removes concurrency protection and is suitable only when the cache instance is confined to one thread.

## Follow the cached chain

```csharp
CachedType cachedType = cache.GetCachedType(typeof(OrderHandler));

CachedMethod? method = cachedType.GetCachedMethod(
    nameof(OrderHandler.Handle),
    [typeof(Order)]);

CachedParameter[]? parameters = method?.GetCachedParameters()?.GetCachedParameters();
object? result = method?.Invoke(handler, order);
```

Methods returning `CachedType`, `CachedMethod`, `CachedProperty`, `CachedField`, `CachedConstructor`, and related wrappers keep subsequent reflection work inside the cache. Convenience methods such as `GetMethod()` and `GetProperties()` return the underlying reflection objects; operations performed directly on those objects are outside the cache.

Name-only method lookup returns the first cached method with that name. Use a parameter-type overload whenever overload selection matters.

## Compiled property and field access

```csharp
CachedProperty? property = cache
    .GetCachedType(typeof(Person))
    .GetCachedProperty(nameof(Person.Name));

if (property?.TrySetValue(person, "Jane") == true &&
    property.TryGetValue(person, out object? name))
{
    Console.WriteLine(name);
}
```

Compiled accessors support public instance, non-indexed members. Static and non-public accessors, indexers, open generic/by-ref-like/pointer member types, init-only property setters, readonly fields, and setters declared on value types do not produce delegates. `TryGetValue`/`TrySetValue` return `false` for unsupported or incompatible access; `GetValue`/`SetValue` throw instead.

## Constructors, attributes, and generics

```csharp
CachedType listDefinition = cache.GetCachedType(typeof(List<>));
CachedType? closedList = listDefinition.MakeGenericCachedType(typeof(string));

CachedConstructor? constructor = closedList?.GetCachedConstructor([]);
var values = constructor?.Invoke<List<string>>();

ObsoleteAttribute? obsolete = cachedType.GetCachedCustomAttribute<ObsoleteAttribute>();
```

Closed generic types and methods are cached by their type-argument sequence. Constructor and method wrappers also cache parameter metadata, attributes, and invocation preparation.

## Binding flags

Manual construction accepts `ReflectionCacheOptions` when discovery should use different binding flags by category:

```csharp
var options = new ReflectionCacheOptions
{
    MethodFlags = BindingFlags.Instance | BindingFlags.Public,
    PropertyFlags = BindingFlags.Instance | BindingFlags.Public
};

var cache = new ReflectionCache(options);
```

The selected flags become part of that cache's behavior. Create a separate cache when another component requires a different reflection scope.

String type lookup follows `Type.GetType(string)`. Use an assembly-qualified name for types outside the calling assembly and core library. Failed name lookups are cached as unresolved results, so load required assemblies before the first lookup.

## Benchmarks (.NET)

### `GetType()` 5,772% faster

| Method                              | Mean        | Error    | StdDev   | Ratio         | RatioSD |
|------------------------------------ |------------:|---------:|---------:|--------------:|--------:|
| GetType_string_NoCache              | 1,022.30 ns | 9.462 ns | 8.851 ns |      baseline |         |
| GetType_string_Cache                |    17.52 ns | 0.303 ns | 0.283 ns | 58.38x faster |   1.12x |
| GetType_string_threadSafe_Cache     |    24.73 ns | 0.139 ns | 0.116 ns | 41.29x faster |   0.33x |
| GetCachedType_type_Cache            |    12.21 ns | 0.234 ns | 0.218 ns | 83.76x faster |   1.74x |
| GetCachedType_type_ThreadSafe_Cache |    19.01 ns | 0.067 ns | 0.052 ns | 53.73x faster |   0.47x |

### `GetMethods()` 24,842% faster

| Method             | Mean       | Error     | StdDev    | Ratio           | RatioSD |
|------------------- |-----------:|----------:|----------:|----------------:|--------:|
| GetMethods_NoCache | 256.526 ns | 1.4587 ns | 1.2180 ns |        baseline |         |
| GetMethods_Cache   |   1.030 ns | 0.0412 ns | 0.0385 ns | 249.428x faster |  10.30x |

### `GetMethod()` 37% faster

| Method            | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|------------------ |---------:|---------:|---------:|-------------:|--------:|
| GetMethod_NoCache | 23.06 ns | 0.234 ns | 0.208 ns |     baseline |         |
| GetMethod_Cache   | 16.77 ns | 0.079 ns | 0.070 ns | 1.37x faster |   0.01x |

### `GetMembers()` 83,924% faster

| Method                      | Mean        | Error     | StdDev    | Ratio           | RatioSD |
|---------------------------- |------------:|----------:|----------:|----------------:|--------:|
| GetMembers_NoCache          | 550.2334 ns | 4.1411 ns | 3.8736 ns |        baseline |         |
| GetMembers_Cache            |   0.6579 ns | 0.0515 ns | 0.0481 ns | 840.247x faster |  58.17x |
| GetMembers_ThreadSafe_Cache |   0.7273 ns | 0.0307 ns | 0.0287 ns | 757.728x faster |  31.76x |

### `GetMember()` 1,043% faster

| Method            | Mean      | Error    | StdDev   | Ratio         | RatioSD |
|------------------ |----------:|---------:|---------:|--------------:|--------:|
| GetMember_NoCache | 136.57 ns | 1.353 ns | 1.266 ns |      baseline |         |
| GetMember_Cache   |  11.95 ns | 0.091 ns | 0.081 ns | 11.43x faster |   0.12x |

### `GetProperties()` 8,960% faster

| Method                         | Mean       | Error     | StdDev    | Ratio         | RatioSD |
|------------------------------- |-----------:|----------:|----------:|--------------:|--------:|
| GetProperties_NoCache          | 58.5363 ns | 0.3463 ns | 0.3070 ns |      baseline |         |
| GetProperties_Cache            |  0.6502 ns | 0.0370 ns | 0.0328 ns | 90.25x faster |   4.59x |
| GetProperties_ThreadSafe_Cache |  0.7169 ns | 0.0129 ns | 0.0108 ns | 81.72x faster |   1.36x |

### `GetProperty()` 57% faster

| Method              | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|-------------------- |---------:|---------:|---------:|-------------:|--------:|
| GetProperty_NoCache | 25.61 ns | 0.382 ns | 0.357 ns |     baseline |         |
| GetProperty_Cache   | 16.23 ns | 0.074 ns | 0.062 ns | 1.57x faster |   0.01x |

### `GetFields()` 419% faster

| Method                     | Mean      | Error     | StdDev    | Ratio         | RatioSD |
|--------------------------- |----------:|----------:|----------:|--------------:|--------:|
| GetFields_NoCache          | 48.941 ns | 0.9092 ns | 0.8505 ns |      baseline |         |
| GetFields_Cache            |  1.141 ns | 0.0512 ns | 0.0479 ns | 42.95x faster |   1.32x |
| GetFields_ThreadSafe_Cache |  1.178 ns | 0.0144 ns | 0.0128 ns | 41.56x faster |   0.79x |

### `GetInterfaces()` 1,439% faster

| Method                | Mean       | Error     | StdDev    | Ratio         | RatioSD |
|---------------------- |-----------:|----------:|----------:|--------------:|--------:|
| GetInterfaces_NoCache | 13.1880 ns | 0.1197 ns | 0.0999 ns |      baseline |         |
| GetInterfaces_Cache   |  0.8649 ns | 0.0469 ns | 0.0439 ns | 15.39x faster |   0.58x |

### `GetInterface()` 144% faster

| Method               | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|--------------------- |---------:|---------:|---------:|-------------:|--------:|
| GetInterface_NoCache | 32.84 ns | 0.411 ns | 0.364 ns |     baseline |         |
| GetInterface_Cache   | 13.47 ns | 0.227 ns | 0.212 ns | 2.44x faster |   0.05x |

### `GetConstructors()` 4,054% faster

| Method                           | Mean       | Error     | StdDev    | Ratio         | RatioSD |
|--------------------------------- |-----------:|----------:|----------:|--------------:|--------:|
| GetConstructors_NoCache          | 38.4477 ns | 0.2020 ns | 0.1687 ns |      baseline |         |
| GetConstructors_Cache            |  0.9280 ns | 0.0109 ns | 0.0102 ns | 41.54x faster |   0.36x |
| GetConstructors_ThreadSafe_Cache |  1.0120 ns | 0.0689 ns | 0.0645 ns | 38.39x faster |   2.42x |

### `GetConstructor()` 601% faster

| Method                            | Mean      | Error    | StdDev   | Ratio        | RatioSD |
|---------------------------------- |----------:|---------:|---------:|-------------:|--------:|
| GetConstructor_NoCache            |  18.16 ns | 0.298 ns | 0.278 ns |     baseline |         |
| GetConstructor_NoCache_Parameters | 127.18 ns | 1.592 ns | 1.489 ns | 7.01x slower |   0.16x |
| GetConstructor_Cache              |  10.36 ns | 0.057 ns | 0.048 ns | 1.76x faster |   0.03x |
| GetConstructor_Cache_Parameters   |  21.12 ns | 0.366 ns | 0.342 ns | 1.16x slower |   0.02x |

### `Activator.CreateInstance(params)` vs `cachedConstructor.CreateInstance(params)` 242% faster

| Method                               | Mean      | Error    | StdDev   | Ratio        | RatioSD |
|------------------------------------- |----------:|---------:|---------:|-------------:|--------:|
| Activator_Create_with_parameters     | 325.58 ns | 1.713 ns | 1.431 ns |     baseline |         |
| Cache_CreateInstance_with_parameters |  95.61 ns | 1.943 ns | 1.908 ns | 3.42x faster |   0.07x |

### `GetCustomAttributes()` 1,658% faster

| Method                | Mean        | Error    | StdDev   | Ratio           | RatioSD |
|---------------------- |------------:|---------:|---------:|----------------:|--------:|
| GetAttributes_NoCache | 2,560.76 ns | 6.740 ns | 6.305 ns |        baseline |         |
| GetAttributes_Cache   |    15.35 ns | 0.287 ns | 0.268 ns | 166.858x faster |   2.97x |

### `GetGenericTypeDefinition()` 499% faster

| Method                           | Mean      | Error     | StdDev    | Ratio        | RatioSD |
|--------------------------------- |----------:|----------:|----------:|-------------:|--------:|
| GetGenericTypeDefinition_NoCache | 1.8759 ns | 0.0481 ns | 0.0450 ns |     baseline |         |
| GetGenericTypeDefinition_Cache   | 0.3159 ns | 0.0313 ns | 0.0293 ns | 5.99x faster |   0.58x |

### `IsAssignableFrom()` 51% faster

| Method                   | Mean     | Error     | StdDev    | Ratio        | RatioSD |
|------------------------- |---------:|----------:|----------:|-------------:|--------:|
| IsAssignableFrom_NoCache | 9.355 ns | 0.1357 ns | 0.1270 ns |     baseline |         |
| IsAssignableFrom_Cache   | 6.198 ns | 0.0794 ns | 0.0742 ns | 1.51x faster |   0.04x |

### `MakeGenericType()` 1,485% faster

| Method                  | Mean      | Error    | StdDev   | Ratio         | RatioSD |
|------------------------ |----------:|---------:|---------:|--------------:|--------:|
| MakeGenericType_NoCache | 158.56 ns | 0.900 ns | 0.842 ns |      baseline |         |
| MakeGenericType_Cache   |  10.01 ns | 0.216 ns | 0.202 ns | 15.85x faster |   0.31x |

### `GetElementType()` 1,847% faster

| Method                 | Mean      | Error     | StdDev    | Ratio         | RatioSD |
|----------------------- |----------:|----------:|----------:|--------------:|--------:|
| GetElementType_NoCache | 4.9175 ns | 0.0442 ns | 0.0369 ns |      baseline |         |
| GetElementType_Cache   | 0.2585 ns | 0.0204 ns | 0.0191 ns | 19.47x faster |   1.06x |

## Properties on `Type` (e.g. `typeof(string).IsNullable`)

| Method                | Mean      | Error     | StdDev    | Median    |
|---------------------- |----------:|----------:|----------:|----------:|
| IsAbstract_NoCache    | 2.4277 ns | 0.0319 ns | 0.0299 ns | 2.4152 ns |
| IsAbstract_Cache      | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |
| IsInterface_NoCache   | 0.7720 ns | 0.0219 ns | 0.0194 ns | 0.7662 ns |
| IsInterface_Cache     | 0.0045 ns | 0.0076 ns | 0.0071 ns | 0.0000 ns |
| IsGenericType_NoCache | 0.8707 ns | 0.0281 ns | 0.0262 ns | 0.8732 ns |
| IsGenericType_Cache   | 0.2667 ns | 0.0195 ns | 0.0182 ns | 0.2651 ns |
| IsEnum_NoCache        | 0.5322 ns | 0.0242 ns | 0.0227 ns | 0.5291 ns |
| IsEnum_Cache          | 0.2612 ns | 0.0251 ns | 0.0235 ns | 0.2526 ns |
| IsNullable_NoCache    | 1.4296 ns | 0.0297 ns | 0.0278 ns | 1.4305 ns |
| IsNullable_Cache      | 0.2312 ns | 0.0077 ns | 0.0065 ns | 0.2278 ns |
| IsByRef_NoCache       | 1.8439 ns | 0.0332 ns | 0.0310 ns | 1.8410 ns |
| IsByRef_Cache         | 0.0012 ns | 0.0025 ns | 0.0022 ns | 0.0000 ns |
| IsArray_NoCache       | 2.2914 ns | 0.0484 ns | 0.0452 ns | 2.2652 ns |
| IsArray_Cache         | 0.0060 ns | 0.0102 ns | 0.0096 ns | 0.0000 ns |

Notes:
- These benchmarks are built over iterations. The first operation is going to be as slow as the Reflection it sits in front of.
- Many of these are based off of a test class `TestType` which is located in the test library.
