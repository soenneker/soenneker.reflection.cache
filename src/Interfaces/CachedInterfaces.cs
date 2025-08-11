using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Interfaces.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Interfaces;

/// <inheritdoc cref="ICachedInterfaces"/>
public sealed class CachedInterfaces : ICachedInterfaces
{
    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;

    // Build all data in one pass
    private readonly Lazy<BuiltCache> _built;

    private sealed class BuiltCache
    {
        public readonly CachedType[] CachedArray;
        public readonly FrozenDictionary<string, CachedType> Map; // stable, case-sensitive
        public readonly Type[] TypesArray;

        public BuiltCache(CachedType[] cachedArray, FrozenDictionary<string, CachedType> map, Type[] typesArray)
        {
            CachedArray = cachedArray;
            Map = map;
            TypesArray = typesArray;
        }
    }

    public CachedInterfaces(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType ?? throw new ArgumentNullException(nameof(cachedType));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));

        _built = new Lazy<BuiltCache>(BuildAll, threadSafe);
    }

    private BuiltCache BuildAll()
    {
        Type[] interfaces = _cachedType.Type!.GetInterfaces();
        var cachedArray = new CachedType[interfaces.Length];
        var dict = new Dictionary<string, CachedType>(interfaces.Length, StringComparer.Ordinal);

        for (var i = 0; i < interfaces.Length; i++)
        {
            CachedType cachedType = _cachedTypes.GetCachedType(interfaces[i]);
            cachedArray[i] = cachedType;
            dict[cachedType.Type!.FullName!] = cachedType;
        }

        return new BuiltCache(
            cachedArray,
            dict.ToFrozenDictionary(StringComparer.Ordinal),
            cachedArray.ToTypes()
        );
    }

    public CachedType GetCachedInterface(string typeName)
    {
        if (_built.Value.Map.TryGetValue(typeName, out CachedType? cachedType))
            return cachedType;

        // Fallback: resolve dynamically if not in initial set
        Type? interfaceType = _cachedType.Type!.GetInterface(typeName);
        CachedType result = _cachedTypes.GetCachedType(interfaceType);
        // This path won’t be stored in FrozenDictionary — can be added to a side-cache if needed
        return result!;
    }

    public Type? GetInterface(string typeName) =>
        GetCachedInterface(typeName)?.Type;

    public CachedType[] GetCachedInterfaces() =>
        _built.Value.CachedArray;

    public Type[] GetInterfaces() =>
        _built.Value.TypesArray;
}
