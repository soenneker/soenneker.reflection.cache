using Soenneker.Reflection.Cache.Parameters;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedParametersExtension
{
    public static ParameterInfo[] ToParameters(this CachedParameter[] cachedParameters)
    {
        int count = cachedParameters.Length;

        var parameterInfosArray = new ParameterInfo[count];

        for (var i = 0; i < count; i++)
        {
            parameterInfosArray[i] = cachedParameters[i].ParameterInfo;
        }

        return parameterInfosArray;
    }

    public static Type[] ToParametersTypes(this CachedParameter[] cachedParameters)
    {
        int count = cachedParameters.Length;

        var typesArray = new Type[count];

        for (var i = 0; i < count; i++)
        {
            typesArray[i] = cachedParameters[i].ParameterInfo.ParameterType;
        }

        return typesArray;
    }
}