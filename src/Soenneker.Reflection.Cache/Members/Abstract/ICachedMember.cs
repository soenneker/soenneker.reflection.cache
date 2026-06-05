using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Members.Abstract;

/// <summary>
/// Represents a cached member for a type.
/// </summary>
public interface ICachedMember
{
    /// <summary>
    /// Declaring Type
    /// </summary>
    CachedType CachedType { get; }

    /// <summary>
    /// Declaring Type
    /// </summary>
    Type Type { get; }

    /// <summary>
    /// Gets the <see cref="MemberInfo"/> associated with this cached member.
    /// </summary>
    MemberInfo? MemberInfo { get; }

    /// <summary>
    /// Gets the name of the member.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets a value indicating whether the instance is property.
    /// </summary>
    bool IsProperty { get; }

    /// <summary>
    /// Gets a value indicating whether the instance is field.
    /// </summary>
    bool IsField { get; }

   // int CacheKey { get; }

    /// <summary>
    /// Gets member type.
    /// </summary>
    MemberTypes MemberType { get; }

    /// <summary>
    /// Gets cached custom attributes.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    CachedCustomAttributes? GetCachedCustomAttributes();

    /// <summary>
    /// Gets custom attributes.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    object[] GetCustomAttributes();

    /// <summary>
    /// Gets cached custom attribute.
    /// </summary>
    /// <typeparam name="T">The T type.</typeparam>
    /// <param name="inherit">The inherit.</param>
    /// <returns>The result of the operation.</returns>
    T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute;
}

