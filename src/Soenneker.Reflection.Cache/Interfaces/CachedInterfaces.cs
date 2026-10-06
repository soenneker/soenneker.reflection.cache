using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Soenneker.Reflection.Cache.Interfaces.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Interfaces;

/// <inheritdoc cref="ICachedInterfaces"/>
public sealed class CachedInterfaces : ICachedInterfaces
{
    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;

    private readonly CachedInterfacesCache _built;

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedInterfaces(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType ?? throw new ArgumentNullException(nameof(cachedType));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));

        _built = BuildAll();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    private CachedInterfacesCache BuildAll()
    {
        Type[] interfaces = _cachedType.Type!.GetInterfaces();
        var cachedArray = new CachedType[interfaces.Length];
        var dict = new Dictionary<string, CachedType>(interfaces.Length * 2, StringComparer.Ordinal);

        for (var i = 0; i < interfaces.Length; i++)
        {
            CachedType cachedType = _cachedTypes.GetCachedType(interfaces[i]);
            cachedArray[i] = cachedType;
            Type interfaceType = cachedType.Type!;
            dict.TryAdd(interfaceType.Name, cachedType);

            if (interfaceType.FullName is { } fullName)
                dict.TryAdd(fullName, cachedType);
        }

        return new CachedInterfacesCache(
            cachedArray,
            dict.ToFrozenDictionary(StringComparer.Ordinal),
            interfaces
        );
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType GetCachedInterface(string typeName)
    {
        if (_built.Map.TryGetValue(typeName, out CachedType? cachedType))
            return cachedType;

        // Fallback: resolve dynamically if not in initial set
        Type? interfaceType = _cachedType.Type!.GetInterface(typeName);
        return interfaceType is null ? null! : _cachedTypes.GetCachedType(interfaceType);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? GetInterface(string typeName) =>
        GetCachedInterface(typeName)?.Type;

    public CachedType[] GetCachedInterfaces() =>
        _built.CachedArray;

    public Type[] GetInterfaces() =>
        _built.TypesArray;
}
