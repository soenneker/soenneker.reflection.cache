using System;
using Soenneker.Reflection.Cache.Attributes.Abstract;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Extensions;
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

    private readonly Lazy<object[]> _cachedObjects;

    public CachedCustomAttributes(CachedType cachedType, bool threadSafe = true)
    {
        _cachedType = cachedType;
        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(SetArrayForType, threadSafe);
        _cachedObjects = new Lazy<object[]>(_cachedCustomAttributes.Value.ToObjects, threadSafe);
    }

    public CachedCustomAttributes(CachedMethod cachedMethod, bool threadSafe = true)
    {
        _cachedMethod = cachedMethod;
        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(SetArrayForMethod, threadSafe);
        _cachedObjects = new Lazy<object[]>(_cachedCustomAttributes.Value.ToObjects, threadSafe);
    }

    public CachedCustomAttributes(CachedConstructor cachedConstructor, bool threadSafe = true)
    {
        _cachedConstructor = cachedConstructor;
        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(SetArrayForConstructor, threadSafe);
        _cachedObjects = new Lazy<object[]>(_cachedCustomAttributes.Value.ToObjects, threadSafe);
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
        return _cachedCustomAttributes.Value;
    }

    public object[] GetCustomAttributes()
    {
        return _cachedObjects.Value;
    }
}