using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Parameters;

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

    object[] GetCustomAttributes();

    object? Invoke(object instance);

    object? Invoke(object instance, params object[] param);

    T? Invoke<T>(object instance);

    T? Invoke<T>(params object[] param);
}