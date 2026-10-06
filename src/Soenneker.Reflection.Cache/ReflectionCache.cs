using System;
using Soenneker.Reflection.Cache.Abstract;
using Soenneker.Reflection.Cache.Options;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache;

/// <inheritdoc cref="IReflectionCache" />
public class ReflectionCache : IReflectionCache
{
    private readonly CachedTypes _cachedTypes;

    public ReflectionCache(ReflectionCacheOptions? options = null, bool threadSafe = true)
    {
        _cachedTypes = new CachedTypes(options, threadSafe);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType GetCachedType(string typeName)
    {
        return _cachedTypes.GetCachedType(typeName);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType GetCachedType(Type type)
    {
        return _cachedTypes.GetCachedType(type);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? GetType(string typeName)
    {
        return _cachedTypes.GetType(typeName);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? GetType(Type type)
    {
        return _cachedTypes.GetType(type);
    }
}
