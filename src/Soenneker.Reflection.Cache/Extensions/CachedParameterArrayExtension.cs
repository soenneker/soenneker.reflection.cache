using Soenneker.Reflection.Cache.Parameters;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides conversions for arrays of cached parameters.
/// </summary>
public static class CachedParameterArrayExtension
{
    /// <summary>
    /// Extracts the underlying parameter metadata.
    /// </summary>
    /// <param name="cachedParameters">The cached parameters.</param>
    /// <returns>The parameter metadata in declaration order.</returns>
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
    /// Extracts each parameter's declared type.
    /// </summary>
    /// <param name="cachedParameters">The cached parameters.</param>
    /// <returns>The parameter types in declaration order.</returns>
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
