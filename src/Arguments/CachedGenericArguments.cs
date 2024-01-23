using System;
using Soenneker.Reflection.Cache.Arguments.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Arguments;

///<inheritdoc cref="ICachedGenericArguments"/>
public class CachedGenericArguments : ICachedGenericArguments
{
    private readonly CachedType _cachedType;
    private readonly Lazy<CachedType[]> _cachedGenericArguments;

    public CachedGenericArguments(CachedType cachedType, bool threadSafe = true)
    {
        _cachedType = cachedType;
        _cachedGenericArguments = new Lazy<CachedType[]>(SetArray, threadSafe);
    }

    private CachedType[] SetArray()
    {
        Type[] types = _cachedType.Type!.GetGenericArguments();
        var result = new CachedType[types.Length];

        for (var i = 0; i < types.Length; i++)
        {
            result[i] = new CachedType(types[i]);
        }

        return result;
    }

    public CachedType[] GetCachedGenericArguments()
    {
        return _cachedGenericArguments.Value;
    }

    public Type[] GetGenericArguments()
    {
        CachedType[]? cachedArguments = _cachedGenericArguments.Value;
        var result = new Type[cachedArguments.Length];

        for (var i = 0; i < cachedArguments.Length; i++)
        {
            result[i] = cachedArguments[i].Type!;
        }

        return result;
    }
}