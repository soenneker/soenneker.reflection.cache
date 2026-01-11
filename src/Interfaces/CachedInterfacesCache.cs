using System.Collections.Frozen;
using System;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Interfaces;

internal sealed class CachedInterfacesCache
{
    public readonly CachedType[] CachedArray;
    public readonly FrozenDictionary<string, CachedType> Map;
    public readonly Type[] TypesArray;

    public CachedInterfacesCache(CachedType[] cachedArray, FrozenDictionary<string, CachedType> map, Type[] typesArray)
    {
        CachedArray = cachedArray;
        Map = map;
        TypesArray = typesArray;
    }
}

