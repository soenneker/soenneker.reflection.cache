using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Parameters.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Parameters;

///<inheritdoc cref="ICachedParameter"/>
public class CachedParameter : ICachedParameter
{
    public ParameterInfo ParameterInfo { get; }

    public string? Name => ParameterInfo.Name;

    public Type ParameterType => _parameterTypeLazy.Value;
    private readonly Lazy<Type> _parameterTypeLazy;

    public CachedType CachedParameterType => _lazyCachedParameterType.Value;
    private readonly Lazy<CachedType> _lazyCachedParameterType;

    public CachedParameter(ParameterInfo parameterInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        ParameterInfo = parameterInfo;

        _lazyCachedParameterType = new Lazy<CachedType>(() => cachedTypes.GetCachedType(parameterInfo.ParameterType), threadSafe);
        _parameterTypeLazy = new Lazy<Type>(() => _lazyCachedParameterType.Value.Type!, threadSafe);
    }
}