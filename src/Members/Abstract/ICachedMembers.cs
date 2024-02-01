using System.Reflection;

namespace Soenneker.Reflection.Cache.Members.Abstract;

/// <summary>
/// Represents a cached set of members for a type.
/// </summary>
public interface ICachedMembers
{
    CachedMember[] GetCachedMembers();

    /// <summary>
    /// Gets an array of cached members.
    /// </summary>
    /// <returns>An array of cached members.</returns>
    MemberInfo[] GetMembers();
}