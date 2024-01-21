using System.Reflection;

namespace Soenneker.Reflection.Cache.Members.Abstract;

/// <summary>
/// Represents a cached set of members for a type.
/// </summary>
public interface ICachedMembers
{
    /// <summary>
    /// Gets a member by name.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <returns>The member with the specified name, or <c>null</c> if not found.</returns>
    MemberInfo? GetMember(string name);

    /// <summary>
    /// Gets an array of cached members.
    /// </summary>
    /// <returns>An array of cached members.</returns>
    MemberInfo[] GetMembers();
}