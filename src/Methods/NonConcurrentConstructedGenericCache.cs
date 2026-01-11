using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Methods;

internal sealed class NonConcurrentConstructedGenericCache : IConstructedGenericCache
{
    private readonly Dictionary<TypeHandleSequenceKey, CachedMethod> _map = new(capacity: 4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(TypeHandleSequenceKey key, out CachedMethod? value) => _map.TryGetValue(key, out value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetIfAbsent(TypeHandleSequenceKey key, CachedMethod value) => _map.TryAdd(key, value);
}

