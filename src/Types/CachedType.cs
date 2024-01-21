using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Arguments;
using Soenneker.Reflection.Cache.Arguments.Abstract;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Attributes.Abstract;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Interfaces;
using Soenneker.Reflection.Cache.Interfaces.Abstract;
using Soenneker.Reflection.Cache.Members;
using Soenneker.Reflection.Cache.Members.Abstract;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Methods.Abstract;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Properties.Abstract;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedType"/>
public class CachedType : ICachedType
{
    public Type? Type { get; }

    private int? _cacheKey;

    public int? GetCacheKey()
    {
        if (_cacheKey != null)
            return _cacheKey;

        int? key = Type?.GetHashCode();
        _cacheKey = key;
        return key;
    }

    private readonly ICachedProperties? _cachedProperties;
    private readonly ICachedMethods? _cachedMethods;
    private readonly ICachedCustomAttributes? _cachedAttributes;
    private readonly ICachedInterfaces? _cachedInterfaces;
    private readonly ICachedConstructors? _cachedConstructors;
    private readonly ICachedMembers? _cachedMembers;
    private readonly ICachedGenericArguments? _cachedGenericArguments;
    private readonly ICachedGenericTypeDefinition? _cachedGenericTypeDefinition;
    private readonly ICachedIsAssignableFrom _cachedIsAssignableFrom;

    public CachedType(Type? type)
    {
        Type = type;

        if (Type == null)
            return;

        _cachedProperties = new CachedProperties(this);
        _cachedMethods = new CachedMethods(this);
        _cachedAttributes = new CachedCustomAttributes(this);
        _cachedInterfaces = new CachedInterfaces(this);
        _cachedConstructors = new CachedConstructors(this);
        _cachedMembers = new CachedMembers(this);
        _cachedGenericArguments = new CachedGenericArguments(this);
        _cachedGenericTypeDefinition = new CachedGenericTypeDefinition(this);
        _cachedIsAssignableFrom = new CachedIsAssignableFrom(this);
    }

    public PropertyInfo? GetProperty(string property)
    {
        if (Type == null)
            return null;

        return _cachedProperties!.GetProperty(property);
    }

    public PropertyInfo[]? GetProperties()
    {
        if (Type == null)
            return null;

        return _cachedProperties!.GetProperties();
    }

    public CachedMethod? GetCachedMethod(string methodName)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.GetCachedMethod(methodName);
    }

    public MethodInfo? GetMethod(string methodName)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.GetMethod(methodName);
    }

    public CachedMethod[]? GetCachedMethods()
    {
        if (Type == null)
            return null;

        return _cachedMethods!.GetCachedMethods();
    }

    public MethodInfo?[]? GetMethods()
    {
        if (Type == null)
            return null;

        return _cachedMethods!.GetMethods();
    }

    public CachedType? GetCachedInterface(string typeName)
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.GetCachedInterface(typeName);
    }

    public CachedType[]? GetCachedInterfaces()
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.GetCachedInterfaces();
    }

    public Type? GetInterface(string typeName)
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.GetInterface(typeName);
    }

    public Type[]? GetInterfaces()
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.GetInterfaces();
    }

    public CachedAttribute[]? GetCachedCustomAttributes()
    {
        if (Type == null)
            return null;

        CachedAttribute[] result = _cachedAttributes!.GetCachedCustomAttributes();
        return result;
    }

    public object[]? GetCustomAttributes()
    {
        if (Type == null)
            return null;

        object[] result = _cachedAttributes!.GetCustomAttributes();
        return result;
    }

    public CachedConstructor? GetCachedConstructor(Type[] parameterTypes)
    {
        if (Type == null)
            return null;

        CachedConstructor? result = _cachedConstructors!.GetCachedConstructor(parameterTypes);
        return result;
    }

    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null)
    {
        if (Type == null)
            return null;

        ConstructorInfo? result = _cachedConstructors!.GetConstructor(parameterTypes);
        return result;
    }

    public CachedConstructor[]? GetCachedConstructors()
    {
        if (Type == null)
            return null;

        CachedConstructor[] result = _cachedConstructors!.GetCachedConstructors();
        return result;
    }

    public ConstructorInfo?[]? GetConstructors()
    {
        if (Type == null)
            return null;

        ConstructorInfo?[] result = _cachedConstructors!.GetConstructors();
        return result;
    }

    public object? CreateInstance()
    {
        if (Type == null)
            return null;

        object? result = _cachedConstructors!.CreateInstance();
        return result;
    }

    public object? CreateInstance(params object[] parameters)
    {
        if (Type == null)
            return null;

        object? result = _cachedConstructors!.CreateInstance(parameters);
        return result;
    }

    public T? CreateInstance<T>(params object[] parameters)
    {
        if (Type == null)
            return default;

        var result = _cachedConstructors!.CreateInstance<T>(parameters);
        return result;
    }

    public CachedType? GetCachedGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        CachedType result = _cachedGenericTypeDefinition!.GetCachedGenericTypeDefinition();
        return result;
    }

    public Type? GetGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        return _cachedGenericTypeDefinition!.GetGenericTypeDefinition();
    }

    public CachedType[]? GetCachedGenericArguments()
    {
        if (Type == null)
            return null;

        CachedType[] result = _cachedGenericArguments!.GetCachedGenericArguments();
        return result;
    }

    public Type[]? GetGenericArguments()
    {
        if (Type == null)
            return null;

        Type[] result = _cachedGenericArguments!.GetGenericArguments();
        return result;
    }

    public MemberInfo? GetMember(string name)
    {
        if (Type == null)
            return null;

        return _cachedMembers!.GetMember(name);
    }

    public MemberInfo[]? GetMembers()
    {
        if (Type == null)
            return null;

        return _cachedMembers!.GetMembers();
    }

    public bool IsAssignableFrom(Type derivedType)
    {
        if (Type == null)
            return false;

        return _cachedIsAssignableFrom.IsAssignableFrom(derivedType);
    }
}