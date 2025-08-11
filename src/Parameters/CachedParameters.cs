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
    private readonly Func<ParameterInfo[]> _getParameterInfos; // unified source for ctor/method
    private readonly CachedTypes _cachedTypes;

    private readonly Lazy<BuiltCache> _built; // build once, reuse everywhere

    private sealed class BuiltCache
    {
        public readonly CachedParameter[] CachedParams;
        public readonly ParameterInfo[] ParamInfos;
        public readonly CachedType[] CachedTypes;
        public readonly Type[] Types;

        public BuiltCache(CachedParameter[] cp, ParameterInfo[] pis, CachedType[] cts, Type[] ts)
        {
            CachedParams = cp;
            ParamInfos = pis;
            CachedTypes = cts;
            Types = ts;
        }
    }

    public CachedParameters(CachedMethod cachedMethod, CachedTypes cachedTypes, bool threadSafe = true)
    {
        if (cachedMethod is null) throw new ArgumentNullException(nameof(cachedMethod));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));

        _getParameterInfos = () =>
            cachedMethod.MethodInfo is null ? []
                                            : cachedMethod.MethodInfo.GetParameters();

        _built = new Lazy<BuiltCache>(BuildAll, threadSafe);
    }

    public CachedParameters(CachedConstructor cachedConstructor, CachedTypes cachedTypes, bool threadSafe = true)
    {
        if (cachedConstructor is null) throw new ArgumentNullException(nameof(cachedConstructor));
        _cachedTypes = cachedTypes ?? throw new ArgumentNullException(nameof(cachedTypes));

        _getParameterInfos = () =>
            cachedConstructor.ConstructorInfo is null ? []
                                                      : cachedConstructor.ConstructorInfo.GetParameters();

        _built = new Lazy<BuiltCache>(BuildAll, threadSafe);
    }

    private BuiltCache BuildAll()
    {
        ParameterInfo[] paramInfos = _getParameterInfos();
        int len = paramInfos.Length;

        if (len == 0)
        {
            return new BuiltCache(
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
            cachedParams[i] = new CachedParameter(pi, _cachedTypes, threadSafe: true);
            CachedType ct = _cachedTypes.GetCachedType(pi.ParameterType);
            cachedTypes[i] = ct;
            types[i] = ct.Type!; // cachedTypes guarantees non-null Type
        }

        return new BuiltCache(cachedParams, paramInfos, cachedTypes, types);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedParameter[] GetCachedParameters() => _built.Value.CachedParams;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ParameterInfo[] GetParameters() => _built.Value.ParamInfos;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Type[] GetParameterTypes() => _built.Value.Types;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CachedType[] GetCachedParameterTypes() => _built.Value.CachedTypes;
}
