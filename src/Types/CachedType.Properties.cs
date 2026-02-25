using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Utils.LazyBools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Types;

public partial class CachedType
{
    public bool IsAbstract => Type is { IsAbstract: true };
    public bool IsInterface => Type is { IsInterface: true };
    public bool IsGenericType => Type is { IsGenericType: true };
    public bool IsEnum => Type is { IsEnum: true };
    public bool IsByRef => Type is { IsByRef: true };
    public bool IsArray => Type is { IsArray: true };
    public bool IsSealed => Type is { IsSealed: true };
    public bool IsClass => Type is { IsClass: true };
    public bool IsValueType => Type is { IsValueType: true };
    public bool IsPrimitive => Type is { IsPrimitive: true };
    public bool IsStaticClass => Type is { IsAbstract: true, IsSealed: true };
    public bool IsConstructedGenericType => Type?.IsConstructedGenericType == true;
    public bool IsAbstractAndSealed => Type is { IsAbstract: true, IsSealed: true };

    private int _isNullable;
    public bool IsNullable =>
        LazyBoolUtil.GetOrInit(ref _isNullable, _threadSafe, this, static self =>
        {
            Type? t = self.Type;
            return t != null && Nullable.GetUnderlyingType(t) != null;
        });

    private int _isDictionary;
    public bool IsDictionary =>
        LazyBoolUtil.GetOrInit(ref _isDictionary, _threadSafe, this, static self => self.ComputeIsDictionary());

    private int _isCollection;
    public bool IsCollection =>
        LazyBoolUtil.GetOrInit(ref _isCollection, _threadSafe, this, static self => self.ComputeIsCollection());

    private int _isEnumerable;
    public bool IsEnumerable =>
        LazyBoolUtil.GetOrInit(ref _isEnumerable, _threadSafe, this, static self => self.ComputeIsEnumerable());

    private int _isReadOnlyDictionary;
    public bool IsReadOnlyDictionary =>
        LazyBoolUtil.GetOrInit(ref _isReadOnlyDictionary, _threadSafe, this, static self => self.ComputeIsReadOnlyDictionary());

    private int _isExpandoObject;
    public bool IsExpandoObject =>
        LazyBoolUtil.GetOrInit(ref _isExpandoObject, _threadSafe, this, static self =>
        {
            Type? t = self.Type;
            return t != null && t == typeof(ExpandoObject);
        });

    private int _isFunc;
    public bool IsFunc =>
        LazyBoolUtil.GetOrInit(ref _isFunc, _threadSafe, this, static self => self.ComputeIsFunc());

    private int _isTuple;
    public bool IsTuple =>
        LazyBoolUtil.GetOrInit(ref _isTuple, _threadSafe, this, static self => self.ComputeIsTuple());

    private int _isDelegate;
    public bool IsDelegate =>
        LazyBoolUtil.GetOrInit(ref _isDelegate, _threadSafe, this, static self =>
            self._cachedTypes.GetCachedType(typeof(Delegate)).IsAssignableFrom(self));

    private int _isAnonymousType;
    public bool IsAnonymousType =>
        LazyBoolUtil.GetOrInit(ref _isAnonymousType, _threadSafe, this, static self =>
        {
            Type? t = self.Type;
            return t != null && t.Name.Contains("AnonymousType") && t.IsSealed && t.IsGenericType;
        });

    private int _isRecord;
    public bool IsRecord =>
        LazyBoolUtil.GetOrInit(ref _isRecord, _threadSafe, this, static self => self.ComputeIsRecord());

    private int _isNullableValueType;
    public bool IsNullableValueType =>
        LazyBoolUtil.GetOrInit(ref _isNullableValueType, _threadSafe, this, static self =>
        {
            Type? t = self.Type;
            return t != null && Nullable.GetUnderlyingType(t)?.IsValueType == true;
        });

    private int _isObsolete;
    public bool IsObsolete =>
        LazyBoolUtil.GetOrInit(ref _isObsolete, _threadSafe, this, static self =>
            self.Type?.GetCustomAttribute<ObsoleteAttribute>() != null);

    private int _isWeakReference;
    public bool IsWeakReference =>
        LazyBoolUtil.GetOrInit(ref _isWeakReference, _threadSafe, this, static self => self.ComputeIsWeakReference());

    private int _isEnumValue;
    public bool IsEnumValue =>
        LazyBoolUtil.GetOrInit(ref _isEnumValue, _threadSafe, this, static self => self.ComputeIsEnumValue());

    private int _isIntellenum;
    public bool IsIntellenum =>
        LazyBoolUtil.GetOrInit(ref _isIntellenum, _threadSafe, this, static self => self.ComputeIsIntellenum());

    private int _isSmartEnum;
    public bool IsSmartEnum =>
        LazyBoolUtil.GetOrInit(ref _isSmartEnum, _threadSafe, this, static self => self.ComputeIsSmartEnum());
    
    private bool ComputeIsDictionary()
    {
        if (Type == null)
            return false;

        if (IsGenericType)
        {
            CachedType genericDictionary = _cachedTypes.GetCachedType(typeof(IDictionary<,>));

            if (GetCachedGenericTypeDefinition() == genericDictionary.GetCachedGenericTypeDefinition())
                return true;
        }

        CachedType dictionaryType = _cachedTypes.GetCachedType(typeof(IDictionary));
        return dictionaryType.IsAssignableFrom(this);
    }

    private bool ComputeIsCollection()
    {
        if (Type == null)
            return false;

        if (Type.Name == "ICollection`1")
            return true;

        CachedType[] interfaces = GetCachedInterfaces()!;

        for (var index = 0; index < interfaces.Length; index++)
        {
            CachedType i = interfaces[index];

            if (i.Type!.Name == "ICollection`1")
                return true;
        }

        return false;
    }

    private bool ComputeIsEnumerable()
    {
        if (Type == null)
            return false;

        CachedType enumerableType = _cachedTypes.GetCachedType(typeof(IEnumerable));
        return enumerableType.IsAssignableFrom(this);
    }

    private bool ComputeIsReadOnlyDictionary()
    {
        // Not fun that ReadOnlyDictionary can't use IsAssignableFrom
        if (Type == null)
            return false;

        if (Type.Name == "ReadOnlyDictionary")
            return true;

        CachedType[] interfaces = GetCachedInterfaces()!;

        for (var index = 0; index < interfaces.Length; index++)
        {
            CachedType i = interfaces[index];

            if (i.Type!.Name == "IReadOnlyDictionary`2")
                return true;
        }

        return false;
    }

    private bool ComputeIsFunc()
    {
        if (Type == null)
            return false;

        if (IsGenericType && GetCachedGenericTypeDefinition() == _cachedTypes.GetCachedType(typeof(Func<>)))
            return true;

        return false;
    }

    private bool ComputeIsTuple()
    {
        return Type is { IsGenericType: true } &&
               GetCachedGenericTypeDefinition() == _cachedTypes.GetCachedType(typeof(ValueTuple<>));
    }

    private bool ComputeIsRecord()
    {
        if (Type == null)
            return false;

        bool hasCloneMethod = GetCachedMethod("<Clone>$") != null;

        return Type.IsClass && hasCloneMethod;
    }

    private bool ComputeIsWeakReference()
    {
        if (Type == null)
            return false;

        // WeakReference is sealed, cannot derive
        if (Type == typeof(WeakReference))
            return true;

        if (IsGenericType && GetCachedGenericTypeDefinition() == _cachedTypes.GetCachedType(typeof(WeakReference<>)))
            return true;

        return false;
    }

    private bool ComputeIsIntellenum()
    {
        if (!IsClass)
            return false;

        CachedAttribute[]? attributes = GetCachedCustomAttributes();

        if (attributes == null || attributes.Length == 0)
            return false;

        for (var x = 0; x < attributes.Length; x++)
        {
            CachedAttribute attribute = attributes[x];

            if (attribute.Name.StartsWith("IntellenumAttribute"))
                return true;
        }

        return false;
    }

    private bool ComputeIsEnumValue()
    {
        if (!IsClass)
            return false;

        CachedAttribute[]? attributes = GetCachedCustomAttributes();

        if (attributes == null || attributes.Length == 0)
            return false;

        for (var x = 0; x < attributes.Length; x++)
        {
            CachedAttribute attribute = attributes[x];

            if (attribute.Name.StartsWith("EnumValueAttribute"))
                return true;
        }

        return false;
    }

    private bool ComputeIsSmartEnum()
    {
        if (!IsClass)
            return false;

        CachedType[] interfaces = GetCachedInterfaces()!;

        for (var index = 0; index < interfaces.Length; index++)
        {
            CachedType i = interfaces[index];

            if (i.Type!.Name == "ISmartEnum")
                return true;
        }

        return false;
    }
}