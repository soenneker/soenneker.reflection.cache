using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Methods;

internal sealed class ConcurrentConstructedGenericCache : IConstructedGenericCache
{
    private readonly ConcurrentDictionary<TypeHandleSequenceKey, CachedMethod> _map =
        new(concurrencyLevel: Environment.ProcessorCount, capacity: 4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(TypeHandleSequenceKey key, out CachedMethod? value) => _map.TryGetValue(key, out value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetIfAbsent(TypeHandleSequenceKey key, CachedMethod value) => _map.TryAdd(key, value);
}

