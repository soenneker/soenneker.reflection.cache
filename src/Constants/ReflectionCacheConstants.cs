using System.Reflection;

namespace Soenneker.Reflection.Cache.Constants;

public static class ReflectionCacheConstants
{
    // ReSharper disable once ConvertToConstant.Global
    public static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
}