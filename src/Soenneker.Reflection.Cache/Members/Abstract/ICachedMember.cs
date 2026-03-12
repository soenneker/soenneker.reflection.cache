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

    bool IsProperty { get; }

    bool IsField { get; }

   // int CacheKey { get; }

    MemberTypes MemberType { get; }

    CachedCustomAttributes? GetCachedCustomAttributes();

    object[] GetCustomAttributes();
}
