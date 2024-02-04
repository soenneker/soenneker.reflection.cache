using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class ParameterInfoExtension
{
    public static Type[] ToParametersTypes(this ParameterInfo[] parameterInfos)
    {
        ReadOnlySpan<ParameterInfo> span = parameterInfos;

        var typesArray = new Type[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            typesArray[i] = span[i].ParameterType;
        }

        return typesArray;
    }
}