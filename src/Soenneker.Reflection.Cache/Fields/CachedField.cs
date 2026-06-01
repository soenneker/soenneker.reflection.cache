using Soenneker.Reflection.Cache.Fields.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Utils.LazyBools;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Soenneker.Reflection.Cache.Fields;

///<inheritdoc cref="ICachedField"/>
public sealed class CachedField : ICachedField
{
    public FieldInfo FieldInfo { get; }

    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    private int _isDelegate;

    private readonly Lazy<Func<object, object?>?> _getter;
    private readonly Lazy<Action<object, object?>?> _setter;

    public bool IsDelegate =>
        LazyBoolUtil.GetOrInit(
            ref _isDelegate,
            _threadSafe,
            this,
            static self =>
                self._cachedTypes
                    .GetCachedType(typeof(Delegate))
                    .IsAssignableFrom(self.FieldInfo.FieldType));

    public bool CanGetValue => GetGetter() is not null;

    public bool CanSetValue => GetSetter() is not null;

    public CachedField(FieldInfo fieldInfo, CachedTypes cachedTypes, bool threadSafe)
    {
        FieldInfo = fieldInfo;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;
        _getter = new Lazy<Func<object, object?>?>(() => BuildGetter(FieldInfo), threadSafe);
        _setter = new Lazy<Action<object, object?>?>(() => BuildSetter(FieldInfo), threadSafe);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Func<object, object?>? GetGetter()
    {
        return _getter.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Action<object, object?>? GetSetter()
    {
        return _setter.Value;
    }

    public bool TryGetValue(object instance, out object? value)
    {
        value = null;

        Func<object, object?>? getter = GetGetter();
        if (getter is null || !IsCompatibleInstance(instance))
            return false;

        value = getter(instance);
        return true;
    }

    public bool TrySetValue(object instance, object? value)
    {
        Action<object, object?>? setter = GetSetter();
        if (setter is null || !IsCompatibleInstance(instance) || !IsAssignableValue(FieldInfo.FieldType, value))
            return false;

        setter(instance, value);
        return true;
    }

    public object? GetValue(object instance)
    {
        Func<object, object?>? getter = GetGetter();
        if (getter is null)
            throw new NotSupportedException($"Field '{GetDisplayName()}' does not have a supported public instance getter.");

        ValidateInstance(instance);
        return getter(instance);
    }

    public void SetValue(object instance, object? value)
    {
        Action<object, object?>? setter = GetSetter();
        if (setter is null)
            throw new NotSupportedException($"Field '{GetDisplayName()}' does not have a supported public instance setter.");

        ValidateInstance(instance);
        ValidateValue(value);

        setter(instance, value);
    }

    private static Func<object, object?>? BuildGetter(FieldInfo fieldInfo)
    {
        if (!CanBuildGetter(fieldInfo, out Type declaringType))
            return null;

        try
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            UnaryExpression typedInstance = Expression.Convert(instance, declaringType);
            MemberExpression field = Expression.Field(typedInstance, fieldInfo);
            UnaryExpression body = Expression.Convert(field, typeof(object));

            return Expression.Lambda<Func<object, object?>>(body, instance)
                             .Compile();
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (MemberAccessException)
        {
            return null;
        }
        catch (NotSupportedException)
        {
            return null;
        }
    }

    private static Action<object, object?>? BuildSetter(FieldInfo fieldInfo)
    {
        if (!CanBuildSetter(fieldInfo, out Type declaringType))
            return null;

        try
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");
            UnaryExpression typedInstance = Expression.Convert(instance, declaringType);
            UnaryExpression typedValue = Expression.Convert(value, fieldInfo.FieldType);
            MemberExpression field = Expression.Field(typedInstance, fieldInfo);
            BinaryExpression body = Expression.Assign(field, typedValue);

            return Expression.Lambda<Action<object, object?>>(body, instance, value)
                             .Compile();
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (MemberAccessException)
        {
            return null;
        }
        catch (NotSupportedException)
        {
            return null;
        }
    }

    private static bool CanBuildGetter(FieldInfo fieldInfo, out Type declaringType)
    {
        declaringType = null!;

        if (!fieldInfo.IsPublic || fieldInfo.IsStatic)
            return false;

        Type? fieldDeclaringType = fieldInfo.DeclaringType;
        if (fieldDeclaringType is null || fieldDeclaringType.ContainsGenericParameters || IsUnsupportedAccessorType(fieldInfo.FieldType))
            return false;

        declaringType = fieldDeclaringType;
        return true;
    }

    private static bool CanBuildSetter(FieldInfo fieldInfo, out Type declaringType)
    {
        declaringType = null!;

        if (!fieldInfo.IsPublic || fieldInfo.IsStatic || fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
            return false;

        Type? fieldDeclaringType = fieldInfo.DeclaringType;
        if (fieldDeclaringType is null || fieldDeclaringType.ContainsGenericParameters || fieldDeclaringType.IsValueType ||
            IsUnsupportedAccessorType(fieldInfo.FieldType))
            return false;

        declaringType = fieldDeclaringType;
        return true;
    }

    private static bool IsUnsupportedAccessorType(Type type)
    {
        return type.IsByRef || type.IsByRefLike || type.IsPointer || type.ContainsGenericParameters;
    }

    private bool IsCompatibleInstance(object? instance)
    {
        Type? declaringType = FieldInfo.DeclaringType;
        return instance is not null && declaringType is not null && declaringType.IsInstanceOfType(instance);
    }

    private static bool IsAssignableValue(Type targetType, object? value)
    {
        if (value is null)
            return !targetType.IsValueType || Nullable.GetUnderlyingType(targetType) is not null;

        Type valueType = value.GetType();
        if (targetType.IsAssignableFrom(valueType))
            return true;

        Type? nullableUnderlyingType = Nullable.GetUnderlyingType(targetType);
        return nullableUnderlyingType is not null && nullableUnderlyingType.IsAssignableFrom(valueType);
    }

    private void ValidateInstance(object? instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance), $"An instance is required to access field '{GetDisplayName()}'.");

        Type? declaringType = FieldInfo.DeclaringType;
        if (declaringType is null || !declaringType.IsInstanceOfType(instance))
            throw new ArgumentException($"Instance type '{instance.GetType().FullName}' is not assignable to field declaring type '{declaringType?.FullName}'.",
                nameof(instance));
    }

    private void ValidateValue(object? value)
    {
        Type fieldType = FieldInfo.FieldType;
        if (IsAssignableValue(fieldType, value))
            return;

        string valueTypeName = value is null ? "null" : value.GetType().FullName ?? value.GetType().Name;
        throw new ArgumentException($"Value type '{valueTypeName}' is not assignable to field '{GetDisplayName()}' of type '{fieldType.FullName}'.",
            nameof(value));
    }

    private string GetDisplayName()
    {
        return $"{FieldInfo.DeclaringType?.FullName}.{FieldInfo.Name}";
    }
}
