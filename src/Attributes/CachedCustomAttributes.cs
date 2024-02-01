using System;
using Soenneker.Reflection.Cache.Attributes.Abstract;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Members;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Attributes;

///<inheritdoc cref="ICachedCustomAttributes"/>
public class CachedCustomAttributes : ICachedCustomAttributes
{
    private readonly CachedType? _cachedType;
    private readonly CachedMethod? _cachedMethod;
    private readonly CachedConstructor? _cachedConstructor;
    private readonly CachedMember? _cachedMember;

    private readonly Lazy<CachedAttribute[]> _cachedCustomAttributes;

    private readonly Lazy<object[]> _cachedObjects;
    private readonly CachedTypes _cachedTypes;

    public CachedCustomAttributes(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedType = cachedType;
        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(() => SetArrayForType(threadSafe), threadSafe);
        _cachedObjects = new Lazy<object[]>(() => _cachedCustomAttributes.Value.ToObjects(), threadSafe);
    }

    public CachedCustomAttributes(CachedMethod cachedMethod, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedMethod = cachedMethod;
        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(() => SetArrayForMethod(threadSafe), threadSafe);
        _cachedObjects = new Lazy<object[]>(() => _cachedCustomAttributes.Value.ToObjects(), threadSafe);
    }

    public CachedCustomAttributes(CachedConstructor cachedConstructor, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedConstructor = cachedConstructor;
        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(() => SetArrayForConstructor(threadSafe), threadSafe);
        _cachedObjects = new Lazy<object[]>(() => _cachedCustomAttributes.Value.ToObjects(), threadSafe);
    }

    public CachedCustomAttributes(CachedMember cachedMember, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedMember = cachedMember;
        _cachedCustomAttributes = new Lazy<CachedAttribute[]>(() => SetArrayForType(threadSafe), threadSafe);
        _cachedObjects = new Lazy<object[]>(() => _cachedCustomAttributes.Value.ToObjects(), threadSafe);
    }

    private CachedAttribute[] SetArrayForType(bool threadSafe)
    {
        object[] attributes = _cachedType!.Type!.GetCustomAttributes(true);
        var result = new CachedAttribute[attributes.Length];

        for (var i = 0; i < attributes.Length; i++)
        {
            result[i] = new CachedAttribute(attributes[i], _cachedTypes, threadSafe);
        }

        return result;
    }

    private CachedAttribute[] SetArrayForMethod(bool threadSafe)
    {
        object[] attributes = _cachedMethod!.MethodInfo!.GetCustomAttributes(true);
        var result = new CachedAttribute[attributes.Length];

        for (var i = 0; i < attributes.Length; i++)
        {
            result[i] = new CachedAttribute(attributes[i], _cachedTypes, threadSafe);
        }

        return result;
    }

    private CachedAttribute[] SetArrayForConstructor(bool threadSafe)
    {
        object[] attributes = _cachedConstructor!.ConstructorInfo!.GetCustomAttributes(true);
        var result = new CachedAttribute[attributes.Length];

        for (var i = 0; i < attributes.Length; i++)
        {
            result[i] = new CachedAttribute(attributes[i], _cachedTypes, threadSafe);
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