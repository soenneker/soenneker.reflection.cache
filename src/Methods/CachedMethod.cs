using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Parameters;

namespace Soenneker.Reflection.Cache.Methods;

///<inheritdoc cref="ICachedMethod"/>
public class CachedMethod : ICachedMethod
{
    public MethodInfo? MethodInfo { get; }

    public string? Name => MethodInfo?.Name;

    public Type? ReturnType => MethodInfo?.ReturnType;

    private readonly Lazy<CachedParameters>? _parameters;
    private readonly Lazy<CachedCustomAttributes>? _attributes;

    public CachedMethod(MethodInfo? methodInfo, bool threadSafe = true)
    {
        MethodInfo = methodInfo;

        if (methodInfo == null)
            return;

        _parameters = new Lazy<CachedParameters>(() => new CachedParameters(this), threadSafe);
        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this), threadSafe);
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