using System;
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

}