using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Parameters.Abstract;

namespace Soenneker.Reflection.Cache.Parameters;

///<inheritdoc cref="ICachedParameter"/>
public class CachedParameter : ICachedParameter
{
    public ParameterInfo ParameterInfo { get; }

    public string? Name => ParameterInfo.Name;

    public Type Type => ParameterInfo.ParameterType;

    public CachedParameter(ParameterInfo parameterInfo)
    {
        ParameterInfo = parameterInfo;
    }
}