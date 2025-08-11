using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Options;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedTypes"/>
public sealed class CachedTypes : ICachedTypes
{
    private readonly bool _threadSafe;

    // canonical: type -> CachedType
    private readonly ConcurrentDictionary<Type, CachedType>? _concurrentByType;
    private readonly Dictionary<Type, CachedType>? _byType;

    // name cache: input string -> CachedType (can be null-type for misses)
    private readonly ConcurrentDictionary<string, CachedType>? _concurrentByName;
    private readonly Dictionary<string, CachedType>? _byName;

    public ReflectionCacheOptions Options { get; private set; }

    public CachedTypes(ReflectionCacheOptions? options = null, bool threadSafe = true)
    {
        Options = options ?? new ReflectionCacheOptions();
        _threadSafe = threadSafe;

        if (threadSafe)
        {
            _concurrentByType = new ConcurrentDictionary<Type, CachedType>();
            _concurrentByName = new ConcurrentDictionary<string, CachedType>(StringComparer.Ordinal);
        }
        else
        {
            _byType = new Dictionary<Type, CachedType>();
            _byName = new Dictionary<string, CachedType>(StringComparer.Ordinal);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType GetCachedType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            throw new ArgumentException("typeName cannot be null or empty", nameof(typeName));

        if (_threadSafe)
        {
            ConcurrentDictionary<string, CachedType>? byName = _concurrentByName!;
            if (byName.TryGetValue(typeName, out CachedType? cached))
                return cached;

            var type = Type.GetType(typeName, throwOnError: false, ignoreCase: false);
            cached = type is null
                ? new CachedType(null, this, _threadSafe)          // negative cache
                : GetCachedType(type);                             // canonical path

            byName.TryAdd(typeName, cached);
            return cached;
        }
        else
        {
            Dictionary<string, CachedType>? byName = _byName!;
            if (byName.TryGetValue(typeName, out CachedType? cached))
                return cached;

            var type = Type.GetType(typeName, throwOnError: false, ignoreCase: false);
            cached = type is null
                ? new CachedType(null, this, _threadSafe)
                : GetCachedType(type);

            byName.TryAdd(typeName, cached);
            return cached;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType GetCachedType(Type type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        if (_threadSafe)
        {
            ConcurrentDictionary<Type, CachedType>? map = _concurrentByType!;
            if (map.TryGetValue(type, out CachedType? cached))
                return cached;

            var created = new CachedType(type, this, _threadSafe);
            if (map.TryAdd(type, created))
                return created;

            // rare race: someone else added first
            return map[type];
        }
        else
        {
            Dictionary<Type, CachedType>? map = _byType!;
            if (map.TryGetValue(type, out CachedType? cached))
                return cached;

            var created = new CachedType(type, this, _threadSafe);
            map.TryAdd(type, created);
            return created;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type? GetType(string typeName) => GetCachedType(typeName).Type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type? GetType(Type type) => GetCachedType(type).Type;
}
