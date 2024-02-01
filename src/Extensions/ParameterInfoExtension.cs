using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class ParameterInfoExtension
{
    public static Type[] ToParametersTypes(this ParameterInfo[] parameterInfos)
    {
        int count = parameterInfos.Length;

        var typesArray = new Type[count];

        for (var i = 0; i < count; i++)
        {
            typesArray[i] = parameterInfos[i].ParameterType;
        }

        return typesArray;
    }
}