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
public partial interface ICachedType
{
    /// <summary>
    /// Gets all cached fields for this type.
    /// </summary>
    /// <returns>An array of cached fields, or null.</returns>
    [Pure]
    CachedField[]? GetCachedFields();

    /// <summary>
    /// Gets information about all fields using reflection.
    /// </summary>
    /// <returns>An array of FieldInfo objects representing all fields.</returns>
    [Pure]
    FieldInfo[]? GetFields();

    /// <summary>
    /// Gets the cached field with the specified name.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    /// <returns>The cached field, or null if not found.</returns>
    [Pure]
    CachedField? GetCachedField(string fieldName);

    /// <summary>
    /// Gets information about a specific field using reflection.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    /// <returns>FieldInfo for the specified field, or null if not found.</returns>
    [Pure]
    FieldInfo? GetField(string fieldName);


    /// <summary>
    /// Gets the cached method with the specified name.
    /// </summary>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>The cached method, or null if not found.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string methodName);

    /// <summary>
    /// Gets the cached method with the specified name and parameter types.
    /// </summary>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="parameters">The parameter types of the method.</param>
    /// <returns>The cached method, or null if not found.</returns>
    [Pure]
    CachedMethod? GetCachedMethod(string methodName, Type[] parameters);

    /// <summary>
    /// Gets the cached method with the specified name and cached parameter types.
    /// </summary>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="parameters">The cached parameter types of the method.</param>
    /// <returns>The cached method, or null if not found.</returns>
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

    /// <summary>
    /// Gets information about a specific method with the given parameter types using reflection.
    /// </summary>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="parameterTypes">The parameter types of the method.</param>
    /// <returns>MethodInfo for the specified method, or null if not found.</returns>
    [Pure]
    MethodInfo? GetMethod(string methodName, Type[] parameterTypes);

    /// <summary>
    /// Gets information about all methods using reflection.
    /// </summary>
    /// <returns>An array of MethodInfo objects representing all methods.</returns>
    [Pure]
    MethodInfo?[]? GetMethods();

    /// <summary>
    /// Gets the cached interface with the specified name.
    /// </summary>
    /// <param name="typeName">The name of the interface.</param>
    /// <returns>The cached interface type, or null if not found.</returns>
    [Pure]
    CachedType? GetCachedInterface(string typeName);

    /// <summary>
    /// Gets the interface with the specified name using reflection.
    /// </summary>
    /// <param name="typeName">The name of the interface.</param>
    /// <returns>Type for the specified interface, or null if not found.</returns>
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
    /// Gets the cached constructor with the specified parameter types.
    /// </summary>
    /// <param name="parameterTypes">The parameter types of the constructor.</param>
    /// <returns>The cached constructor, or null if not found.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type[] parameterTypes);

    /// <summary>
    /// Gets the cached constructor with a single parameter type.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <returns>The cached constructor, or null if not found.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0);

    /// <summary>
    /// Gets the cached constructor with two parameter types.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <returns>The cached constructor, or null if not found.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1);

    /// <summary>
    /// Gets the cached constructor with three parameter types.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <returns>The cached constructor, or null if not found.</returns>
    [Pure]
    CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets the cached constructor with four parameter types.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <param name="t3">The fourth parameter type.</param>
    /// <returns>The cached constructor, or null if not found.</returns>
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

    /// <summary>
    /// Gets the constructor with a single parameter type using reflection.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <returns>ConstructorInfo for the constructor, or null if not found.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0);

    /// <summary>
    /// Gets the constructor with two parameter types using reflection.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <returns>ConstructorInfo for the constructor, or null if not found.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1);

    /// <summary>
    /// Gets the constructor with three parameter types using reflection.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <returns>ConstructorInfo for the constructor, or null if not found.</returns>
    [Pure]
    ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2);

    /// <summary>
    /// Gets the constructor with four parameter types using reflection.
    /// </summary>
    /// <param name="t0">The first parameter type.</param>
    /// <param name="t1">The second parameter type.</param>
    /// <param name="t2">The third parameter type.</param>
    /// <param name="t3">The fourth parameter type.</param>
    /// <returns>ConstructorInfo for the constructor, or null if not found.</returns>
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

    /// <summary>
    /// Creates an instance of the type using the constructor that takes one argument.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <returns>An instance of the type, or null.</returns>
    [Pure]
    object? CreateInstance(object? arg0);

    /// <summary>
    /// Creates an instance of the type using the constructor that takes two arguments.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <returns>An instance of the type, or null.</returns>
    [Pure]
    object? CreateInstance(object? arg0, object? arg1);

    /// <summary>
    /// Creates an instance of the type using the constructor that takes three arguments.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <returns>An instance of the type, or null.</returns>
    [Pure]
    object? CreateInstance(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Creates an instance of the type using the constructor that takes four arguments.
    /// </summary>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <param name="arg3">The fourth constructor argument.</param>
    /// <returns>An instance of the type, or null.</returns>
    [Pure]
    object? CreateInstance(object? arg0, object? arg1, object? arg2, object? arg3);

    /// <summary>
    /// Creates an instance of the type as T using the constructor that takes one argument.
    /// </summary>
    /// <typeparam name="T">The type to cast the result to.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <returns>An instance of the type as T, or null.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0);

    /// <summary>
    /// Creates an instance of the type as T using the constructor that takes two arguments.
    /// </summary>
    /// <typeparam name="T">The type to cast the result to.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <returns>An instance of the type as T, or null.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1);

    /// <summary>
    /// Creates an instance of the type as T using the constructor that takes three arguments.
    /// </summary>
    /// <typeparam name="T">The type to cast the result to.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <returns>An instance of the type as T, or null.</returns>
    [Pure]
    T? CreateInstance<T>(object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Creates an instance of the type as T using the constructor that takes four arguments.
    /// </summary>
    /// <typeparam name="T">The type to cast the result to.</typeparam>
    /// <param name="arg0">The first constructor argument.</param>
    /// <param name="arg1">The second constructor argument.</param>
    /// <param name="arg2">The third constructor argument.</param>
    /// <param name="arg3">The fourth constructor argument.</param>
    /// <returns>An instance of the type as T, or null.</returns>
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

    /// <summary>
    /// Gets all cached members (fields, properties, methods, etc.) for this type.
    /// </summary>
    /// <returns>An array of cached members, or null.</returns>
    [Pure]
    CachedMember[]? GetCachedMembers();

    /// <summary>
    /// Gets information about all members.
    /// </summary>
    /// <returns>An array of MemberInfo objects representing all members.</returns>
    [Pure]
    MemberInfo[]? GetMembers();

    /// <summary>
    /// Determines whether an instance of the specified type can be assigned to an instance of this type.
    /// </summary>
    /// <param name="derivedType">The type to check.</param>
    /// <returns>True if instances of the specified type can be assigned to this type; otherwise, false.</returns>
    [Pure]
    bool IsAssignableFrom(Type derivedType);

    /// <summary>
    /// Determines whether an instance of the specified cached type can be assigned to an instance of this type.
    /// </summary>
    /// <param name="cachedDerivedType">The cached type to check.</param>
    /// <returns>True if instances of the specified type can be assigned to this type; otherwise, false.</returns>
    [Pure]
    bool IsAssignableFrom(CachedType cachedDerivedType);

    /// <summary>
    /// Constructs a cached generic type from the current generic type definition with the specified type arguments. This is the fastest of all the MakeGenericType methods.
    /// </summary>
    /// <param name="typeArguments">The type arguments for the generic type.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(params Type[] typeArguments);

    /// <summary>
    /// Constructs a cached generic type from the current generic type definition with the specified cached type arguments.
    /// </summary>
    /// <param name="typeArguments">The cached type arguments for the generic type.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(params CachedType[] typeArguments);

    /// <summary>
    /// Constructs a cached generic type with one type argument.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(CachedType t0);

    /// <summary>
    /// Constructs a cached generic type with two type arguments.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(CachedType t0, CachedType t1);

    /// <summary>
    /// Constructs a cached generic type with three type arguments.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <param name="t2">The third type argument.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(CachedType t0, CachedType t1, CachedType t2);

    /// <summary>
    /// Constructs a cached generic type with four type arguments.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <param name="t2">The third type argument.</param>
    /// <param name="t3">The fourth type argument.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(CachedType t0, CachedType t1, CachedType t2, CachedType t3);

    /// <summary>
    /// Constructs a cached generic type with one type argument.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(Type t0);

    /// <summary>
    /// Constructs a cached generic type with two type arguments.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(Type t0, Type t1);

    /// <summary>
    /// Constructs a cached generic type with three type arguments.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <param name="t2">The third type argument.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(Type t0, Type t1, Type t2);

    /// <summary>
    /// Constructs a cached generic type with four type arguments.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <param name="t2">The third type argument.</param>
    /// <param name="t3">The fourth type argument.</param>
    /// <returns>The constructed cached generic type, or null.</returns>
    [Pure]
    CachedType? MakeCachedGenericType(Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Substitutes the type parameters of the generic type definition with the specified type arguments and returns a Type representing the resulting constructed type.
    /// </summary>
    /// <param name="typeArguments">The type arguments for the generic type.</param>
    /// <returns>The constructed Type, or null.</returns>
    [Pure]
    Type? MakeGenericType(params Type[] typeArguments);

    /// <summary>
    /// Substitutes the type parameters with one type argument and returns the resulting constructed type.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <returns>The constructed Type, or null.</returns>
    [Pure]
    Type? MakeGenericType(Type t0);

    /// <summary>
    /// Substitutes the type parameters with two type arguments and returns the resulting constructed type.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <returns>The constructed Type, or null.</returns>
    [Pure]
    Type? MakeGenericType(Type t0, Type t1);

    /// <summary>
    /// Substitutes the type parameters with three type arguments and returns the resulting constructed type.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <param name="t2">The third type argument.</param>
    /// <returns>The constructed Type, or null.</returns>
    [Pure]
    Type? MakeGenericType(Type t0, Type t1, Type t2);

    /// <summary>
    /// Substitutes the type parameters with four type arguments and returns the resulting constructed type.
    /// </summary>
    /// <param name="t0">The first type argument.</param>
    /// <param name="t1">The second type argument.</param>
    /// <param name="t2">The third type argument.</param>
    /// <param name="t3">The fourth type argument.</param>
    /// <returns>The constructed Type, or null.</returns>
    [Pure]
    Type? MakeGenericType(Type t0, Type t1, Type t2, Type t3);

    /// <summary>
    /// Gets the cached element type when this type represents an array, pointer, or by-ref type.
    /// </summary>
    /// <returns>The cached element type, or null.</returns>
    [Pure]
    CachedType? GetCachedElementType();

    /// <summary>
    /// Gets the Type of the element when this type represents an array, pointer, or by-ref type.
    /// </summary>
    /// <returns>The element Type, or null.</returns>
    [Pure]
    Type? GetElementType();
}