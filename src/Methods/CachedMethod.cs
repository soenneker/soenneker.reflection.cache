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

    public CachedParameters? Parameters { get; }

    public CachedCustomAttributes? Attributes { get; }

    public CachedMethod(MethodInfo? methodInfo)
    {
        MethodInfo = methodInfo;

        if (methodInfo == null)
            return;

        Parameters = new CachedParameters(this);
        Attributes = new CachedCustomAttributes(this);
    }
}