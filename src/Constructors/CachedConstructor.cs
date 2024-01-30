﻿using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Parameters;

namespace Soenneker.Reflection.Cache.Constructors;

///<inheritdoc cref="ICachedConstructor"/>
public class CachedConstructor : ICachedConstructor
{
    public ConstructorInfo? ConstructorInfo { get; }

    private readonly Lazy<CachedCustomAttributes>? _attributes;

    private readonly Lazy<CachedParameters>? _parameters;

    public CachedConstructor(ConstructorInfo? constructorInfo, bool threadSafe = true)
    {
        ConstructorInfo = constructorInfo;

        if (constructorInfo == null)
            return;

        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, threadSafe), threadSafe);
        _parameters = new Lazy<CachedParameters>(() => new CachedParameters(this, threadSafe), threadSafe);
    }

    public CachedParameter[] GetCachedParameters()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetCachedParameters();
    }

    public ParameterInfo[] GetParameters()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetParameters();
    }

    public CachedAttribute[] GetCachedCustomAttributes()
    {
        if (ConstructorInfo == null)
            return [];

        return _attributes!.Value.GetCachedCustomAttributes();
    }

    public object[] GetCustomAttributes()
    {
        if (ConstructorInfo == null)
            return [];

        return _attributes!.Value.GetCustomAttributes();
    }

    public Type[] GetParametersTypes()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetParametersTypes();
    }

    public object? Invoke()
    {
        if (ConstructorInfo == null)
            return null;

        return ConstructorInfo.Invoke(null);
    }

    public object? Invoke(params object[] param)
    {
        if (ConstructorInfo == null)
            return null;

        return ConstructorInfo.Invoke(param);
    }

    public T? Invoke<T>()
    {
        return (T?) Invoke();
    }

    public T? Invoke<T>(params object[] param)
    {
        return (T?) Invoke(param);
    }
}