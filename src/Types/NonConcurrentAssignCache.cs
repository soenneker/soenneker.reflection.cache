using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

internal sealed class NonConcurrentAssignCache : IAssignCache
{
    // Initialize with a tiny capacity; grows if this base type is queried a lot
    private readonly Dictionary<RuntimeTypeHandle, bool> _map = new(capacity: 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(RuntimeTypeHandle key, out bool value) => _map.TryGetValue(key, out value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetIfAbsent(RuntimeTypeHandle key, bool value) => _map.TryAdd(key, value);
}