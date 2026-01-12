using System;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Utils;

/// <summary>
/// A small, allocation-minimizing, collision-free key for a sequence of type arguments.
/// Stores the first 4 <see cref="RuntimeTypeHandle"/> values inline; spills the remainder to an array.
/// </summary>
internal readonly struct TypeHandleSequenceKey : IEquatable<TypeHandleSequenceKey>
{
    private readonly int _length;
    private readonly RuntimeTypeHandle _h0;
    private readonly RuntimeTypeHandle _h1;
    private readonly RuntimeTypeHandle _h2;
    private readonly RuntimeTypeHandle _h3;
    private readonly RuntimeTypeHandle[]? _rest;
    private readonly int _hash;

    private TypeHandleSequenceKey(int length, RuntimeTypeHandle h0, RuntimeTypeHandle h1, RuntimeTypeHandle h2, RuntimeTypeHandle h3,
        RuntimeTypeHandle[]? rest, int hash)
    {
        _length = length;
        _h0 = h0;
        _h1 = h1;
        _h2 = h2;
        _h3 = h3;
        _rest = rest;
        _hash = hash;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeHandleSequenceKey FromTypes(Type[] types)
    {
        if (types is null)
            throw new ArgumentNullException(nameof(types));

        int length = types.Length;

        RuntimeTypeHandle h0 = default, h1 = default, h2 = default, h3 = default;
        RuntimeTypeHandle[]? rest = null;

        var hc = new HashCode();
        hc.Add(length);

        for (var i = 0; i < length; i++)
        {
            RuntimeTypeHandle h = types[i].TypeHandle;
            hc.Add(h);

            switch (i)
            {
                case 0:
                    h0 = h;
                    break;
                case 1:
                    h1 = h;
                    break;
                case 2:
                    h2 = h;
                    break;
                case 3:
                    h3 = h;
                    break;
                default:
                    rest ??= new RuntimeTypeHandle[length - 4];
                    rest[i - 4] = h;
                    break;
            }
        }

        return new TypeHandleSequenceKey(length, h0, h1, h2, h3, rest, hc.ToHashCode());
    }

    /// <summary>
    /// Creates a key from CachedTypes and fills the provided <paramref name="typeArguments"/> array in a single pass.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeHandleSequenceKey FromCachedTypes(CachedType[] cachedTypes, Type[] typeArguments)
    {
        if (cachedTypes is null)
            throw new ArgumentNullException(nameof(cachedTypes));
        if (typeArguments is null)
            throw new ArgumentNullException(nameof(typeArguments));
        if (typeArguments.Length != cachedTypes.Length)
            throw new ArgumentException("typeArguments must match cachedTypes length", nameof(typeArguments));

        int length = cachedTypes.Length;

        RuntimeTypeHandle h0 = default, h1 = default, h2 = default, h3 = default;
        RuntimeTypeHandle[]? rest = null;

        var hc = new HashCode();
        hc.Add(length);

        for (var i = 0; i < length; i++)
        {
            Type t = cachedTypes[i].Type!;
            typeArguments[i] = t;

            RuntimeTypeHandle h = t.TypeHandle;
            hc.Add(h);

            switch (i)
            {
                case 0:
                    h0 = h;
                    break;
                case 1:
                    h1 = h;
                    break;
                case 2:
                    h2 = h;
                    break;
                case 3:
                    h3 = h;
                    break;
                default:
                    rest ??= new RuntimeTypeHandle[length - 4];
                    rest[i - 4] = h;
                    break;
            }
        }

        return new TypeHandleSequenceKey(length, h0, h1, h2, h3, rest, hc.ToHashCode());
    }

    /// <summary>
    /// Creates a key from CachedTypes without allocating/filling a Type[].
    /// Useful when you want to probe a cache first and only allocate the Type[] on a miss.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeHandleSequenceKey FromCachedTypes(CachedType[] cachedTypes)
    {
        if (cachedTypes is null)
            throw new ArgumentNullException(nameof(cachedTypes));

        int length = cachedTypes.Length;

        RuntimeTypeHandle h0 = default, h1 = default, h2 = default, h3 = default;
        RuntimeTypeHandle[]? rest = null;

        var hc = new HashCode();
        hc.Add(length);

        for (var i = 0; i < length; i++)
        {
            RuntimeTypeHandle h = cachedTypes[i].Type!.TypeHandle;
            hc.Add(h);

            switch (i)
            {
                case 0:
                    h0 = h;
                    break;
                case 1:
                    h1 = h;
                    break;
                case 2:
                    h2 = h;
                    break;
                case 3:
                    h3 = h;
                    break;
                default:
                    rest ??= new RuntimeTypeHandle[length - 4];
                    rest[i - 4] = h;
                    break;
            }
        }

        return new TypeHandleSequenceKey(length, h0, h1, h2, h3, rest, hc.ToHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeHandleSequenceKey From1(RuntimeTypeHandle h0)
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(h0);
        return new TypeHandleSequenceKey(1, h0, default, default, default, null, hc.ToHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeHandleSequenceKey From2(RuntimeTypeHandle h0, RuntimeTypeHandle h1)
    {
        var hc = new HashCode();
        hc.Add(2);
        hc.Add(h0);
        hc.Add(h1);
        return new TypeHandleSequenceKey(2, h0, h1, default, default, null, hc.ToHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeHandleSequenceKey From3(RuntimeTypeHandle h0, RuntimeTypeHandle h1, RuntimeTypeHandle h2)
    {
        var hc = new HashCode();
        hc.Add(3);
        hc.Add(h0);
        hc.Add(h1);
        hc.Add(h2);
        return new TypeHandleSequenceKey(3, h0, h1, h2, default, null, hc.ToHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeHandleSequenceKey From4(RuntimeTypeHandle h0, RuntimeTypeHandle h1, RuntimeTypeHandle h2, RuntimeTypeHandle h3)
    {
        var hc = new HashCode();
        hc.Add(4);
        hc.Add(h0);
        hc.Add(h1);
        hc.Add(h2);
        hc.Add(h3);
        return new TypeHandleSequenceKey(4, h0, h1, h2, h3, null, hc.ToHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(TypeHandleSequenceKey other)
    {
        if (_length != other._length)
            return false;

        if (_hash != other._hash)
            return false;

        // RuntimeTypeHandle doesn't provide unambiguous ==/!= operators across TFMs; use Equals.
        if (!_h0.Equals(other._h0) || !_h1.Equals(other._h1) || !_h2.Equals(other._h2) || !_h3.Equals(other._h3))
            return false;

        // length <= 4 => nothing spilled
        if (_length <= 4)
            return true;

        RuntimeTypeHandle[]? a = _rest;
        RuntimeTypeHandle[]? b = other._rest;

        // both should be non-null if length > 4, but keep it defensive
        if (a is null || b is null)
            return false;

        int len = _length - 4;
        for (var i = 0; i < len; i++)
        {
            if (!a[i].Equals(b[i]))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj) => obj is TypeHandleSequenceKey other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _hash;
}

