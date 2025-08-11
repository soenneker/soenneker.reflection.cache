using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Fields.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Fields;

///<inheritdoc cref="ICachedFields"/>
public sealed class CachedFields : ICachedFields
{
    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;

    private readonly Lazy<BuiltCache> _built; // single source of truth

    private sealed class BuiltCache
    {
        public readonly CachedField[] CachedArray;
        public readonly FrozenDictionary<string, CachedField> MapByName;
        public readonly FieldInfo[] FieldInfos;

        public BuiltCache(CachedField[] cachedArray, FrozenDictionary<string, CachedField> mapByName, FieldInfo[] fieldInfos)
        {
            CachedArray = cachedArray;
            MapByName = mapByName;
            FieldInfos = fieldInfos;
        }
    }

    public CachedFields(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType ?? throw new ArgumentNullException(nameof(cachedType));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));

        _built = new Lazy<BuiltCache>(BuildAll, threadSafe);
    }

    private BuiltCache BuildAll()
    {
        // One reflection hit + one allocation of FieldInfo[]
        FieldInfo[] fields = _cachedType.Type!.GetFields(_cachedTypes.Options.FieldFlags);
        int len = fields.Length;

        var cached = new CachedField[len];
        var dict = new Dictionary<string, CachedField>(len, StringComparer.Ordinal);

        for (var i = 0; i < len; i++)
        {
            FieldInfo fi = fields[i];
            var cf = new CachedField(fi, _cachedTypes, threadSafe: true); // thread safety here matches previous behavior
            cached[i] = cf;
            dict[fi.Name] = cf; // field names are unique per type for given BindingFlags
        }

        return new BuiltCache(
            cached,
            dict.ToFrozenDictionary(StringComparer.Ordinal),
            fields // keep original FieldInfo[] so we don't re-convert later
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FieldInfo? GetField(string name)
        => GetCachedField(name)?.FieldInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedField? GetCachedField(string name)
    {
        // Fast, allocation-free lookup
        return _built.Value.MapByName.GetValueOrDefault(name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FieldInfo[] GetFields()
        => _built.Value.FieldInfos;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedField[] GetCachedFields()
        => _built.Value.CachedArray;
}
