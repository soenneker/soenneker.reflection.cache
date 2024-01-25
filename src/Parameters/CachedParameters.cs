﻿using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Parameters.Abstract;

namespace Soenneker.Reflection.Cache.Parameters;

///<inheritdoc cref="ICachedParameters"/>
public class CachedParameters : ICachedParameters
{
    private readonly CachedMethod? _cachedMethod;
    private readonly CachedConstructor? _cachedConstructor;

    private readonly Lazy<CachedParameter[]> _cachedArray;

    public CachedParameters(CachedMethod cachedMethod, bool threadSafe = true)
    {
        _cachedMethod = cachedMethod;

        _cachedArray = new Lazy<CachedParameter[]>(SetArrayForMethod, threadSafe);
    }

    public CachedParameters(CachedConstructor cachedConstructor, bool threadSafe = true)
    {
        _cachedConstructor = cachedConstructor;

        _cachedArray = new Lazy<CachedParameter[]>(SetArrayForConstructor, threadSafe);
    }

    private CachedParameter[] SetArrayForConstructor()
    {
        ParameterInfo[] parameters = _cachedConstructor!.ConstructorInfo!.GetParameters();
        var cachedParameters = new CachedParameter[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            cachedParameters[i] = new CachedParameter(parameters[i]);
        }

        return cachedParameters;
    }

    private CachedParameter[] SetArrayForMethod()
    {
        ParameterInfo[] parameters = _cachedMethod!.MethodInfo!.GetParameters();
        var cachedParameters = new CachedParameter[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            cachedParameters[i] = new CachedParameter(parameters[i]);
        }

        return cachedParameters;
    }

    public CachedParameter[] GetCachedParameters()
    {
        return _cachedArray.Value;
    }

    public ParameterInfo[] GetParameters()
    {
        return _cachedArray.Value.ToParameters();
    }

    public Type[] GetParametersTypes()
    {
        return _cachedArray.Value.ToParametersTypes();
    }
}