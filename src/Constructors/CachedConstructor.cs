using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Constructors;

///<inheritdoc cref="ICachedConstructor"/>
public sealed partial class CachedConstructor : ICachedConstructor
{
    public ConstructorInfo? ConstructorInfo { get; }

    private readonly Lazy<CachedCustomAttributes>? _attributes;

    private readonly Lazy<CachedParameters>? _parameters;

    private readonly bool _threadSafe;

    public CachedConstructor(ConstructorInfo? constructorInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        ConstructorInfo = constructorInfo;
        _threadSafe = threadSafe;

        InitializeProperties();

        if (constructorInfo == null)
            return;

        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, cachedTypes, threadSafe), threadSafe);
        _parameters = new Lazy<CachedParameters>(() => new CachedParameters(this, cachedTypes, threadSafe), threadSafe);
    }

    public CachedParameter[] GetCachedParameters()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetCachedParameters();
    }

    public ParameterInfo[] GetParameters()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetParameters();
    }

    public CachedAttribute[] GetCachedCustomAttributes()
    {
        if (ConstructorInfo == null)
            return [];

        return _attributes!.Value.GetCachedCustomAttributes();
    }

    public object[] GetCustomAttributes()
    {
        if (ConstructorInfo == null)
            return [];

        return _attributes!.Value.GetCustomAttributes();
    }

    public Type[] GetParametersTypes()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetParameterTypes();
    }

    public CachedType[] GetCachedParameterTypes()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetCachedParameterTypes();
    }

    public object? Invoke()
    {
        if (ConstructorInfo == null)
            return null;

        return ConstructorInfo.Invoke(null);
    }

    public object? Invoke(params object[] param)
    {
        if (ConstructorInfo == null)
            return null;

        return ConstructorInfo.Invoke(param);
    }

    public T? Invoke<T>()
    {
        return (T?) Invoke();
    }

    public T? Invoke<T>(params object[] param)
    {
        return (T?) Invoke(param);
    }
}