using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Arguments;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Interfaces;
using Soenneker.Reflection.Cache.Members;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedType"/>
public class CachedType : ICachedType
{
    public Type? Type { get; }

    public int? CacheKey => _cacheKeyLazy.Value;
    private readonly Lazy<int?> _cacheKeyLazy;

    public bool IsAbstract => _isAbstractLazy.Value;
    private readonly Lazy<bool> _isAbstractLazy;

    public bool IsInterface => _isInterfaceLazy.Value;
    private readonly Lazy<bool> _isInterfaceLazy;

    public bool IsGenericType => _isGenericTypeLazy.Value;
    private readonly Lazy<bool> _isGenericTypeLazy;

    public bool IsEnum => _isEnumLazy.Value;
    private readonly Lazy<bool> _isEnumLazy;

    public bool IsNullable => _isNullable.Value;
    private readonly Lazy<bool> _isNullable;

    private readonly CachedProperties? _cachedProperties;
    private readonly CachedMethods? _cachedMethods;
    private readonly CachedCustomAttributes? _cachedAttributes;
    private readonly CachedInterfaces? _cachedInterfaces;
    private readonly CachedConstructors? _cachedConstructors;
    private readonly CachedMembers? _cachedMembers;
    private readonly CachedGenericArguments? _cachedGenericArguments;
    private readonly CachedGenericTypeDefinition? _cachedGenericTypeDefinition;
    private readonly CachedIsAssignableFrom? _cachedIsAssignableFrom;

    public CachedType(Type? type, bool threadSafe = true)
    {
        Type = type;

        _cacheKeyLazy = new Lazy<int?>(() => Type?.GetHashCode(), threadSafe);

        _isAbstractLazy = new Lazy<bool>(() => Type is {IsAbstract: true}, threadSafe);
        _isInterfaceLazy = new Lazy<bool>(() => type is {IsInterface: true}, threadSafe);
        _isGenericTypeLazy = new Lazy<bool>(() => type is {IsGenericType: true}, threadSafe);
        _isEnumLazy = new Lazy<bool>(() => type is {IsEnum: true}, threadSafe);

        _isNullable = new Lazy<bool>(() =>
        {
            if (type == null)
                return false;

            return Nullable.GetUnderlyingType(type) != null;
        }, threadSafe);

        if (Type == null)
            return;

        _cachedProperties = new CachedProperties(this, threadSafe);
        _cachedMethods = new CachedMethods(this, threadSafe);
        _cachedAttributes = new CachedCustomAttributes(this, threadSafe);
        _cachedInterfaces = new CachedInterfaces(this, threadSafe);
        _cachedConstructors = new CachedConstructors(this, threadSafe);
        _cachedMembers = new CachedMembers(this, threadSafe);
        _cachedGenericArguments = new CachedGenericArguments(this, threadSafe);
        _cachedGenericTypeDefinition = new CachedGenericTypeDefinition(this, threadSafe);
        _cachedIsAssignableFrom = new CachedIsAssignableFrom(this, threadSafe);
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

        return _cachedAttributes!.GetCachedCustomAttributes();
    }

    public object[]? GetCustomAttributes()
    {
        if (Type == null)
            return null;

        return _cachedAttributes!.GetCustomAttributes();
    }

    public CachedConstructor? GetCachedConstructor(Type[] parameterTypes)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.GetCachedConstructor(parameterTypes);
    }

    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.GetConstructor(parameterTypes);
    }

    public CachedConstructor[]? GetCachedConstructors()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.GetCachedConstructors();
    }

    public ConstructorInfo?[]? GetConstructors()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.GetConstructors();
    }

    public object? CreateInstance()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.CreateInstance();
    }

    public object? CreateInstance(params object[] parameters)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.CreateInstance(parameters);
    }

    public T? CreateInstance<T>(params object[] parameters)
    {
        if (Type == null)
            return default;

        return _cachedConstructors!.CreateInstance<T>(parameters);
    }

    public CachedType? GetCachedGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        return _cachedGenericTypeDefinition!.GetCachedGenericTypeDefinition();
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

        return _cachedGenericArguments!.GetCachedGenericArguments();
    }

    public Type[]? GetGenericArguments()
    {
        if (Type == null)
            return null;

        return _cachedGenericArguments!.GetGenericArguments();
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

        return _cachedIsAssignableFrom!.IsAssignableFrom(derivedType);
    }
}