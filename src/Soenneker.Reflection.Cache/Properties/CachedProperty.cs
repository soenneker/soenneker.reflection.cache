using Soenneker.Reflection.Cache.Properties.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Utils.LazyBools;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Soenneker.Reflection.Cache.Properties;

public sealed class CachedProperty : ICachedProperty
{
    public PropertyInfo PropertyInfo { get; }

    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    private int _isDelegate;

    private readonly Lazy<Func<object, object?>?> _getter;
    private readonly Lazy<Action<object, object?>?> _setter;

    public bool IsDelegate =>
        LazyBoolUtil.GetOrInit(ref _isDelegate, _threadSafe, this, static self => self._cachedTypes.GetCachedType(typeof(Delegate))
                                                                                      .IsAssignableFrom(self.PropertyInfo.PropertyType));

    private int _isCompilerGenerated;

    public bool IsCompilerGenerated =>
        LazyBoolUtil.GetOrInit(ref _isCompilerGenerated, _threadSafe, this,
            static self => Attribute.IsDefined(self.PropertyInfo, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute)));

    public bool IsEqualityContract { get; }
    public bool IsStatic { get; }
    public bool IsVirtual { get; }
    public bool IsIndexer { get; }
    public bool IsReadOnly { get; }
    public bool IsPublic { get; }
    public bool IsProtected { get; }
    public bool IsPrivate { get; }

    public bool CanGetValue => GetGetter() is not null;

    public bool CanSetValue => GetSetter() is not null;

    public CachedProperty(PropertyInfo propertyInfo, CachedTypes cachedTypes, bool threadSafe)
    {
        PropertyInfo = propertyInfo;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;
        _getter = new Lazy<Func<object, object?>?>(() => BuildGetter(PropertyInfo), threadSafe);
        _setter = new Lazy<Action<object, object?>?>(() => BuildSetter(PropertyInfo), threadSafe);

        // Cheap checks are evaluated once during construction.
        MethodInfo? getMethod = PropertyInfo.GetMethod;
        MethodInfo? setMethod = PropertyInfo.SetMethod;

        IsEqualityContract = PropertyInfo.Name == "EqualityContract";
        IsStatic = (getMethod ?? setMethod)?.IsStatic ?? false;
        IsVirtual = (getMethod ?? setMethod)?.IsVirtual ?? false;
        IsIndexer = PropertyInfo.GetIndexParameters()
                                .Length > 0;
        IsReadOnly = setMethod == null;
        IsPublic = getMethod?.IsPublic == true;
        IsProtected = getMethod?.IsFamily == true;
        IsPrivate = getMethod?.IsPrivate == true && (setMethod == null || setMethod.IsPrivate);
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
        if (setter is null || !IsCompatibleInstance(instance) || !IsAssignableValue(PropertyInfo.PropertyType, value))
            return false;

        setter(instance, value);
        return true;
    }

    public object? GetValue(object instance)
    {
        Func<object, object?>? getter = GetGetter();
        if (getter is null)
            throw new NotSupportedException($"Property '{GetDisplayName()}' does not have a supported public instance getter.");

        ValidateInstance(instance);
        return getter(instance);
    }

    public void SetValue(object instance, object? value)
    {
        Action<object, object?>? setter = GetSetter();
        if (setter is null)
            throw new NotSupportedException($"Property '{GetDisplayName()}' does not have a supported public instance setter.");

        ValidateInstance(instance);
        ValidateValue(value);

        setter(instance, value);
    }

    private static Func<object, object?>? BuildGetter(PropertyInfo propertyInfo)
    {
        if (!CanBuildGetter(propertyInfo, out Type declaringType))
            return null;

        try
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            UnaryExpression typedInstance = Expression.Convert(instance, declaringType);
            MemberExpression property = Expression.Property(typedInstance, propertyInfo);
            UnaryExpression body = Expression.Convert(property, typeof(object));

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

    private static Action<object, object?>? BuildSetter(PropertyInfo propertyInfo)
    {
        if (!CanBuildSetter(propertyInfo, out Type declaringType))
            return null;

        try
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");
            UnaryExpression typedInstance = Expression.Convert(instance, declaringType);
            UnaryExpression typedValue = Expression.Convert(value, propertyInfo.PropertyType);
            MemberExpression property = Expression.Property(typedInstance, propertyInfo);
            BinaryExpression body = Expression.Assign(property, typedValue);

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

    private static bool CanBuildGetter(PropertyInfo propertyInfo, out Type declaringType)
    {
        declaringType = null!;

        MethodInfo? getMethod = propertyInfo.GetMethod;
        if (getMethod is null || !getMethod.IsPublic || getMethod.IsStatic || propertyInfo.GetIndexParameters().Length != 0)
            return false;

        Type? propertyDeclaringType = propertyInfo.DeclaringType;
        if (propertyDeclaringType is null || propertyDeclaringType.ContainsGenericParameters || IsUnsupportedAccessorType(propertyInfo.PropertyType))
            return false;

        declaringType = propertyDeclaringType;
        return true;
    }

    private static bool CanBuildSetter(PropertyInfo propertyInfo, out Type declaringType)
    {
        declaringType = null!;

        MethodInfo? setMethod = propertyInfo.SetMethod;
        if (setMethod is null || !setMethod.IsPublic || setMethod.IsStatic || IsInitOnly(setMethod) || propertyInfo.GetIndexParameters().Length != 0)
            return false;

        Type? propertyDeclaringType = propertyInfo.DeclaringType;
        if (propertyDeclaringType is null || propertyDeclaringType.ContainsGenericParameters || propertyDeclaringType.IsValueType ||
            IsUnsupportedAccessorType(propertyInfo.PropertyType))
            return false;

        declaringType = propertyDeclaringType;
        return true;
    }

    private static bool IsUnsupportedAccessorType(Type type)
    {
        return type.IsByRef || type.IsByRefLike || type.IsPointer || type.ContainsGenericParameters;
    }

    private static bool IsInitOnly(MethodInfo setMethod)
    {
        Type[] modifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();

        for (var i = 0; i < modifiers.Length; i++)
        {
            if (modifiers[i].FullName == "System.Runtime.CompilerServices.IsExternalInit")
                return true;
        }

        return false;
    }

    private bool IsCompatibleInstance(object? instance)
    {
        Type? declaringType = PropertyInfo.DeclaringType;
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
            throw new ArgumentNullException(nameof(instance), $"An instance is required to access property '{GetDisplayName()}'.");

        Type? declaringType = PropertyInfo.DeclaringType;
        if (declaringType is null || !declaringType.IsInstanceOfType(instance))
            throw new ArgumentException($"Instance type '{instance.GetType().FullName}' is not assignable to property declaring type '{declaringType?.FullName}'.",
                nameof(instance));
    }

    private void ValidateValue(object? value)
    {
        Type propertyType = PropertyInfo.PropertyType;
        if (IsAssignableValue(propertyType, value))
            return;

        string valueTypeName = value is null ? "null" : value.GetType().FullName ?? value.GetType().Name;
        throw new ArgumentException($"Value type '{valueTypeName}' is not assignable to property '{GetDisplayName()}' of type '{propertyType.FullName}'.",
            nameof(value));
    }

    private string GetDisplayName()
    {
        return $"{PropertyInfo.DeclaringType?.FullName}.{PropertyInfo.Name}";
    }
}
