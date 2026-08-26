using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Members.Abstract;

/// <summary>
/// Provides cached reflection metadata and attributes for a member.
/// </summary>
public interface ICachedMember
{
    /// <summary>
    /// Gets the cached declaring type.
    /// </summary>
    CachedType CachedType { get; }

    /// <summary>
    /// Gets the declaring reflection type.
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
    /// Gets a value indicating whether the member is a property.
    /// </summary>
    bool IsProperty { get; }

    /// <summary>
    /// Gets a value indicating whether the member is a field.
    /// </summary>
    bool IsField { get; }

   // int CacheKey { get; }

    /// <summary>
    /// Gets the reflection member category.
    /// </summary>
    MemberTypes MemberType { get; }

    /// <summary>
    /// Gets the cached custom attributes applied to the member.
    /// </summary>
    /// <returns>The cached attribute collection, or <c>null</c> when no member metadata is available.</returns>
    CachedCustomAttributes? GetCachedCustomAttributes();

    /// <summary>
    /// Gets the custom attributes applied to the member.
    /// </summary>
    /// <returns>The instantiated custom attributes.</returns>
    object[] GetCustomAttributes();

    /// <summary>
    /// Gets the first custom attribute of the specified type applied to the member.
    /// </summary>
    /// <typeparam name="T">The attribute type to retrieve.</typeparam>
    /// <param name="inherit">Whether to search the member inheritance chain.</param>
    /// <returns>The matching attribute, or <c>null</c> when none is present.</returns>
    T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute;
}
