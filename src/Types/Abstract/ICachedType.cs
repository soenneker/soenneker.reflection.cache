using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Members;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Properties;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Interface for CachedType providing reflection-related functionalities.
/// </summary>
public interface ICachedType
{
    /// <summary>
    /// Gets the Type associated with this CachedType. <para/>
    /// This can be null because we still cache the "type" even if it is not found! This is because there might be lookups later that we don't want to use reflection for. <para/>
    /// </summary>
    [Pure]
    Type? Type { get; }

    /// <summary>
    /// Is a hash code of <see cref="Type"/>. If <see cref="Type"/> is null, this is null.
    /// </summary>
    [Pure]
    int? CacheKey { get; }

    [Pure]
    CachedProperty? GetCachedProperty(string property);

    /// <summary>
    /// Gets information about a specific property.
    /// </summary>
    /// <param name="property">The name of the property.</param>
    /// <returns>Information about the specified property.</returns>
    [Pure]
    PropertyInfo? GetProperty(string property);

    [Pure]
    CachedProperty[]? GetCachedProperties();

    /// <summary>
    /// Gets information about all properties.
    /// </summary>
    /// <returns>An array of PropertyInfo objects representing all properties.</returns>
    [Pure]
    PropertyInfo[]? GetProperties();

    [Pure]
    CachedField[]? GetCachedFields();

    [Pure]
    FieldInfo[]? GetFields();

    [Pure]
    CachedField? GetCachedField(string fieldName);

    [Pure]
    FieldInfo? GetField(string fieldName);


    /// <summary>
    /// Gets information about a specific method.
    /// </summary>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>Information about the specified method.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string methodName);

    [Pure]
    CachedMethod? GetCachedMethod(string methodName, Type[] parameters);

    [Pure]
    CachedMethod? GetCachedMethod(string methodName, CachedType[] parameters);

    /// <summary>
    /// Gets information about all methods.
    /// </summary>
    /// <returns>An array of CachedMethod objects representing all methods.</returns>
    [Pure]
    CachedMethod[]? GetCachedMethods();

    /// <summary>
    /// Gets information about a specific method using reflection.
    /// </summary>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>MethodInfo object representing the specified method.</returns>
    [Pure]
    MethodInfo? GetMethod(string methodName);

    [Pure]
    MethodInfo? GetMethod(string methodName, Type[] parameterTypes);

    /// <summary>
    /// Gets information about all methods using reflection.
    /// </summary>
    /// <returns>An array of MethodInfo objects representing all methods.</returns>
    [Pure]
    MethodInfo?[]? GetMethods();

    [Pure]
    CachedType? GetCachedInterface(string typeName);

    [Pure]
    Type? GetInterface(string typeName);

    /// <summary>
    /// Gets information about cached interfaces.
    /// </summary>
    /// <returns>An array of CachedType objects representing cached interfaces.</returns>
    [Pure]
    CachedType[]? GetCachedInterfaces();

    /// <summary>
    /// Gets information about interfaces using reflection.
    /// </summary>
    [Pure]
    Type[]? GetInterfaces();

    /// <summary>
    /// Gets information about cached custom attributes.
    /// </summary>
    /// <returns>An array of CachedAttribute objects representing cached custom attributes.</returns>
    [Pure]
    CachedAttribute[]? GetCachedCustomAttributes();

    /// <summary>
    /// Gets information about custom attributes using reflection.
    /// </summary>
    /// <returns>An array of object representing custom attributes.</returns>
    [Pure]
    object[]? GetCustomAttributes();

    /// <summary>
    /// Gets information about a specific constructor.
    /// </summary>
    /// <param name="parameterTypes">The parameter types of the constructor.</param>
    /// <returns>Information about the specified constructor.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type[] parameterTypes);

    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0);

    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1);

    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2);

    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets information about all constructors.
    /// </summary>
    /// <returns>An array of CachedConstructor objects representing all constructors.</returns>
    [Pure]
    CachedConstructor[]? GetCachedConstructors();

    /// <summary>
    /// Gets information about a specific constructor using reflection.
    /// </summary>
    /// <param name="parameterTypes">The parameter types of the constructor.</param>
    /// <returns>ConstructorInfo object representing the specified constructor.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type[]? parameterTypes = null);

    [Pure]
    ConstructorInfo? GetConstructor(Type t0);

    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1);

    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2);

    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets information about all constructors using reflection.
    /// </summary>
    /// <returns>An array of ConstructorInfo objects representing all constructors.</returns>
    [Pure]
    ConstructorInfo?[]? GetConstructors();

    /// <summary>
    /// Creates an instance of the type.
    /// </summary>
    /// <returns>An instance of the type.</returns>
    [Pure]
    object? CreateInstance();

    /// <summary>
    /// Creates an instance of the type.
    /// </summary>
    /// <returns>An instance of the type.</returns>
    [Pure]
    T? CreateInstance<T>();

    /// <summary>
    /// Creates an instance of the type with specified parameters.
    /// </summary>
    /// <param name="parameters">Parameters for the constructor.</param>
    /// <returns>An instance of the type.</returns>
    [Pure]
    object? CreateInstance(params object[] parameters);

    /// <summary>
    /// Creates an instance of the generic type with specified parameters.
    /// </summary>
    /// <typeparam name="T">The type of the instance to create.</typeparam>
    /// <param name="parameters">Parameters for the constructor.</param>
    /// <returns>An instance of the generic type.</returns>
    [Pure]
    T? CreateInstance<T>(params object[] parameters);

    [Pure]
    object? CreateInstance(object? arg0);

    [Pure]
    object? CreateInstance(object? arg0, object? arg1);

    [Pure]
    object? CreateInstance(object? arg0, object? arg1, object? arg2);

    [Pure]
    object? CreateInstance(object? arg0, object? arg1, object? arg2, object? arg3);

    [Pure]
    T? CreateInstance<T>(object? arg0);

    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1);

    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1, object? arg2);

    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1, object? arg2, object? arg3);

    /// <summary>
    /// Gets information about the cached generic type definition.
    /// </summary>
    /// <returns>Information about the cached generic type definition.</returns>
    [Pure]
    CachedType? GetCachedGenericTypeDefinition();

    /// <summary>
    /// Gets information about the generic type definition using reflection.
    /// </summary>
    /// <returns>Type object representing the generic type definition.</returns>
    [Pure]
    Type? GetGenericTypeDefinition();

    /// <summary>
    /// Gets information about the cached generic arguments.
    /// </summary>
    /// <returns>An array of CachedType objects representing cached generic arguments.</returns>
    [Pure]
    CachedType[]? GetCachedGenericArguments();

    /// <summary>
    /// Gets information about the generic arguments using reflection.
    /// </summary>
    /// <returns>An array of Type objects representing generic arguments.</returns>
    [Pure]
    Type[]? GetGenericArguments();

    //[Pure]
    //CachedMember? GetCachedMember(string name);

    ///// <summary>
    ///// Gets information about a specific member.
    ///// </summary>
    ///// <param name="name">The name of the member.</param>
    ///// <returns>Information about the specified member.</returns>
    //[Pure]
    //MemberInfo? GetMember(string name);

    [Pure]
    CachedMember[]? GetCachedMembers();

    /// <summary>
    /// Gets information about all members.
    /// </summary>
    /// <returns>An array of MemberInfo objects representing all members.</returns>
    [Pure]
    MemberInfo[]? GetMembers();

    [Pure]
    bool IsAssignableFrom(Type derivedType);

    [Pure]
    bool IsAssignableFrom(CachedType cachedDerivedType);

    /// <summary>
    /// This is going to be the fastest of all the MakeGenericType methods
    /// </summary>
    /// <param name="typeArguments"></param>
    /// <returns></returns>
    [Pure]
    CachedType? MakeCachedGenericType(params Type[] typeArguments);

    [Pure]
    CachedType? MakeCachedGenericType(params CachedType[] typeArguments);

    [Pure]
    CachedType? MakeCachedGenericType(CachedType t0);

    [Pure]
    CachedType? MakeCachedGenericType(CachedType t0, CachedType t1);

    [Pure]
    CachedType? MakeCachedGenericType(CachedType t0, CachedType t1, CachedType t2);

    [Pure]
    CachedType? MakeCachedGenericType(CachedType t0, CachedType t1, CachedType t2, CachedType t3);

    [Pure]
    Type? MakeGenericType(params Type[] typeArguments);

    [Pure]
    CachedType? GetCachedElementType();

    [Pure]
    Type? GetElementType();
}