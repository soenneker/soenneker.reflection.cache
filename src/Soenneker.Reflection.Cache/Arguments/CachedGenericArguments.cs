using System;
using Soenneker.Reflection.Cache.Arguments.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Arguments;

/// <inheritdoc cref="ICachedGenericArguments"/>
public sealed class CachedGenericArguments : ICachedGenericArguments
{
    private readonly CachedType[] _cachedGenericArguments;
    private readonly Type[] _genericArguments;

    public CachedGenericArguments(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _genericArguments = cachedType.Type!.GetGenericArguments();
        _cachedGenericArguments = new CachedType[_genericArguments.Length];

        for (var i = 0; i < _genericArguments.Length; i++)
        {
            _cachedGenericArguments[i] = cachedTypes.GetCachedType(_genericArguments[i]);
        }
    }

    public CachedType[] GetCachedGenericArguments()
    {
        return _cachedGenericArguments;
    }

    public Type[] GetGenericArguments()
    {
        return _genericArguments;
    }
}
