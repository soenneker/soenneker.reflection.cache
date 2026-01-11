using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Parameters;

internal sealed class CachedParametersCache
{
    public readonly CachedParameter[] CachedParams;
    public readonly ParameterInfo[] ParamInfos;
    public readonly CachedType[] CachedTypes;
    public readonly Type[] Types;

    public CachedParametersCache(CachedParameter[] cp, ParameterInfo[] pis, CachedType[] cts, Type[] ts)
    {
        CachedParams = cp;
        ParamInfos = pis;
        CachedTypes = cts;
        Types = ts;
    }
}

