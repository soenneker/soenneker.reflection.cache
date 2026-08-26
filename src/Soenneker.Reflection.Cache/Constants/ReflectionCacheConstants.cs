using System.Reflection;

namespace Soenneker.Reflection.Cache.Constants;

/// <summary>
/// Defines default values used by the reflection cache.
/// </summary>
public static class ReflectionCacheConstants
{
    // ReSharper disable once ConvertToConstant.Global
    /// <summary>
    /// Gets the default reflection scope: public and non-public, instance and static members.
    /// </summary>
    public static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
}
