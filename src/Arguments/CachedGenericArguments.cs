using System;
using Soenneker.Reflection.Cache.Arguments.Abstract;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Arguments;

///<inheritdoc cref="ICachedGenericArguments"/>
public class CachedGenericArguments : ICachedGenericArguments
{
    private readonly CachedType _cachedType;
    private readonly Lazy<CachedType[]> _cachedGenericArguments;

    private readonly CachedTypes _cachedTypes;
    private readonly Lazy<Type[]> _cachedGenericArgumentsTypes;

    public CachedGenericArguments(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedType = cachedType;
        _cachedTypes = cachedTypes;
        _cachedGenericArguments = new Lazy<CachedType[]>(SetArray, threadSafe);
        _cachedGenericArgumentsTypes = new Lazy<Type[]>(() => _cachedGenericArguments.Value.ToTypes(), threadSafe);
    }

    private CachedType[] SetArray()
    {
        Type[] types = _cachedType.Type!.GetGenericArguments();
        var result = new CachedType[types.Length];

        for (var i = 0; i < types.Length; i++)
        {
            result[i] = _cachedTypes.GetCachedType(types[i]);
        }

        return result;
    }

    public CachedType[] GetCachedGenericArguments()
    {
        return _cachedGenericArguments.Value;
    }

    public Type[] GetGenericArguments()
    {
        return _cachedGenericArgumentsTypes.Value;
    }
}