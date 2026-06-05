using Soenneker.Reflection.Cache.Parameters;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the cached parameter array extension.
/// </summary>
public static class CachedParameterArrayExtension
{
    /// <summary>
    /// Executes the to parameter infos operation.
    /// </summary>
    /// <param name="cachedParameters">The cached parameters.</param>
    /// <returns>The result of the operation.</returns>
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

    /// <summary>
    /// Executes the to parameters types operation.
    /// </summary>
    /// <param name="cachedParameters">The cached parameters.</param>
    /// <returns>The result of the operation.</returns>
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