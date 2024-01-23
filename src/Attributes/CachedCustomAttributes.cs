using System;
using Soenneker.Reflection.Cache.Attributes.Abstract;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Attributes;

///<inheritdoc cref="ICachedCustomAttributes"/>
public class CachedCustomAttributes : ICachedCustomAttributes
{
    private readonly CachedType? _cachedType;
    private readonly CachedMethod? _cachedMethod;
    private readonly CachedConstructor? _cachedConstructor;

    private readonly Lazy<CachedAttribute[]> _cachedCustomAttributes;

    public CachedCustomAttributes(CachedType cachedType, bool threadSafe = true)
    {
        _cachedType = cachedType;

        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(SetArrayForType, threadSafe);
    }

    public CachedCustomAttributes(CachedMethod cachedMethod, bool threadSafe = true)
    {
        _cachedMethod = cachedMethod;

        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(SetArrayForMethod, threadSafe);
    }

    public CachedCustomAttributes(CachedConstructor cachedConstructor, bool threadSafe = true)
    {
        _cachedConstructor = cachedConstructor;

        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(SetArrayForConstructor, threadSafe);
    }

    private CachedAttribute[] SetArrayForType()
    {
        object[] attributes = _cachedType!.Type!.GetCustomAttributes(true);
        var result = new CachedAttribute[attributes.Length];

        for (var i = 0; i < attributes.Length; i++)
        {
            result[i] = new CachedAttribute(attributes[i]);
        }

        return result;
    }

    private CachedAttribute[] SetArrayForMethod()
    {
        object[] attributes = _cachedMethod!.MethodInfo!.GetCustomAttributes(true);
        var result = new CachedAttribute[attributes.Length];

        for (var i = 0; i < attributes.Length; i++)
        {
            result[i] = new CachedAttribute(attributes[i]);
        }

        return result;
    }

    private CachedAttribute[] SetArrayForConstructor()
    {
        object[] attributes = _cachedConstructor!.ConstructorInfo!.GetCustomAttributes(true);
        var result = new CachedAttribute[attributes.Length];

        for (var i = 0; i < attributes.Length; i++)
        {
            result[i] = new CachedAttribute(attributes[i]);
        }

        return result;
    }

    public CachedAttribute[] GetCachedCustomAttributes()
    {
        CachedAttribute[]? cachedAttributes = _cachedCustomAttributes.Value;
        var result = new CachedAttribute[cachedAttributes.Length];

        for (var i = 0; i < cachedAttributes.Length; i++)
        {
            result[i] = cachedAttributes[i];
        }

        return result;
    }

    public object[] GetCustomAttributes()
    {
        CachedAttribute[]? cachedAttributes = _cachedCustomAttributes.Value;
        var result = new object[cachedAttributes.Length];

        for (var i = 0; i < cachedAttributes.Length; i++)
        {
            result[i] = cachedAttributes[i].Attribute;
        }

        return result;
    }
}