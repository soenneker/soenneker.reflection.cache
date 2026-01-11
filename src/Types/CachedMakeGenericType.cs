using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Types.Abstract;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedMakeGenericType"/>
public sealed class CachedMakeGenericType : ICachedMakeGenericType
{
    private readonly ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? _concurrent;
    private readonly Dictionary<TypeHandleSequenceKey, CachedType>? _dict;
    private readonly bool _threadSafe;

    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;

    public CachedMakeGenericType(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _threadSafe = threadSafe;
        _concurrent = threadSafe ? new ConcurrentDictionary<TypeHandleSequenceKey, CachedType>() : null;
        _dict = threadSafe ? null : new Dictionary<TypeHandleSequenceKey, CachedType>();

        _cachedType = cachedType;
        _cachedTypes = cachedTypes;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(params Type[] typeArguments)
    {
        Type? baseType = _cachedType.Type;

        if (baseType is null)
            return null;

        // Collision-free key (prevents wrong-type returns under rare hash collisions)
        TypeHandleSequenceKey key = TypeHandleSequenceKey.FromTypes(typeArguments);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
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
            Dictionary<TypeHandleSequenceKey, CachedType>? map = _dict!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType(typeArguments);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);

            map.TryAdd(key, wrapped);
            return wrapped;
        }
    }

    /// <summary>
    /// CachedType overload. Avoids string building and other intermediate work, but still requires a Type[]
    /// for MakeGenericType.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(params CachedType[] cachedTypeArguments)
    {
        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        var typeArguments = new Type[cachedTypeArguments.Length];
        TypeHandleSequenceKey key = TypeHandleSequenceKey.FromCachedTypes(cachedTypeArguments, typeArguments);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType(typeArguments);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);

            // Benign race okay; TryAdd avoids replacing
            map.TryAdd(key, wrapped);
            return wrapped;
        }
        else
        {
            Dictionary<TypeHandleSequenceKey, CachedType>? map = _dict!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType(typeArguments);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);

            map.TryAdd(key, wrapped);
            return wrapped;
        }
    }

    // ---- allocation-reducing overloads (avoid params CachedType[] allocations) ----

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(CachedType t0)
    {
        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        Type type0 = t0.Type!;
        TypeHandleSequenceKey key = TypeHandleSequenceKey.From1(type0.TypeHandle);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType([type0]);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);
            map.TryAdd(key, wrapped);
            return wrapped;
        }

        Dictionary<TypeHandleSequenceKey, CachedType>? dict = _dict!;
        if (dict.TryGetValue(key, out CachedType? hit2))
            return hit2;

        Type constructed2 = baseType.MakeGenericType([type0]);
        CachedType wrapped2 = _cachedTypes.GetCachedType(constructed2);
        dict.TryAdd(key, wrapped2);
        return wrapped2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(CachedType t0, CachedType t1)
    {
        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        Type type0 = t0.Type!;
        Type type1 = t1.Type!;
        TypeHandleSequenceKey key = TypeHandleSequenceKey.From2(type0.TypeHandle, type1.TypeHandle);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType([type0, type1]);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);
            map.TryAdd(key, wrapped);
            return wrapped;
        }

        Dictionary<TypeHandleSequenceKey, CachedType>? dict = _dict!;
        if (dict.TryGetValue(key, out CachedType? hit2))
            return hit2;

        Type constructed2 = baseType.MakeGenericType([type0, type1]);
        CachedType wrapped2 = _cachedTypes.GetCachedType(constructed2);
        dict.TryAdd(key, wrapped2);
        return wrapped2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(CachedType t0, CachedType t1, CachedType t2)
    {
        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        Type type0 = t0.Type!;
        Type type1 = t1.Type!;
        Type type2 = t2.Type!;
        TypeHandleSequenceKey key = TypeHandleSequenceKey.From3(type0.TypeHandle, type1.TypeHandle, type2.TypeHandle);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType([type0, type1, type2]);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);
            map.TryAdd(key, wrapped);
            return wrapped;
        }

        Dictionary<TypeHandleSequenceKey, CachedType>? dict = _dict!;
        if (dict.TryGetValue(key, out CachedType? hit2))
            return hit2;

        Type constructed2 = baseType.MakeGenericType([type0, type1, type2]);
        CachedType wrapped2 = _cachedTypes.GetCachedType(constructed2);
        dict.TryAdd(key, wrapped2);
        return wrapped2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(CachedType t0, CachedType t1, CachedType t2, CachedType t3)
    {
        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        Type type0 = t0.Type!;
        Type type1 = t1.Type!;
        Type type2 = t2.Type!;
        Type type3 = t3.Type!;
        TypeHandleSequenceKey key = TypeHandleSequenceKey.From4(type0.TypeHandle, type1.TypeHandle, type2.TypeHandle, type3.TypeHandle);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType([type0, type1, type2, type3]);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);
            map.TryAdd(key, wrapped);
            return wrapped;
        }

        Dictionary<TypeHandleSequenceKey, CachedType>? dict = _dict!;
        if (dict.TryGetValue(key, out CachedType? hit2))
            return hit2;

        Type constructed2 = baseType.MakeGenericType([type0, type1, type2, type3]);
        CachedType wrapped2 = _cachedTypes.GetCachedType(constructed2);
        dict.TryAdd(key, wrapped2);
        return wrapped2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type? MakeGenericType(params Type[] typeArguments)
        => MakeGenericCachedType(typeArguments)?.Type;
}
