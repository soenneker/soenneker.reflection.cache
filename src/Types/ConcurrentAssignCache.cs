using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

internal sealed class ConcurrentAssignCache : IAssignCache
{
    private readonly ConcurrentDictionary<RuntimeTypeHandle, bool> _map = new(concurrencyLevel: Environment.ProcessorCount, capacity: 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(RuntimeTypeHandle key, out bool value) => _map.TryGetValue(key, out value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetIfAbsent(RuntimeTypeHandle key, bool value) => _map.TryAdd(key, value);
}

