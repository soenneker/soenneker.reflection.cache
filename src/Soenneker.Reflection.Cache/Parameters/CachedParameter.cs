using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Parameters.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Parameters;

/// <inheritdoc cref="ICachedParameter"/>
public sealed class CachedParameter : ICachedParameter
{
    public ParameterInfo ParameterInfo { get; }

    public string? Name => ParameterInfo.Name;

    public Type ParameterType => ParameterInfo.ParameterType;

    public CachedType CachedParameterType { get; }

    public CachedParameter(ParameterInfo parameterInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        ParameterInfo = parameterInfo;
        CachedParameterType = cachedTypes.GetCachedType(parameterInfo.ParameterType);
    }

    internal CachedParameter(ParameterInfo parameterInfo, CachedType cachedParameterType)
    {
        ParameterInfo = parameterInfo;
        CachedParameterType = cachedParameterType;
    }
}
