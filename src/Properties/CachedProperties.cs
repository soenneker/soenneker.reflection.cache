using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Properties.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Properties;

///<inheritdoc cref="ICachedProperties"/>
public sealed class CachedProperties : ICachedProperties
{
    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;

    // Build once; immutable after warmup
    private readonly Lazy<CachedPropertiesCache> _built;

    public CachedProperties(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType ?? throw new ArgumentNullException(nameof(cachedType));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));

        _built = new Lazy<CachedPropertiesCache>(BuildAll, threadSafe);
    }

    private CachedPropertiesCache BuildAll()
    {
        // Single reflection hit; reuse array
        PropertyInfo[] props = _cachedType.Type!.GetProperties(_cachedTypes.Options.PropertyFlags);
        int len = props.Length;

        var cached = new CachedProperty[len];
        var dict = new Dictionary<string, CachedProperty>(len, StringComparer.Ordinal);

        for (var i = 0; i < len; i++)
        {
            PropertyInfo pi = props[i];
            var cp = new CachedProperty(pi, _cachedTypes, threadSafe: true);
            cached[i] = cp;

            // Property names are unique per declaring type for given flags; using name matches original behavior.
            // If you want to guard against rare name collisions across hides/new, consider key $"{pi.DeclaringType!.FullName}|{pi.Name}".
            dict[pi.Name] = cp;
        }

        return new CachedPropertiesCache(cached, dict.ToFrozenDictionary(StringComparer.Ordinal), props);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PropertyInfo? GetProperty(string name) => GetCachedProperty(name)?.PropertyInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedProperty? GetCachedProperty(string name) => _built.Value.MapByName.GetValueOrDefault(name);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PropertyInfo[] GetProperties() => _built.Value.PropertyInfos;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedProperty[] GetCachedProperties() => _built.Value.CachedArray;
}