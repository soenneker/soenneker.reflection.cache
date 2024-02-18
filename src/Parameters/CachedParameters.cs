using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Parameters.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Parameters;

///<inheritdoc cref="ICachedParameters"/>
public class CachedParameters : ICachedParameters
{
    private readonly CachedMethod? _cachedMethod;
    private readonly CachedConstructor? _cachedConstructor;

    private readonly Lazy<CachedParameter[]> _cachedArray;
    private readonly Lazy<ParameterInfo[]> _cachedParameterInfos;

    private readonly Lazy<CachedType[]> _cachedParameterTypes;
    private readonly Lazy<Type[]> _parameterTypes;

    private readonly CachedTypes _cachedTypes;

    public CachedParameters(CachedMethod cachedMethod, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedMethod = cachedMethod;
        _cachedTypes = cachedTypes;

        _cachedArray = new Lazy<CachedParameter[]>(() => SetArrayForMethod(threadSafe), threadSafe);

        _cachedParameterInfos = new Lazy<ParameterInfo[]>(() => _cachedArray.Value.ToParameters(), threadSafe);
        _parameterTypes = new Lazy<Type[]>(() => _cachedArray.Value.ToParametersTypes(), threadSafe);
    }

    public CachedParameters(CachedConstructor cachedConstructor, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedConstructor = cachedConstructor;
        _cachedTypes = cachedTypes;

        _cachedArray = new Lazy<CachedParameter[]>(() => SetArrayForConstructor(threadSafe), threadSafe);

        _cachedParameterInfos = new Lazy<ParameterInfo[]>(() => _cachedArray.Value.ToParameters(), threadSafe);
        _parameterTypes = new Lazy<Type[]>(() => _cachedArray.Value.ToParametersTypes(), threadSafe);
    }

    private CachedParameter[] SetArrayForConstructor(bool threadSafe)
    {
        ParameterInfo[] parameters = _cachedConstructor!.ConstructorInfo!.GetParameters();
        var cachedParameters = new CachedParameter[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            cachedParameters[i] = new CachedParameter(parameters[i], _cachedTypes, threadSafe);
        }

        return cachedParameters;
    }

    private CachedParameter[] SetArrayForMethod(bool threadSafe)
    {
        ParameterInfo[] parameters = _cachedMethod!.MethodInfo!.GetParameters();
        var cachedParameters = new CachedParameter[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            cachedParameters[i] = new CachedParameter(parameters[i], _cachedTypes, threadSafe);
        }

        return cachedParameters;
    }

    public CachedParameter[] GetCachedParameters()
    {
        return _cachedArray.Value;
    }

    public ParameterInfo[] GetParameters()
    {
        return _cachedParameterInfos.Value;
    }

    public Type[] GetParameterTypes()
    {
        return _parameterTypes.Value;
    }

    public CachedType[] GetCachedParameterTypes()
    {
        return _cachedParameterTypes.Value;
    }
}