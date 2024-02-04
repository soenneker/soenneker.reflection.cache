using Soenneker.Reflection.Cache.Parameters;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedParametersExtension
{
    public static ParameterInfo[] ToParameters(this CachedParameter[] cachedParameters)
    {
        ReadOnlySpan<CachedParameter> span = cachedParameters;

        var parameterInfosArray = new ParameterInfo[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            parameterInfosArray[i] = span[i].ParameterInfo;
        }

        return parameterInfosArray;
    }

    public static Type[] ToParametersTypes(this CachedParameter[] cachedParameters)
    {
        ReadOnlySpan<CachedParameter> span = cachedParameters;

        var typesArray = new Type[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            typesArray[i] = span[i].ParameterInfo.ParameterType;
        }

        return typesArray;
    }
}