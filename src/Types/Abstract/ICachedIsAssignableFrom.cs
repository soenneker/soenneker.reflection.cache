using System;
using System.Diagnostics.Contracts;

namespace Soenneker.Reflection.Cache.Types.Abstract;

/// <summary>
/// Represents a cached mechanism for determining whether one type is assignable from another.
/// </summary>
public interface ICachedIsAssignableFrom
{
    /// <summary>
    /// Determines whether the specified target type is assignable from the specified source type.
    /// </summary>
    /// <param name="derivedType">The source type to check assignability.</param>
    /// <returns><c>true</c> if the <see cref="CachedType"/> is assignable from <paramref name="derivedType"/>; otherwise, <c>false</c>.</returns>
    [Pure] 
    bool IsAssignableFrom(Type derivedType);

    /// <summary>
    /// Determines whether the specified target type (cached) is assignable from the specified source type (cached).
    /// </summary>
    /// <param name="derivedType">The cached source type to check assignability.</param>
    /// <returns><c>true</c> if <see cref="CachedType"/> is assignable from <paramref name="derivedType"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    bool IsAssignableFrom(CachedType derivedType);
}