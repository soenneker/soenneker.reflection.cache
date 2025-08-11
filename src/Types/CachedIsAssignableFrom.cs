using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

/// <inheritdoc cref="ICachedIsAssignableFrom"/>
public sealed class CachedIsAssignableFrom : ICachedIsAssignableFrom
{
    private readonly Type? _baseType;
    private readonly IAssignCache _cache;

    public CachedIsAssignableFrom(CachedType cachedType, bool threadSafe = true)
    {
        _baseType = cachedType.Type;

        _cache = threadSafe ? new ConcurrentAssignCache() : new NonConcurrentAssignCache();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsAssignableFrom(Type derivedType)
    {
        // Base type unknown: nothing assignable
        if (_baseType is null || derivedType is null)
            return false;

        // Trivial fast path
        if (ReferenceEquals(_baseType, derivedType))
            return true;

        RuntimeTypeHandle key = derivedType.TypeHandle; // stable, collision-free

        if (_cache.TryGet(key, out bool cached))
            return cached;

        bool result = _baseType.IsAssignableFrom(derivedType);
        _cache.SetIfAbsent(key, result);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsAssignableFrom(CachedType derivedType)
    {
        Type? t = derivedType.Type;
        return t is not null && IsAssignableFrom(t);
    }

    // --- cache abstraction ---

    private interface IAssignCache
    {
        bool TryGet(RuntimeTypeHandle key, out bool value);
        void SetIfAbsent(RuntimeTypeHandle key, bool value);
    }

    private sealed class NonConcurrentAssignCache : IAssignCache
    {
        // Initialize with a tiny capacity; grows if this base type is queried a lot
        private readonly Dictionary<RuntimeTypeHandle, bool> _map = new(capacity: 8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(RuntimeTypeHandle key, out bool value) => _map.TryGetValue(key, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetIfAbsent(RuntimeTypeHandle key, bool value)
        {
            // Direct set is cheaper than TryAdd; duplicates overwrite to same value
            _map.TryAdd(key, value);
        }
    }

    private sealed class ConcurrentAssignCache : IAssignCache
    {
        private readonly ConcurrentDictionary<RuntimeTypeHandle, bool> _map = new(concurrencyLevel: Environment.ProcessorCount, capacity: 8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(RuntimeTypeHandle key, out bool value) => _map.TryGetValue(key, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetIfAbsent(RuntimeTypeHandle key, bool value) => _map.TryAdd(key, value);
    }
}