using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Arguments;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Interfaces;
using Soenneker.Reflection.Cache.Members;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types.Abstract;

namespace Soenneker.Reflection.Cache.Types;

///<inheritdoc cref="ICachedType"/>
public partial class CachedType : ICachedType
{
    public Type? Type { get; }

    public Type? BaseType => Type?.BaseType;

    public CachedType? CachedBaseType => _cachedBaseTypeLazy.Value;

    private readonly Lazy<CachedType?> _cachedBaseTypeLazy;

    public int? CacheKey => _cacheKeyLazy.Value;
    private readonly Lazy<int?> _cacheKeyLazy;

    private readonly Lazy<CachedProperties>? _cachedProperties;
    private readonly Lazy<CachedMethods>? _cachedMethods;
    private readonly Lazy<CachedFields>? _cachedFields;
    private readonly Lazy<CachedCustomAttributes>? _cachedAttributes;
    private readonly Lazy<CachedInterfaces>? _cachedInterfaces;
    private readonly Lazy<CachedConstructors>? _cachedConstructors;
    private readonly Lazy<CachedMembers>? _cachedMembers;
    private readonly Lazy<CachedGenericArguments>? _cachedGenericArguments;
    private readonly Lazy<CachedGenericTypeDefinition>? _cachedGenericTypeDefinition;
    private readonly Lazy<CachedIsAssignableFrom>? _cachedIsAssignableFrom;
    private readonly Lazy<CachedMakeGenericType>? _cachedMakeGenericType;
    private readonly Lazy<CachedGetElementType>? _cachedGetElementType;

    private readonly bool _threadSafe;
    private readonly CachedTypes _cachedTypes;

    public CachedType(Type? type, CachedTypes cachedTypes, bool threadSafe = true)
    {
        Type = type;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;

        _cacheKeyLazy = new Lazy<int?>(() => type?.GetHashCode(), threadSafe);

        InitializeProperties();

        if (Type == null)
            return;

        _cachedBaseTypeLazy = new Lazy<CachedType>(() => _cachedTypes.GetCachedType(Type.BaseType), _threadSafe);

        _cachedProperties = new Lazy<CachedProperties>(() => new CachedProperties(this, cachedTypes, threadSafe), threadSafe);
        _cachedMethods = new Lazy<CachedMethods>(() => new CachedMethods(this, cachedTypes, threadSafe), threadSafe);
        _cachedFields = new Lazy<CachedFields>(() => new CachedFields(this, cachedTypes, threadSafe), threadSafe);
        _cachedAttributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, cachedTypes, threadSafe), threadSafe);
        _cachedInterfaces = new Lazy<CachedInterfaces>(() => new CachedInterfaces(this, cachedTypes, threadSafe), threadSafe);
        _cachedConstructors = new Lazy<CachedConstructors>(() => new CachedConstructors(this, cachedTypes, threadSafe), threadSafe);
        _cachedMembers = new Lazy<CachedMembers>(() => new CachedMembers(this, cachedTypes, threadSafe), threadSafe);
        _cachedGenericArguments = new Lazy<CachedGenericArguments>(() => new CachedGenericArguments(this, cachedTypes, threadSafe), threadSafe);
        _cachedGenericTypeDefinition = new Lazy<CachedGenericTypeDefinition>(() => new CachedGenericTypeDefinition(this, cachedTypes, threadSafe), threadSafe);
        _cachedIsAssignableFrom = new Lazy<CachedIsAssignableFrom>(() => new CachedIsAssignableFrom(this, threadSafe), threadSafe);
        _cachedMakeGenericType = new Lazy<CachedMakeGenericType>(() => new CachedMakeGenericType(this, cachedTypes, threadSafe), threadSafe);
        _cachedGetElementType = new Lazy<CachedGetElementType>(() => new CachedGetElementType(this, cachedTypes, threadSafe), threadSafe);
    }

    public PropertyInfo? GetProperty(string property)
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetProperty(property);
    }

    public CachedProperty? GetCachedProperty(string property)
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetCachedProperty(property);
    }

    public PropertyInfo[]? GetProperties()
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetProperties();
    }

    public CachedProperty[]? GetCachedProperties()
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetCachedProperties();
    }

    public CachedMethod? GetCachedMethod(string methodName)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethod(methodName);
    }

    public CachedMethod? GetCachedMethod(string methodName, Type[] parameters)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethod(methodName, parameters);
    }

    public CachedMethod? GetCachedMethod(string methodName, CachedType[] parameters)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethod(methodName, parameters);
    }

    public CachedField[]? GetCachedFields()
    {
        if (Type == null)
            return null;

        CachedField[] result = _cachedFields!.Value.GetCachedFields();

        return result;
    }

    public FieldInfo[]? GetFields()
    {
        if (Type == null)
            return null;

        FieldInfo[] result = _cachedFields!.Value.GetFields();
        
        return result;
    }

    public CachedField? GetCachedField(string fieldName)
    {
        if (Type == null)
            return null;

        return _cachedFields!.Value.GetCachedField(fieldName);
    }

    public FieldInfo? GetField(string fieldName)
    {
        if (Type == null)
            return null;

        return _cachedFields!.Value.GetField(fieldName);
    }

    public MethodInfo? GetMethod(string methodName)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetMethod(methodName);
    }

    public MethodInfo? GetMethod(string methodName, Type[] parameterTypes)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetMethod(methodName, parameterTypes);
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

    //public CachedMember? GetCachedMember(string name)
    //{
    //    if (Type == null)
    //        return null;

    //    return _cachedMembers!.Value.GetCachedMember(name);
    //}

    //public MemberInfo? GetMember(string name)
    //{
    //    if (Type == null)
    //        return null;

    //    return _cachedMembers!.Value.GetMember(name);
    //}

    public CachedMember[]? GetCachedMembers()
    {
        if (Type == null)
            return null;

        return _cachedMembers!.Value.GetCachedMembers();
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

    public bool IsAssignableFrom(CachedType cachedDerivedType)
    {
        if (Type == null)
            return false;

        return _cachedIsAssignableFrom!.Value.IsAssignableFrom(cachedDerivedType);
    }

    public CachedType? MakeCachedGenericType(params Type[] typeArguments)
    {
        return _cachedMakeGenericType!.Value.MakeGenericCachedType(typeArguments);
    }

    public CachedType? MakeCachedGenericType(params CachedType[] typeArguments)
    {
        return _cachedMakeGenericType!.Value.MakeGenericCachedType(typeArguments);
    }

    public Type? MakeGenericType(params Type[] typeArguments)
    {
        return _cachedMakeGenericType!.Value.MakeGenericType(typeArguments);
    }

    public CachedType? GetCachedElementType()
    {
        return _cachedGetElementType!.Value.GetCachedElementType();
    }

    public Type? GetElementType()
    {
        return _cachedGetElementType!.Value.GetElementType();
    }

    public override string ToString()
    {
        return Type == null ? "null" : Type.Name;
    }
}