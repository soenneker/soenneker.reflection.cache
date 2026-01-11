using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Types;
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
    private readonly Lazy<CachedMethodsCache> _built;

    private readonly bool _threadSafe;

    public CachedMethods(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType ?? throw new ArgumentNullException(nameof(cachedType));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));
        _threadSafe = threadSafe;

        _built = new Lazy<CachedMethodsCache>(BuildAll, isThreadSafe: threadSafe);
    }

    private CachedMethodsCache BuildAll()
    {
        // Single reflection hit
        MethodInfo[] methodInfos = _cachedType.Type!.GetMethods(_cachedTypes.Options.MethodFlags);
        int count = methodInfos.Length;

        var methods = new CachedMethod[count];
        var names = new string[count];
        var countsByName = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var i = 0; i < count; i++)
        {
            var cm = new CachedMethod(methodInfos[i], _cachedTypes, _threadSafe);
            methods[i] = cm;

            string name = cm.Name!;
            names[i] = name;
            countsByName[name] = countsByName.TryGetValue(name, out int c) ? c + 1 : 1;
        }

        // Build arrays directly (avoid List<> allocations + ToArray copies)
        var byName = new Dictionary<string, CachedMethod[]>(countsByName.Count, StringComparer.Ordinal);
        var fill = new Dictionary<string, int>(countsByName.Count, StringComparer.Ordinal);

        foreach (KeyValuePair<string, int> kvp in countsByName)
        {
            byName[kvp.Key] = new CachedMethod[kvp.Value];
            fill[kvp.Key] = 0;
        }

        for (var i = 0; i < count; i++)
        {
            string name = names[i];
            int idx = fill[name];
            byName[name][idx] = methods[i];
            fill[name] = idx + 1;
        }

        FrozenDictionary<string, CachedMethod[]> methodsByName = byName.ToFrozenDictionary(StringComparer.Ordinal);

        // MethodInfos array without extra enumerations
        var infos = new MethodInfo?[count];
        for (var i = 0; i < count; i++)
        {
            infos[i] = methods[i].MethodInfo;
        }

        return new CachedMethodsCache(methods, methodsByName, infos);
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
    public MethodInfo? GetMethod(string name, Type t0) => GetCachedMethod(name, t0)?.MethodInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MethodInfo? GetMethod(string name, Type t0, Type t1) => GetCachedMethod(name, t0, t1)?.MethodInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MethodInfo? GetMethod(string name, Type t0, Type t1, Type t2) => GetCachedMethod(name, t0, t1, t2)?.MethodInfo;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MethodInfo? GetMethod(string name, Type t0, Type t1, Type t2, Type t3) => GetCachedMethod(name, t0, t1, t2, t3)?.MethodInfo;

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
    public CachedMethod? GetCachedMethod(string name, Type t0)
    {
        if (!_built.Value.MethodsByName.TryGetValue(name, out CachedMethod[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != 1)
                continue;

            if (ReferenceEquals(ps[0].ParameterType, t0))
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? GetCachedMethod(string name, Type t0, Type t1)
    {
        if (!_built.Value.MethodsByName.TryGetValue(name, out CachedMethod[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != 2)
                continue;

            if (ReferenceEquals(ps[0].ParameterType, t0) && ReferenceEquals(ps[1].ParameterType, t1))
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? GetCachedMethod(string name, Type t0, Type t1, Type t2)
    {
        if (!_built.Value.MethodsByName.TryGetValue(name, out CachedMethod[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != 3)
                continue;

            if (ReferenceEquals(ps[0].ParameterType, t0) && ReferenceEquals(ps[1].ParameterType, t1) && ReferenceEquals(ps[2].ParameterType, t2))
                return candidates[i];
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? GetCachedMethod(string name, Type t0, Type t1, Type t2, Type t3)
    {
        if (!_built.Value.MethodsByName.TryGetValue(name, out CachedMethod[]? candidates) || candidates.Length == 0)
            return null;

        for (var i = 0; i < candidates.Length; i++)
        {
            ParameterInfo[] ps = candidates[i].GetParameters();
            if (ps.Length != 4)
                continue;

            if (ReferenceEquals(ps[0].ParameterType, t0) && ReferenceEquals(ps[1].ParameterType, t1) && ReferenceEquals(ps[2].ParameterType, t2) &&
                ReferenceEquals(ps[3].ParameterType, t3))
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