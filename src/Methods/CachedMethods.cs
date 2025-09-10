using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Reflection.Cache.Utils;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Soenneker.Reflection.Cache.Methods;

///<inheritdoc cref="ICachedMethods"/>
public sealed class CachedMethods : ICachedMethods
{
    private readonly CachedType _cachedType;
    private readonly CachedTypes _cachedTypes;

    // Build all artifacts in one go, once.
    private readonly Lazy<BuiltCache> _built;

    private readonly bool _threadSafe;

    private sealed class BuiltCache
    {
        public readonly CachedMethod[] Methods;
        public readonly FrozenDictionary<string, CachedMethod[]> MethodsByName;
        public readonly MethodInfo?[] MethodInfos;

        public BuiltCache(CachedMethod[] methods, FrozenDictionary<string, CachedMethod[]> methodsByName, MethodInfo?[] infos)
        {
            Methods = methods;
            MethodsByName = methodsByName;
            MethodInfos = infos;
        }
    }

    public CachedMethods(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType ?? throw new ArgumentNullException(nameof(cachedType));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));
        _threadSafe = threadSafe;

        _built = new Lazy<BuiltCache>(BuildAll, isThreadSafe: threadSafe);
    }

    private BuiltCache BuildAll()
    {
        // Single reflection hit
        MethodInfo[] methodInfos = _cachedType.Type!.GetMethods(_cachedTypes.Options.MethodFlags);
        int count = methodInfos.Length;

        var methods = new CachedMethod[count];
        var byName = new Dictionary<string, List<CachedMethod>>(StringComparer.Ordinal);

        for (var i = 0; i < count; i++)
        {
            var cm = new CachedMethod(methodInfos[i], _cachedTypes, _threadSafe);
            methods[i] = cm;

            string name = cm.Name!;
            if (!byName.TryGetValue(name, out List<CachedMethod>? list))
            {
                list = new List<CachedMethod>();
                byName[name] = list;
            }
            list.Add(cm);
        }

        // Freeze name map
        var frozenByName = new Dictionary<string, CachedMethod[]>(byName.Count, StringComparer.Ordinal);
        foreach (KeyValuePair<string, List<CachedMethod>> kvp in byName)
        {
            frozenByName[kvp.Key] = kvp.Value.ToArray();
        }

        FrozenDictionary<string, CachedMethod[]> methodsByName = frozenByName.ToFrozenDictionary(StringComparer.Ordinal);

        // MethodInfos array without extra enumerations
        var infos = new MethodInfo?[count];
        for (var i = 0; i < count; i++)
        {
            infos[i] = methods[i].MethodInfo;
        }

        return new BuiltCache(methods, methodsByName, infos);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod[] GetCachedMethods() => _built.Value.Methods;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MethodInfo?[] GetMethods() => _built.Value.MethodInfos;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MethodInfo? GetMethod(string name) => GetCachedMethod(name)?.MethodInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MethodInfo? GetMethod(string name, Type[] types) => GetCachedMethod(name, types)?.MethodInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? GetCachedMethod(string name)
    {
        if (!_built.Value.MethodsByName.TryGetValue(name, out CachedMethod[]? candidates) || candidates.Length == 0)
            return null;

        // If there is a single candidate, return it; otherwise prefer parameterless
        if (candidates.Length == 1)
            return candidates[0];

        for (var i = 0; i < candidates.Length; i++)
        {
            ParameterInfo[]? ps = candidates[i].GetParameters();
            if (ps.Length == 0)
                return candidates[i];
        }

        // Fallback: first candidate
        return candidates[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? GetCachedMethod(string name, Type[] parameterTypes)
    {
        if (!_built.Value.MethodsByName.TryGetValue(name, out CachedMethod[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != parameterTypes.Length)
                continue;

            var match = true;
            for (var j = 0; j < ps.Length; j++)
            {
                if (!ReferenceEquals(ps[j].ParameterType, parameterTypes[j]))
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? GetCachedMethod(string name, CachedType[] cachedParameterTypes)
    {
        if (!_built.Value.MethodsByName.TryGetValue(name, out CachedMethod[]? candidates) || candidates.Length == 0)
            return null;

        int len = cachedParameterTypes?.Length ?? 0;

        for (var i = 0; i < candidates.Length; i++)
        {
            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != len)
                continue;

            var match = true;
            for (var j = 0; j < len; j++)
            {
                Type? t = cachedParameterTypes[j].Type;
                if (!ReferenceEquals(ps[j].ParameterType, t))
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return candidates[i];
        }

        return null;
    }
}