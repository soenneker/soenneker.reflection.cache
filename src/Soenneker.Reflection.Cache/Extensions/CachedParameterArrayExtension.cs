using Soenneker.Reflection.Cache.Parameters;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedParameterArrayExtension
{
    public static ParameterInfo[] ToParameterInfos(this CachedParameter[] cachedParameters)
    {
        int length = cachedParameters.Length;

        if (length == 0)
            return [];

        var parameterInfosArray = new ParameterInfo[length];

        for (var i = 0; i < length; i++)
        {
            parameterInfosArray[i] = cachedParameters[i].ParameterInfo;
        }

        return parameterInfosArray;
    }

    public static Type[] ToParametersTypes(this CachedParameter[] cachedParameters)
    {
        int length = cachedParameters.Length;

        if (length == 0)
            return [];

        var typesArray = new Type[length];

        for (var i = 0; i < length; i++)
        {
            typesArray[i] = cachedParameters[i].ParameterInfo.ParameterType;
        }

        return typesArray;
    }
}