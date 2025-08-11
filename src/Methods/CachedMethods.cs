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

    // Build all three artifacts in one go, once.
    private readonly Lazy<BuiltCache> _built;

    private readonly bool _threadSafe;

    private sealed class BuiltCache
    {
        public readonly CachedMethod[] Methods;
        public readonly FrozenDictionary<int, CachedMethod> Index;
        public readonly MethodInfo?[] MethodInfos;

        public BuiltCache(CachedMethod[] methods, FrozenDictionary<int, CachedMethod> index, MethodInfo?[] infos)
        {
            Methods = methods;
            Index = index;
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
        var dict = new Dictionary<int, CachedMethod>(count);

        for (var i = 0; i < count; i++)
        {
            var cm = new CachedMethod(methodInfos[i], _cachedTypes, _threadSafe);
            methods[i] = cm;
            dict.Add(cm.ToHashKey(), cm);
        }

        // Freeze for faster, allocation-friendly lookups
        FrozenDictionary<int, CachedMethod> frozen = dict.ToFrozenDictionary();

        // MethodInfos array without extra enumerations
        var infos = new MethodInfo?[count];
        
        for (var i = 0; i < count; i++)
        {
            infos[i] = methods[i].MethodInfo;
        }

        return new BuiltCache(methods, frozen, infos);
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
        int key = ReflectionCacheUtil.GetCacheKeyForMethod(name);
        return _built.Value.Index.GetValueOrDefault(key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? GetCachedMethod(string name, Type[] parameterTypes)
    {
        int key = ReflectionCacheUtil.GetCacheKeyForMethod(name, parameterTypes);
        return _built.Value.Index.GetValueOrDefault(key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedMethod? GetCachedMethod(string name, CachedType[] cachedParameterTypes)
    {
        int key = ReflectionCacheUtil.GetCacheKeyForMethodWithCachedParameterTypes(name, cachedParameterTypes);
        return _built.Value.Index.GetValueOrDefault(key);
    }
}