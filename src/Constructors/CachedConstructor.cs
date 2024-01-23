using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Parameters;

namespace Soenneker.Reflection.Cache.Constructors;

///<inheritdoc cref="ICachedConstructor"/>
public class CachedConstructor : ICachedConstructor
{
    public ConstructorInfo? ConstructorInfo { get; }

    public CachedCustomAttributes? Attributes { get; }

    public CachedParameters? Parameters { get; }

    public CachedConstructor(ConstructorInfo? constructorInfo, bool threadSafe = true)
    {
        ConstructorInfo = constructorInfo;

        if (constructorInfo == null)
            return;

        Attributes = new CachedCustomAttributes(this, threadSafe);
        Parameters = new CachedParameters(this, threadSafe);
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
        return (T?)Invoke();
    }

    public T? Invoke<T>(params object[] param)
    {
        return (T?)Invoke(param);
    }
}