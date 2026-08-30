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
