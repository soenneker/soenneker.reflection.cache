using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Fields.Abstract;

/// <summary>
/// Defines the cached field contract.
/// </summary>
public interface ICachedField
{
    /// <summary>
    /// Gets the <see cref="FieldInfo"/> associated with this cached field.
    /// </summary>
    FieldInfo FieldInfo { get; }

    /// <summary>
    /// Gets a value indicating whether the field's type is a delegate.
    /// </summary>
    bool IsDelegate { get; }

    /// <summary>
    /// Gets a value indicating whether this field has a supported cached public instance getter.
    /// </summary>
    bool CanGetValue { get; }

    /// <summary>
    /// Gets a value indicating whether this field has a supported cached public instance setter.
    /// </summary>
    bool CanSetValue { get; }

    /// <summary>
    /// Gets the cached object-based getter delegate for this field when it has a supported public instance getter.
    /// </summary>
    /// <returns>A cached getter delegate, or <c>null</c> when the field shape is unsupported.</returns>
    Func<object, object?>? GetGetter();

    /// <summary>
    /// Gets the cached object-based setter delegate for this field when it has a supported public instance setter.
    /// </summary>
    /// <returns>A cached setter delegate, or <c>null</c> when the field shape is unsupported.</returns>
    Action<object, object?>? GetSetter();

    /// <summary>
    /// Tries to read the field value from the supplied instance using the cached getter delegate.
    /// </summary>
    /// <param name="instance">The object instance that owns the field.</param>
    /// <param name="value">The field value when reading succeeds.</param>
    /// <returns><c>true</c> when the field was read; otherwise, <c>false</c>.</returns>
    bool TryGetValue(object instance, out object? value);

    /// <summary>
    /// Tries to assign a field value to the supplied instance using the cached setter delegate.
    /// </summary>
    /// <param name="instance">The object instance that owns the field.</param>
    /// <param name="value">The value to assign. The value must already be assignable to the field type.</param>
    /// <returns><c>true</c> when the field was assigned; otherwise, <c>false</c>.</returns>
    bool TrySetValue(object instance, object? value);

    /// <summary>
    /// Reads the field value from the supplied instance using the cached getter delegate.
    /// </summary>
    /// <param name="instance">The object instance that owns the field.</param>
    /// <returns>The field value.</returns>
    /// <exception cref="NotSupportedException">Thrown when the field does not have a supported public instance getter.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="instance"/> is not assignable to the declaring type.</exception>
    object? GetValue(object instance);

    /// <summary>
    /// Assigns the field value on the supplied instance using the cached setter delegate.
    /// </summary>
    /// <param name="instance">The object instance that owns the field.</param>
    /// <param name="value">The value to assign. The value must already be assignable to the field type.</param>
    /// <exception cref="NotSupportedException">Thrown when the field does not have a supported public instance setter.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="instance"/> or <paramref name="value"/> is not assignable.</exception>
    void SetValue(object instance, object? value);
}
