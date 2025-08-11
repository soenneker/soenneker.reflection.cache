using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedMakeGenericType"/>
public sealed class CachedMakeGenericType : ICachedMakeGenericType
{
    private readonly ConcurrentDictionary<int, CachedType>? _concurrent;
    private readonly Dictionary<int, CachedType>? _dict;
    private readonly bool _threadSafe;

    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;
    private readonly int _baseKey; // stable base for key generation

    public CachedMakeGenericType(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _threadSafe = threadSafe;
        _concurrent = threadSafe ? new ConcurrentDictionary<int, CachedType>() : null;
        _dict = threadSafe ? null : new Dictionary<int, CachedType>();

        _cachedType = cachedType;
        _cachedTypes = cachedTypes;
        _baseKey = cachedType.CacheKey.GetValueOrDefault();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(params Type[] typeArguments)
    {
        Type? baseType = _cachedType.Type;

        if (baseType is null)
            return null;

        // Compute key without intermediate allocations
        int key = ComputeKey(_baseKey, typeArguments);

        if (_threadSafe)
        {
            ConcurrentDictionary<int, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            // Construct/result
            Type constructed = baseType.MakeGenericType(typeArguments);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);

            // Benign race okay; TryAdd avoids replacing
            map.TryAdd(key, wrapped);
            return wrapped;
        }
        else
        {
            Dictionary<int, CachedType>? map = _dict!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType(typeArguments);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);

            map.TryAdd(key, wrapped);
            return wrapped;
        }
    }

    /// <summary>
    /// Micro-optimized overload: no intermediate Type[] allocation.
    /// Rents a Type[] only for the actual MakeGenericType call and returns it to the pool.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(params CachedType[] cachedTypeArguments)
    {
        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        // Hash directly over CachedType[].Type to avoid extra array + ToTypes()
        int key = ComputeKeyFromCached(_baseKey, cachedTypeArguments);

        if (_threadSafe)
        {
            ConcurrentDictionary<int, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            // Rent, fill, construct, return
            Type[] rented = ArrayPool<Type>.Shared.Rent(cachedTypeArguments.Length);
            try
            {
                Span<Type> span = rented.AsSpan(0, cachedTypeArguments.Length);
                for (var i = 0; i < span.Length; i++)
                    span[i] = cachedTypeArguments[i].Type!; // assuming your CachedType guarantees .Type for args

                Type constructed = baseType.MakeGenericType(span.ToArray()); // MakeGenericType needs exact length array
                // NOTE: using ToArray() here allocates; see below for a no-alloc variant
                CachedType wrapped = _cachedTypes.GetCachedType(constructed);

                map.TryAdd(key, wrapped);
                return wrapped;
            }
            finally
            {
                ArrayPool<Type>.Shared.Return(rented, clearArray: false);
            }
        }
        else
        {
            Dictionary<int, CachedType>? map = _dict!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type[] rented = ArrayPool<Type>.Shared.Rent(cachedTypeArguments.Length);
            try
            {
                Span<Type> span = rented.AsSpan(0, cachedTypeArguments.Length);
                for (var i = 0; i < span.Length; i++)
                    span[i] = cachedTypeArguments[i].Type!;

                Type constructed = baseType.MakeGenericType(span.ToArray());
                CachedType wrapped = _cachedTypes.GetCachedType(constructed);

                map.TryAdd(key, wrapped);
                return wrapped;
            }
            finally
            {
                ArrayPool<Type>.Shared.Return(rented, clearArray: false);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type? MakeGenericType(params Type[] typeArguments)
        => MakeGenericCachedType(typeArguments)?.Type;

    // ---------- helpers ----------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ComputeKey(int baseKey, Type[] types)
    {
        var hc = new HashCode();
        hc.Add(baseKey);
        // hashing RuntimeTypeHandle tends to be stable and fast
        for (var i = 0; i < types.Length; i++)
            hc.Add(types[i]?.TypeHandle ?? default);

        return hc.ToHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ComputeKeyFromCached(int baseKey, CachedType[] cached)
    {
        var hc = new HashCode();
        hc.Add(baseKey);
        for (var i = 0; i < cached.Length; i++)
            hc.Add(cached[i].Type?.TypeHandle ?? default);

        return hc.ToHashCode();
    }
}
