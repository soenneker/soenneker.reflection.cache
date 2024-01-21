using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Methods;

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
    int? GetCacheKey();

    /// <summary>
    /// Gets information about a specific property.
    /// </summary>
    /// <param name="property">The name of the property.</param>
    /// <returns>Information about the specified property.</returns>
    [Pure]
    PropertyInfo? GetProperty(string property);

    /// <summary>
    /// Gets information about all properties.
    /// </summary>
    /// <returns>An array of PropertyInfo objects representing all properties.</returns>
    [Pure]
    PropertyInfo[]? GetProperties();

    /// <summary>
    /// Gets information about a specific method.
    /// </summary>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>Information about the specified method.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string methodName);

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
    /// <returns>An IEnumerable of Type objects representing interfaces.</returns>
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

    /// <summary>
    /// Gets information about a specific member.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <returns>Information about the specified member.</returns>
    [Pure]
    MemberInfo? GetMember(string name);

    /// <summary>
    /// Gets information about all members.
    /// </summary>
    /// <returns>An array of MemberInfo objects representing all members.</returns>
    [Pure]
    MemberInfo[]? GetMembers();

    [Pure]
    bool IsAssignableFrom(Type derivedType);
}