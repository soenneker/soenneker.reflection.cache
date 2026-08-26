using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Parameters.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Parameters;

///<inheritdoc cref="ICachedParameters"/>
public sealed class CachedParameters : ICachedParameters
{
    private readonly ParameterInfo[] _parameterInfos;
    private readonly CachedTypes _cachedTypes;

    private readonly CachedParametersCache _built;

    public CachedParameters(CachedMethod cachedMethod, CachedTypes cachedTypes, bool threadSafe = true)
    {
        if (cachedMethod is null) throw new ArgumentNullException(nameof(cachedMethod));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));
        _parameterInfos = cachedMethod.MethodInfo?.GetParameters() ?? [];
        _built = BuildAll();
    }

    public CachedParameters(CachedConstructor cachedConstructor, CachedTypes cachedTypes, bool threadSafe = true)
    {
        if (cachedConstructor is null) throw new ArgumentNullException(nameof(cachedConstructor));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));
        _parameterInfos = cachedConstructor.ConstructorInfo?.GetParameters() ?? [];
        _built = BuildAll();
    }

    internal CachedParameters(ParameterInfo[] parameterInfos, CachedTypes cachedTypes)
    {
        _parameterInfos = parameterInfos;
        _cachedTypes = cachedTypes;
        _built = BuildAll();
    }

    private CachedParametersCache BuildAll()
    {
        ParameterInfo[] paramInfos = _parameterInfos;
        int len = paramInfos.Length;

        if (len == 0)
        {
            return new CachedParametersCache(
                [],
                [],
                [],
                []
            );
        }

        var cachedParams = new CachedParameter[len];
        var cachedTypes = new CachedType[len];
        var types = new Type[len];

        for (var i = 0; i < len; i++)
        {
            ParameterInfo pi = paramInfos[i];
            CachedType ct = _cachedTypes.GetCachedType(pi.ParameterType);
            cachedParams[i] = new CachedParameter(pi, ct);
            cachedTypes[i] = ct;
            types[i] = ct.Type!; // cachedTypes guarantees non-null Type
        }

        return new CachedParametersCache(cachedParams, paramInfos, cachedTypes, types);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedParameter[] GetCachedParameters() => _built.CachedParams;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ParameterInfo[] GetParameters() => _built.ParamInfos;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type[] GetParameterTypes() => _built.Types;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType[] GetCachedParameterTypes() => _built.CachedTypes;
}
