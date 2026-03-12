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

    // ---- allocation-reducing overloads (avoid params Type[] allocations) ----

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(Type t0)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));

        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        TypeHandleSequenceKey key = TypeHandleSequenceKey.From1(t0.TypeHandle);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType([t0]);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);
            map.TryAdd(key, wrapped);
            return wrapped;
        }

        Dictionary<TypeHandleSequenceKey, CachedType>? dict = _dict!;
        if (dict.TryGetValue(key, out CachedType? hit2))
            return hit2;

        Type constructed2 = baseType.MakeGenericType([t0]);
        CachedType wrapped2 = _cachedTypes.GetCachedType(constructed2);
        dict.TryAdd(key, wrapped2);
        return wrapped2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(Type t0, Type t1)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));

        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        TypeHandleSequenceKey key = TypeHandleSequenceKey.From2(t0.TypeHandle, t1.TypeHandle);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType([t0, t1]);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);
            map.TryAdd(key, wrapped);
            return wrapped;
        }

        Dictionary<TypeHandleSequenceKey, CachedType>? dict = _dict!;
        if (dict.TryGetValue(key, out CachedType? hit2))
            return hit2;

        Type constructed2 = baseType.MakeGenericType([t0, t1]);
        CachedType wrapped2 = _cachedTypes.GetCachedType(constructed2);
        dict.TryAdd(key, wrapped2);
        return wrapped2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(Type t0, Type t1, Type t2)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));
        if (t2 is null)
            throw new ArgumentNullException(nameof(t2));

        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        TypeHandleSequenceKey key = TypeHandleSequenceKey.From3(t0.TypeHandle, t1.TypeHandle, t2.TypeHandle);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType([t0, t1, t2]);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);
            map.TryAdd(key, wrapped);
            return wrapped;
        }

        Dictionary<TypeHandleSequenceKey, CachedType>? dict = _dict!;
        if (dict.TryGetValue(key, out CachedType? hit2))
            return hit2;

        Type constructed2 = baseType.MakeGenericType([t0, t1, t2]);
        CachedType wrapped2 = _cachedTypes.GetCachedType(constructed2);
        dict.TryAdd(key, wrapped2);
        return wrapped2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType? MakeGenericCachedType(Type t0, Type t1, Type t2, Type t3)
    {
        if (t0 is null)
            throw new ArgumentNullException(nameof(t0));
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));
        if (t2 is null)
            throw new ArgumentNullException(nameof(t2));
        if (t3 is null)
            throw new ArgumentNullException(nameof(t3));

        Type? baseType = _cachedType.Type;
        if (baseType is null)
            return null;

        TypeHandleSequenceKey key = TypeHandleSequenceKey.From4(t0.TypeHandle, t1.TypeHandle, t2.TypeHandle, t3.TypeHandle);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            Type constructed = baseType.MakeGenericType([t0, t1, t2, t3]);
            CachedType wrapped = _cachedTypes.GetCachedType(constructed);
            map.TryAdd(key, wrapped);
            return wrapped;
        }

        Dictionary<TypeHandleSequenceKey, CachedType>? dict = _dict!;
        if (dict.TryGetValue(key, out CachedType? hit2))
            return hit2;

        Type constructed2 = baseType.MakeGenericType([t0, t1, t2, t3]);
        CachedType wrapped2 = _cachedTypes.GetCachedType(constructed2);
        dict.TryAdd(key, wrapped2);
        return wrapped2;
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

        if (cachedTypeArguments is null)
            throw new ArgumentNullException(nameof(cachedTypeArguments));

        int len = cachedTypeArguments.Length;

        if (len == 0)
            return baseType.IsGenericTypeDefinition ? null : _cachedType;

        // Probe cache without allocating/filling a Type[] (allocate only on miss)
        TypeHandleSequenceKey key = TypeHandleSequenceKey.FromCachedTypes(cachedTypeArguments);

        if (_threadSafe)
        {
            ConcurrentDictionary<TypeHandleSequenceKey, CachedType>? map = _concurrent!;
            if (map.TryGetValue(key, out CachedType? hit))
                return hit;

            var typeArguments = new Type[len];
            for (var i = 0; i < len; i++)
                typeArguments[i] = cachedTypeArguments[i].Type!;

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

            var typeArguments = new Type[len];
            for (var i = 0; i < len; i++)
                typeArguments[i] = cachedTypeArguments[i].Type!;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type? MakeGenericType(Type t0) => MakeGenericCachedType(t0)?.Type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type? MakeGenericType(Type t0, Type t1) => MakeGenericCachedType(t0, t1)?.Type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type? MakeGenericType(Type t0, Type t1, Type t2) => MakeGenericCachedType(t0, t1, t2)?.Type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type? MakeGenericType(Type t0, Type t1, Type t2, Type t3) => MakeGenericCachedType(t0, t1, t2, t3)?.Type;
}
