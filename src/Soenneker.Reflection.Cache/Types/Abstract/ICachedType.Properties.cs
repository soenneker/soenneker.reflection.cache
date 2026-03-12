using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Soenneker.Reflection.Cache.Properties;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Properties and property-related members of <see cref="ICachedType"/>.
/// </summary>
public partial interface ICachedType
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

    /// <summary>
    /// Gets whether the type is abstract.
    /// </summary>
    [Pure]
    bool IsAbstract { get; }

    /// <summary>
    /// Gets whether the type is an interface.
    /// </summary>
    [Pure]
    bool IsInterface { get; }

    /// <summary>
    /// Gets whether the type is a generic type.
    /// </summary>
    [Pure]
    bool IsGenericType { get; }

    /// <summary>
    /// Gets whether the type is an enum.
    /// </summary>
    [Pure]
    bool IsEnum { get; }

    /// <summary>
    /// Gets whether the type is passed by reference.
    /// </summary>
    [Pure]
    bool IsByRef { get; }

    /// <summary>
    /// Gets whether the type is an array.
    /// </summary>
    [Pure]
    bool IsArray { get; }

    /// <summary>
    /// Gets whether the type is sealed.
    /// </summary>
    [Pure]
    bool IsSealed { get; }

    /// <summary>
    /// Gets whether the type is a class (reference type that is not a delegate or value type).
    /// </summary>
    [Pure]
    bool IsClass { get; }

    /// <summary>
    /// Gets whether the type is a value type (struct or enum).
    /// </summary>
    [Pure]
    bool IsValueType { get; }

    /// <summary>
    /// Gets whether the type is a primitive type (e.g. int, bool, double).
    /// </summary>
    [Pure]
    bool IsPrimitive { get; }

    /// <summary>
    /// Gets whether the type is a static class (abstract and sealed).
    /// </summary>
    [Pure]
    bool IsStaticClass { get; }

    /// <summary>
    /// Gets whether the type is a constructed generic type (has type arguments supplied).
    /// </summary>
    [Pure]
    bool IsConstructedGenericType { get; }

    /// <summary>
    /// Gets whether the type is both abstract and sealed (e.g. static class).
    /// </summary>
    [Pure]
    bool IsAbstractAndSealed { get; }

    /// <summary>
    /// Gets whether the type is <see cref="Nullable{T}"/>.
    /// </summary>
    [Pure]
    bool IsNullable { get; }

    /// <summary>
    /// Gets whether the type implements <see cref="System.Collections.IDictionary"/> or <see cref="System.Collections.Generic.IDictionary{TKey,TValue}"/>.
    /// </summary>
    [Pure]
    bool IsDictionary { get; }

    /// <summary>
    /// Gets whether the type implements <see cref="System.Collections.Generic.ICollection{T}"/>.
    /// </summary>
    [Pure]
    bool IsCollection { get; }

    /// <summary>
    /// Gets whether the type implements <see cref="System.Collections.IEnumerable"/>.
    /// </summary>
    [Pure]
    bool IsEnumerable { get; }

    /// <summary>
    /// Gets whether the type implements <see cref="System.Collections.Generic.IReadOnlyDictionary{TKey,TValue}"/>.
    /// </summary>
    [Pure]
    bool IsReadOnlyDictionary { get; }

    /// <summary>
    /// Gets whether the type is <see cref="System.Dynamic.ExpandoObject"/>.
    /// </summary>
    [Pure]
    bool IsExpandoObject { get; }

    /// <summary>
    /// Gets whether the type is a <see cref="Func{TResult}"/> or related delegate type.
    /// </summary>
    [Pure]
    bool IsFunc { get; }

    /// <summary>
    /// Gets whether the type is a <see cref="ValueTuple"/> or related tuple type.
    /// </summary>
    [Pure]
    bool IsTuple { get; }

    /// <summary>
    /// Gets whether the type is a delegate (inherits from <see cref="Delegate"/>).
    /// </summary>
    [Pure]
    bool IsDelegate { get; }

    /// <summary>
    /// Gets whether the type is an anonymous type (compiler-generated).
    /// </summary>
    [Pure]
    bool IsAnonymousType { get; }

    /// <summary>
    /// Gets whether the type is a C# record (class or struct with compiler-generated members).
    /// </summary>
    [Pure]
    bool IsRecord { get; }

    /// <summary>
    /// Gets whether the type is a nullable value type (e.g. int?, DateTime?).
    /// </summary>
    [Pure]
    bool IsNullableValueType { get; }

    /// <summary>
    /// Gets whether the type is marked with <see cref="ObsoleteAttribute"/>.
    /// </summary>
    [Pure]
    bool IsObsolete { get; }

    /// <summary>
    /// Gets whether the type is <see cref="WeakReference"/> or <see cref="WeakReference{T}"/>.
    /// </summary>
    [Pure]
    bool IsWeakReference { get; }

    /// <summary>
    /// Gets whether the type is an enum value type (e.g. marked with EnumValueAttribute).
    /// </summary>
    [Pure]
    bool IsEnumValue { get; }

    /// <summary>
    /// Gets whether the type is an Intellenum (marked with IntellenumAttribute).
    /// </summary>
    [Pure]
    bool IsIntellenum { get; }

    /// <summary>
    /// Gets whether the type is a smart enum (implements ISmartEnum).
    /// </summary>
    [Pure]
    bool IsSmartEnum { get; }

    /// <summary>
    /// Gets the cached property with the specified name.
    /// </summary>
    /// <param name="property">The name of the property.</param>
    /// <returns>The cached property, or null if not found.</returns>
    [Pure]
    CachedProperty? GetCachedProperty(string property);

    /// <summary>
    /// Gets information about a specific property using reflection.
    /// </summary>
    /// <param name="property">The name of the property.</param>
    /// <returns>PropertyInfo for the specified property, or null if not found.</returns>
    [Pure]
    PropertyInfo? GetProperty(string property);

    /// <summary>
    /// Gets all cached properties for this type.
    /// </summary>
    /// <returns>An array of cached properties, or null.</returns>
    [Pure]
    CachedProperty[]? GetCachedProperties();

    /// <summary>
    /// Gets information about all properties using reflection.
    /// </summary>
    /// <returns>An array of PropertyInfo objects representing all properties.</returns>
    [Pure]
    PropertyInfo[]? GetProperties();
}
