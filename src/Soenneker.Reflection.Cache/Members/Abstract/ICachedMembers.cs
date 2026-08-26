using System.Reflection;

namespace Soenneker.Reflection.Cache.Members.Abstract;

/// <summary>
/// Provides the cached members discovered for a type.
/// </summary>
public interface ICachedMembers
{
    /// <summary>
    /// Gets all cached members in the configured reflection scope.
    /// </summary>
    /// <returns>The cached members.</returns>
    CachedMember[] GetCachedMembers();

    /// <summary>
    /// Gets the reflection metadata for all members in the configured reflection scope.
    /// </summary>
    /// <returns>The member metadata.</returns>
    MemberInfo[] GetMembers();
}
