using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Methods.Abstract;

internal interface IConstructedGenericCache
{
    bool TryGet(TypeHandleSequenceKey key, out CachedMethod? value);
    void SetIfAbsent(TypeHandleSequenceKey key, CachedMethod value);
}

