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
    /// <summary>
    /// Gets type.
    /// </summary>
    public Type? Type { get; }

    /// <summary>
    /// Gets or sets base type.
    /// </summary>
    public Type? BaseType => Type?.BaseType;

    /// <summary>
    /// Gets or sets cached base type.
    /// </summary>
    public CachedType? CachedBaseType => _cachedBaseTypeLazy.Value;

    private readonly Lazy<CachedType?> _cachedBaseTypeLazy;

    /// <summary>
    /// Gets or sets cache key.
    /// </summary>
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

    /// <summary>
    /// Gets property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>The result of the operation.</returns>
    public PropertyInfo? GetProperty(string property)
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetProperty(property);
    }

    /// <summary>
    /// Gets cached property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>The result of the operation.</returns>
    public CachedProperty? GetCachedProperty(string property)
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetCachedProperty(property);
    }

    /// <summary>
    /// Gets properties.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public PropertyInfo[]? GetProperties()
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetProperties();
    }

    /// <summary>
    /// Gets cached properties.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedProperty[]? GetCachedProperties()
    {
        if (Type == null)
            return null;

        return _cachedProperties!.Value.GetCachedProperties();
    }

    /// <summary>
    /// Gets cached method.
    /// </summary>
    /// <param name="methodName">The method name.</param>
    /// <returns>The result of the operation.</returns>
    public CachedMethod? GetCachedMethod(string methodName)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethod(methodName);
    }

    /// <summary>
    /// Gets cached method.
    /// </summary>
    /// <param name="methodName">The method name.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The result of the operation.</returns>
    public CachedMethod? GetCachedMethod(string methodName, Type[] parameters)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethod(methodName, parameters);
    }

    /// <summary>
    /// Gets cached method.
    /// </summary>
    /// <param name="methodName">The method name.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The result of the operation.</returns>
    public CachedMethod? GetCachedMethod(string methodName, CachedType[] parameters)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethod(methodName, parameters);
    }

    /// <summary>
    /// Gets cached fields.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedField[]? GetCachedFields()
    {
        if (Type == null)
            return null;

        CachedField[] result = _cachedFields!.Value.GetCachedFields();

        return result;
    }

    /// <summary>
    /// Gets fields.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public FieldInfo[]? GetFields()
    {
        if (Type == null)
            return null;

        FieldInfo[] result = _cachedFields!.Value.GetFields();
        
        return result;
    }

    /// <summary>
    /// Gets cached field.
    /// </summary>
    /// <param name="fieldName">The field name.</param>
    /// <returns>The result of the operation.</returns>
    public CachedField? GetCachedField(string fieldName)
    {
        if (Type == null)
            return null;

        return _cachedFields!.Value.GetCachedField(fieldName);
    }

    /// <summary>
    /// Gets field.
    /// </summary>
    /// <param name="fieldName">The field name.</param>
    /// <returns>The result of the operation.</returns>
    public FieldInfo? GetField(string fieldName)
    {
        if (Type == null)
            return null;

        return _cachedFields!.Value.GetField(fieldName);
    }

    /// <summary>
    /// Gets method.
    /// </summary>
    /// <param name="methodName">The method name.</param>
    /// <returns>The result of the operation.</returns>
    public MethodInfo? GetMethod(string methodName)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetMethod(methodName);
    }

    /// <summary>
    /// Gets method.
    /// </summary>
    /// <param name="methodName">The method name.</param>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>The result of the operation.</returns>
    public MethodInfo? GetMethod(string methodName, Type[] parameterTypes)
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetMethod(methodName, parameterTypes);
    }

    /// <summary>
    /// Gets cached methods.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedMethod[]? GetCachedMethods()
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetCachedMethods();
    }

    /// <summary>
    /// Gets methods.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public MethodInfo?[]? GetMethods()
    {
        if (Type == null)
            return null;

        return _cachedMethods!.Value.GetMethods();
    }

    /// <summary>
    /// Gets cached interface.
    /// </summary>
    /// <param name="typeName">The type name.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? GetCachedInterface(string typeName)
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.Value.GetCachedInterface(typeName);
    }

    /// <summary>
    /// Gets cached interfaces.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedType[]? GetCachedInterfaces()
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.Value.GetCachedInterfaces();
    }

    /// <summary>
    /// Gets interface.
    /// </summary>
    /// <param name="typeName">The type name.</param>
    /// <returns>The result of the operation.</returns>
    public Type? GetInterface(string typeName)
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.Value.GetInterface(typeName);
    }

    /// <summary>
    /// Gets interfaces.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public Type[]? GetInterfaces()
    {
        if (Type == null)
            return null;

        return _cachedInterfaces!.Value.GetInterfaces();
    }

    /// <summary>
    /// Gets cached custom attributes.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedAttribute[]? GetCachedCustomAttributes()
    {
        if (Type == null)
            return null;

        return _cachedAttributes!.Value.GetCachedCustomAttributes();
    }

    /// <summary>
    /// Gets custom attributes.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public object[]? GetCustomAttributes()
    {
        if (Type == null)
            return null;

        return _cachedAttributes!.Value.GetCustomAttributes();
    }

    /// <summary>
    /// Gets cached custom attribute.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="inherit">The inherit.</param>
    /// <returns>The result of the operation.</returns>
    public T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute
    {
        if (Type == null)
            return null;

        return _cachedAttributes!.Value.GetCachedCustomAttribute<T>(inherit);
    }

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>The result of the operation.</returns>
    public CachedConstructor? GetCachedConstructor(Type[] parameterTypes)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetCachedConstructor(parameterTypes);
    }

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    public CachedConstructor? GetCachedConstructor(Type t0)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetCachedConstructor(t0);
    }

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetCachedConstructor(t0, t1);
    }

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetCachedConstructor(t0, t1, t2);
    }

    /// <summary>
    /// Gets cached constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2, Type t3)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetCachedConstructor(t0, t1, t2, t3);
    }

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>The result of the operation.</returns>
    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetConstructor(parameterTypes);
    }

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    public ConstructorInfo? GetConstructor(Type t0)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetConstructor(t0);
    }

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    public ConstructorInfo? GetConstructor(Type t0, Type t1)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetConstructor(t0, t1);
    }

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    public ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetConstructor(t0, t1, t2);
    }

    /// <summary>
    /// Gets constructor.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    public ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2, Type t3)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetConstructor(t0, t1, t2, t3);
    }

    /// <summary>
    /// Gets cached constructors.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedConstructor[]? GetCachedConstructors()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetCachedConstructors();
    }

    /// <summary>
    /// Gets constructors.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public ConstructorInfo?[]? GetConstructors()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.GetConstructors();
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public object? CreateInstance()
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.CreateInstance();
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <returns>The result of the operation.</returns>
    public T? CreateInstance<T>()
    {
        if (Type == null)
            return default;

        return _cachedConstructors!.Value.CreateInstance<T>();
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The result of the operation.</returns>
    public object? CreateInstance(params object[] parameters)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.CreateInstance(parameters);
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The result of the operation.</returns>
    public T? CreateInstance<T>(params object[] parameters)
    {
        if (Type == null)
            return default;

        return _cachedConstructors!.Value.CreateInstance<T>(parameters);
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <returns>The result of the operation.</returns>
    public object? CreateInstance(object? arg0)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.CreateInstance(arg0);
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The result of the operation.</returns>
    public object? CreateInstance(object? arg0, object? arg1)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.CreateInstance(arg0, arg1);
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns>The result of the operation.</returns>
    public object? CreateInstance(object? arg0, object? arg1, object? arg2)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.CreateInstance(arg0, arg1, arg2);
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns>The result of the operation.</returns>
    public object? CreateInstance(object? arg0, object? arg1, object? arg2, object? arg3)
    {
        if (Type == null)
            return null;

        return _cachedConstructors!.Value.CreateInstance(arg0, arg1, arg2, arg3);
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <returns>The result of the operation.</returns>
    public T? CreateInstance<T>(object? arg0)
    {
        object? obj = CreateInstance(arg0);
        return obj is null ? default : (T?) obj;
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The result of the operation.</returns>
    public T? CreateInstance<T>(object? arg0, object? arg1)
    {
        object? obj = CreateInstance(arg0, arg1);
        return obj is null ? default : (T?) obj;
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns>The result of the operation.</returns>
    public T? CreateInstance<T>(object? arg0, object? arg1, object? arg2)
    {
        object? obj = CreateInstance(arg0, arg1, arg2);
        return obj is null ? default : (T?) obj;
    }

    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="arg0">The arg0.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns>The result of the operation.</returns>
    public T? CreateInstance<T>(object? arg0, object? arg1, object? arg2, object? arg3)
    {
        object? obj = CreateInstance(arg0, arg1, arg2, arg3);
        return obj is null ? default : (T?) obj;
    }

    /// <summary>
    /// Gets cached generic type definition.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedType? GetCachedGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        return _cachedGenericTypeDefinition!.Value.GetCachedGenericTypeDefinition();
    }

    /// <summary>
    /// Gets generic type definition.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public Type? GetGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        return _cachedGenericTypeDefinition!.Value.GetGenericTypeDefinition();
    }

    /// <summary>
    /// Gets cached generic arguments.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedType[]? GetCachedGenericArguments()
    {
        if (Type == null)
            return null;

        return _cachedGenericArguments!.Value.GetCachedGenericArguments();
    }

    /// <summary>
    /// Gets generic arguments.
    /// </summary>
    /// <returns>The result of the operation.</returns>
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

    /// <summary>
    /// Gets cached members.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedMember[]? GetCachedMembers()
    {
        if (Type == null)
            return null;

        return _cachedMembers!.Value.GetCachedMembers();
    }

    /// <summary>
    /// Gets members.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public MemberInfo[]? GetMembers()
    {
        if (Type == null)
            return null;

        return _cachedMembers!.Value.GetMembers();
    }

    /// <summary>
    /// Executes the is assignable from operation.
    /// </summary>
    /// <param name="derivedType">The derived type.</param>
    /// <returns>A value indicating whether the operation succeeded.</returns>
    public bool IsAssignableFrom(Type derivedType)
    {
        if (Type == null)
            return false;

        return _cachedIsAssignableFrom!.Value.IsAssignableFrom(derivedType);
    }

    /// <summary>
    /// Executes the is assignable from operation.
    /// </summary>
    /// <param name="cachedDerivedType">The cached derived type.</param>
    /// <returns>A value indicating whether the operation succeeded.</returns>
    public bool IsAssignableFrom(CachedType cachedDerivedType)
    {
        if (Type == null)
            return false;

        return _cachedIsAssignableFrom!.Value.IsAssignableFrom(cachedDerivedType);
    }

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="typeArguments">The type arguments.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(params Type[] typeArguments)
    {
        return _cachedMakeGenericType!.Value.MakeGenericCachedType(typeArguments);
    }

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="typeArguments">The type arguments.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(params CachedType[] typeArguments)
    {
        return _cachedMakeGenericType!.Value.MakeGenericCachedType(typeArguments);
    }

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(Type t0) => _cachedMakeGenericType!.Value.MakeGenericCachedType(t0);

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(Type t0, Type t1) => _cachedMakeGenericType!.Value.MakeGenericCachedType(t0, t1);

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(Type t0, Type t1, Type t2) => _cachedMakeGenericType!.Value.MakeGenericCachedType(t0, t1, t2);

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(Type t0, Type t1, Type t2, Type t3) => _cachedMakeGenericType!.Value.MakeGenericCachedType(t0, t1, t2, t3);

    // ---- allocation-reducing overloads (avoid params CachedType[] allocations) ----

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(CachedType t0)
    {
        return _cachedMakeGenericType!.Value.MakeGenericCachedType(t0);
    }

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(CachedType t0, CachedType t1)
    {
        return _cachedMakeGenericType!.Value.MakeGenericCachedType(t0, t1);
    }

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(CachedType t0, CachedType t1, CachedType t2)
    {
        return _cachedMakeGenericType!.Value.MakeGenericCachedType(t0, t1, t2);
    }

    /// <summary>
    /// Executes the make cached generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    public CachedType? MakeCachedGenericType(CachedType t0, CachedType t1, CachedType t2, CachedType t3)
    {
        return _cachedMakeGenericType!.Value.MakeGenericCachedType(t0, t1, t2, t3);
    }

    /// <summary>
    /// Executes the make generic type operation.
    /// </summary>
    /// <param name="typeArguments">The type arguments.</param>
    /// <returns>The result of the operation.</returns>
    public Type? MakeGenericType(params Type[] typeArguments)
    {
        return _cachedMakeGenericType!.Value.MakeGenericType(typeArguments);
    }

    // ---- allocation-reducing overloads (avoid params Type[] allocations) ----

    /// <summary>
    /// Executes the make generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <returns>The result of the operation.</returns>
    public Type? MakeGenericType(Type t0) => _cachedMakeGenericType!.Value.MakeGenericType(t0);

    /// <summary>
    /// Executes the make generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <returns>The result of the operation.</returns>
    public Type? MakeGenericType(Type t0, Type t1) => _cachedMakeGenericType!.Value.MakeGenericType(t0, t1);

    /// <summary>
    /// Executes the make generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <returns>The result of the operation.</returns>
    public Type? MakeGenericType(Type t0, Type t1, Type t2) => _cachedMakeGenericType!.Value.MakeGenericType(t0, t1, t2);

    /// <summary>
    /// Executes the make generic type operation.
    /// </summary>
    /// <param name="t0">The t0.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <returns>The result of the operation.</returns>
    public Type? MakeGenericType(Type t0, Type t1, Type t2, Type t3) => _cachedMakeGenericType!.Value.MakeGenericType(t0, t1, t2, t3);

    /// <summary>
    /// Gets cached element type.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public CachedType? GetCachedElementType()
    {
        return _cachedGetElementType!.Value.GetCachedElementType();
    }

    /// <summary>
    /// Gets element type.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public Type? GetElementType()
    {
        return _cachedGetElementType!.Value.GetElementType();
    }

    /// <summary>
    /// Returns a string representation of the current instance.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public override string ToString()
    {
        return Type == null ? "null" : Type.Name;
    }
}