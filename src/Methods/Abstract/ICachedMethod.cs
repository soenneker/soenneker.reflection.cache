using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Methods.Abstract;

/// <summary>
/// Represents a cached method for a type.
/// </summary>
public interface ICachedMethod
{
    /// <summary>
    /// Gets the <see cref="MethodInfo"/> associated with this cached method.
    /// </summary>
    MethodInfo? MethodInfo { get; }

    /// <summary>
    /// Gets the name of the method.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the return type of the method.
    /// </summary>
    Type? ReturnType { get; }

    CachedParameters? GetCachedParameters();

    ParameterInfo[] GetParameters();

    CachedCustomAttributes? GetCachedCustomAttributes();

    CachedMethod? MakeCachedGenericMethod(params CachedType[] cachedTypes);

    CachedMethod? MakeCachedGenericMethod(CachedType t0);

    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1);

    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1, CachedType t2);

    CachedMethod? MakeCachedGenericMethod(CachedType t0, CachedType t1, CachedType t2, CachedType t3);

    object[] GetCustomAttributes();

    object? Invoke(object instance);

    object? Invoke(object instance, params object[] param);

    object? Invoke(object instance, object? arg0);

    object? Invoke(object instance, object? arg0, object? arg1);

    object? Invoke(object instance, object? arg0, object? arg1, object? arg2);

    object? Invoke(object instance, object? arg0, object? arg1, object? arg2, object? arg3);

    T? Invoke<T>(object instance);

    T? Invoke<T>(params object[] param);

    T? Invoke<T>(object instance, object? arg0);

    T? Invoke<T>(object instance, object? arg0, object? arg1);

    T? Invoke<T>(object instance, object? arg0, object? arg1, object? arg2);

    T? Invoke<T>(object instance, object? arg0, object? arg1, object? arg2, object? arg3);
}