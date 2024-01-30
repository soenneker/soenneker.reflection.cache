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

    public bool IsByRef => _isByRef.Value;
    private readonly Lazy<bool> _isByRef;

    public bool IsArray => _isArray.Value;
    private readonly Lazy<bool> _isArray;

    private readonly Lazy<CachedProperties>? _cachedProperties;
    private readonly Lazy<CachedMethods>? _cachedMethods;
    private readonly Lazy<CachedCustomAttributes>? _cachedAttributes;
    private readonly Lazy<CachedInterfaces>? _cachedInterfaces;
    private readonly Lazy<CachedConstructors>? _cachedConstructors;
    private readonly Lazy<CachedMembers>? _cachedMembers;
    private readonly Lazy<CachedGenericArguments>? _cachedGenericArguments;
    private readonly Lazy<CachedGenericTypeDefinition>? _cachedGenericTypeDefinition;
    private readonly Lazy<CachedIsAssignableFrom>? _cachedIsAssignableFrom;


    public CachedType(Type? type, CachedTypes cachedTypes, bool threadSafe = true)
    {
        Type = type;

        _cacheKeyLazy = new Lazy<int?>(() => type?.GetHashCode(), threadSafe);

        _isAbstractLazy = new Lazy<bool>(() => type is {IsAbstract: true}, threadSafe);
        _isInterfaceLazy = new Lazy<bool>(() => type is {IsInterface: true}, threadSafe);
        _isGenericTypeLazy = new Lazy<bool>(() => type is {IsGenericType: true}, threadSafe);
        _isEnumLazy = new Lazy<bool>(() => type is {IsEnum: true}, threadSafe);

        _isNullable = new Lazy<bool>(() =>
        {
            if (type == null)
                return false;

            return Nullable.GetUnderlyingType(type) != null;
        }, threadSafe);

        _isByRef = new Lazy<bool>(() => type is {IsByRef: true}, threadSafe);
        _isArray = new Lazy<bool>(() => type is {IsArray: true}, threadSafe);

        if (Type == null)
            return;

        _cachedProperties = new Lazy<CachedProperties>(() => new CachedProperties(this, threadSafe), threadSafe);
        _cachedMethods = new Lazy<CachedMethods>(() => new CachedMethods(this, threadSafe), threadSafe);
        _cachedAttributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, threadSafe), threadSafe);
        _cachedInterfaces = new Lazy<CachedInterfaces>(() => new CachedInterfaces(this, cachedTypes, threadSafe), threadSafe);
        _cachedConstructors = new Lazy<CachedConstructors>(() => new CachedConstructors(this, threadSafe), threadSafe);
        _cachedMembers = new Lazy<CachedMembers>(() => new CachedMembers(this, threadSafe), threadSafe);
        _cachedGenericArguments = new Lazy<CachedGenericArguments>(() => new CachedGenericArguments(this, cachedTypes, threadSafe), threadSafe);
        _cachedGenericTypeDefinition = new Lazy<CachedGenericTypeDefinition>(() => new CachedGenericTypeDefinition(this, cachedTypes, threadSafe), threadSafe);
        _cachedIsAssignableFrom = new Lazy<CachedIsAssignableFrom>(() => new CachedIsAssignableFrom(this, threadSafe), threadSafe);
    }


    public PropertyInfo? GetProperty(string property)
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetProperty(property);
    }


    public PropertyInfo[]? GetProperties()
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetProperties();
    }


    public CachedMethod? GetCachedMethod(string methodName)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethod(methodName);
    }

    public MethodInfo? GetMethod(string methodName)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetMethod(methodName);
    }

    public CachedMethod[]? GetCachedMethods()
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethods();
    }

    public MethodInfo?[]? GetMethods()
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetMethods();
    }

    public CachedType? GetCachedInterface(string typeName)
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.Value.GetCachedInterface(typeName);
    }

    public CachedType[]? GetCachedInterfaces()
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.Value.GetCachedInterfaces();
    }
    
    public Type? GetInterface(string typeName)
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.Value.GetInterface(typeName);
    }

    public Type[]? GetInterfaces()
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.Value.GetInterfaces();
    }

    public CachedAttribute[]? GetCachedCustomAttributes()
    {
        if (Type == null)
            return null;

        return _cachedAttributes!.Value.GetCachedCustomAttributes();
    }

    public object[]? GetCustomAttributes()
    {
        if (Type == null)
            return null;

        return _cachedAttributes!.Value.GetCustomAttributes();
    }

    public CachedConstructor? GetCachedConstructor(Type[] parameterTypes)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetCachedConstructor(parameterTypes);
    }

    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetConstructor(parameterTypes);
    }

    public CachedConstructor[]? GetCachedConstructors()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetCachedConstructors();
    }

    public ConstructorInfo?[]? GetConstructors()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetConstructors();
    }
    public object? CreateInstance()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.CreateInstance();
    }

    public T? CreateInstance<T>()
    {
        if (Type == null)
            return default;

        return _cachedConstructors!.Value.CreateInstance<T>();
    }

    public object? CreateInstance(params object[] parameters)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.CreateInstance(parameters);
    }

    public T? CreateInstance<T>(params object[] parameters)
    {
        if (Type == null)
            return default;

        return _cachedConstructors!.Value.CreateInstance<T>(parameters);
    }

    public CachedType? GetCachedGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        return _cachedGenericTypeDefinition!.Value.GetCachedGenericTypeDefinition();
    }

    public Type? GetGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        return _cachedGenericTypeDefinition!.Value.GetGenericTypeDefinition();
    }

    public CachedType[]? GetCachedGenericArguments()
    {
        if (Type == null)
            return null;

        return _cachedGenericArguments!.Value.GetCachedGenericArguments();
    }

    public Type[]? GetGenericArguments()
    {
        if (Type == null)
            return null;

        return _cachedGenericArguments!.Value.GetGenericArguments();
    }

    public MemberInfo? GetMember(string name)
    {
        if (Type == null)
            return null;

        return _cachedMembers!.Value.GetMember(name);
    }

    public MemberInfo[]? GetMembers()
    {
        if (Type == null)
            return null;

        return _cachedMembers!.Value.GetMembers();
    }

    public bool IsAssignableFrom(Type derivedType)
    {
        if (Type == null)
            return false;

        return _cachedIsAssignableFrom!.Value.IsAssignableFrom(derivedType);
    }

    public override string ToString()
    {
        return Type == null ? "null" : Type.Name;
    }
}