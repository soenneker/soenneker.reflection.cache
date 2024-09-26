using System;
using System.Collections.Generic;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Methods;

///<inheritdoc cref="ICachedMethod"/>
public class CachedMethod : ICachedMethod
{
    public MethodInfo? MethodInfo { get; }

    public string? Name => MethodInfo?.Name;

    public Type? ReturnType => MethodInfo?.ReturnType;

    private readonly Lazy<CachedParameters>? _parameters;
    private readonly Lazy<CachedCustomAttributes>? _attributes;

    private readonly Lazy<Dictionary<int, CachedMethod>>? _genericMethodCache;

    private readonly CachedTypes _cachedTypes;

    private readonly bool _threadSafe;

    public CachedMethod(MethodInfo? methodInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        MethodInfo = methodInfo;
        _threadSafe = threadSafe;

        if (methodInfo == null)
            return;

        _cachedTypes = cachedTypes;
        _parameters = new Lazy<CachedParameters>(() => new CachedParameters(this, cachedTypes, threadSafe), threadSafe);
        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, cachedTypes, threadSafe), threadSafe);
        _genericMethodCache = new Lazy<Dictionary<int, CachedMethod>>(threadSafe);
    }

    public CachedParameters? GetCachedParameters()
    {
        if (MethodInfo == null)
            return null;

        return _parameters!.Value;
    }

    public ParameterInfo[] GetParameters()
    {
        if (MethodInfo == null)
            return [];

        return _parameters!.Value.GetParameters();
    }

    public CachedCustomAttributes? GetCachedCustomAttributes()
    {
        if (MethodInfo == null)
            return null;

        return _attributes!.Value;
    }

    public CachedMethod? MakeCachedGenericMethod(params CachedType[] cachedTypes)
    {
        if (MethodInfo == null)
            return null;

        int key = cachedTypes.ToHashKey();

        if (_genericMethodCache!.Value.TryGetValue(key, out CachedMethod? cachedGenericMethod))
        {
            return cachedGenericMethod;
        }

        MethodInfo genericMethodInfo = MethodInfo.MakeGenericMethod(cachedTypes.ToTypes());

        var newCachedMethod = new CachedMethod(genericMethodInfo, _cachedTypes, _threadSafe);
        _genericMethodCache.Value[key] = newCachedMethod;

        return newCachedMethod;
    }

    public object[] GetCustomAttributes()
    {
        if (MethodInfo == null)
            return [];

        return _attributes!.Value.GetCustomAttributes();
    }

    public object? Invoke(object instance)
    {
        if (MethodInfo == null)
            return null;

        return MethodInfo.Invoke(instance, null);
    }

    public object? Invoke(object instance, params object[] param)
    {
        if (MethodInfo == null)
            return null;

        return MethodInfo.Invoke(instance, param);
    }

    public T? Invoke<T>(object instance)
    {
        return (T?)Invoke(instance);
    }

    public T? Invoke<T>(params object[] param)
    {
        return (T?)Invoke(param);
    }
}