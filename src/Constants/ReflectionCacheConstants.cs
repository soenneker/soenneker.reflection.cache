using System.Reflection;

namespace Soenneker.Reflection.Cache.Constants;

public static class ReflectionCacheConstants
{
    // ReSharper disable once ConvertToConstant.Global
    public static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;

    public static BindingFlags BindingFlagsProperties
    {
        get;
        set;
    } = BindingFlags;

    public static BindingFlags BindingFlagsFields
    {
        get;
        set;
    } = BindingFlags;
}