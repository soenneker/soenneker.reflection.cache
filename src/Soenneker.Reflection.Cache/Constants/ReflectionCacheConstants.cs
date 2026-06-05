using System.Reflection;

namespace Soenneker.Reflection.Cache.Constants;

/// <summary>
/// Represents the reflection cache constants.
/// </summary>
public static class ReflectionCacheConstants
{
    // ReSharper disable once ConvertToConstant.Global
    /// <summary>
    /// The binding flags.
    /// </summary>
    public static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
}