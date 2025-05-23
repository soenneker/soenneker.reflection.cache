﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;

namespace Soenneker.Reflection.Cache.Types;

public partial class CachedType
{
    public bool IsAbstract => _isAbstractLazy.Value;
    private Lazy<bool> _isAbstractLazy;

    public bool IsInterface => _isInterfaceLazy.Value;
    private Lazy<bool> _isInterfaceLazy;

    public bool IsGenericType => _isGenericTypeLazy.Value;
    private Lazy<bool> _isGenericTypeLazy;

    public bool IsEnum => _isEnumLazy.Value;
    private Lazy<bool> _isEnumLazy;

    public bool IsNullable => _isNullable.Value;
    private Lazy<bool> _isNullable;

    public bool IsByRef => _isByRef.Value;
    private Lazy<bool> _isByRef;

    public bool IsArray => _isArray.Value;
    private Lazy<bool> _isArray;

    public bool IsDictionary => _isDictionary.Value;
    private Lazy<bool> _isDictionary;

    public bool IsCollection => _isCollection.Value;
    private Lazy<bool> _isCollection;

    public bool IsEnumerable => _isEnumerable.Value;
    private Lazy<bool> _isEnumerable;

    public bool IsReadOnlyDictionary => _isReadOnlyDictionary.Value;
    private Lazy<bool> _isReadOnlyDictionary;

    public bool IsExpandoObject => _isExpandoObject.Value;
    private Lazy<bool> _isExpandoObject;

    public bool IsSealed => _isSealed.Value;
    private Lazy<bool> _isSealed;

    public bool IsFunc => _isFunc.Value;
    private Lazy<bool> _isFunc;

    public bool IsClass => _isClass.Value;
    private Lazy<bool> _isClass;

    public bool IsValueType => _isValueType.Value;
    private Lazy<bool> _isValueType;

    public bool IsPrimitive => _isPrimitive.Value;
    private Lazy<bool> _isPrimitive;

    public bool IsStaticClass => _isStaticClass.Value;
    private Lazy<bool> _isStaticClass;

    public bool IsTuple => _isTuple.Value;
    private Lazy<bool> _isTuple;

    public bool IsDelegate => _isDelegate.Value;
    private Lazy<bool> _isDelegate;

    public bool IsAnonymousType => _isAnonymousType.Value;
    private Lazy<bool> _isAnonymousType;

    public bool IsRecord => _isRecord.Value;
    private Lazy<bool> _isRecord;

    public bool IsNullableValueType => _isNullableValueType.Value;
    private Lazy<bool> _isNullableValueType;

    public bool IsObsolete => _isObsolete.Value;
    private Lazy<bool> _isObsolete;

    public bool IsConstructedGenericType => _isConstructedGenericType.Value;
    private Lazy<bool> _isConstructedGenericType;

    public bool IsAbstractAndSealed => _isAbstractAndSealed.Value;
    private Lazy<bool> _isAbstractAndSealed;

    public bool IsWeakReference => _isWeakReference.Value;
    private Lazy<bool> _isWeakReference;

    public bool IsIntellenum => _isIntellenumLazy.Value;
    private Lazy<bool> _isIntellenumLazy;

    public bool IsSmartEnum => _isSmartEnumLazy.Value;
    private Lazy<bool> _isSmartEnumLazy;

    private void InitializeProperties()
    {
        _isAbstractLazy = new Lazy<bool>(() => Type is {IsAbstract: true}, _threadSafe);
        _isInterfaceLazy = new Lazy<bool>(() => Type is {IsInterface: true}, _threadSafe);
        _isGenericTypeLazy = new Lazy<bool>(() => Type is {IsGenericType: true}, _threadSafe);
        _isEnumLazy = new Lazy<bool>(() => Type is {IsEnum: true}, _threadSafe);

        _isNullable = new Lazy<bool>(() =>
        {
            if (Type == null)
                return false;

            return Nullable.GetUnderlyingType(Type) != null;
        }, _threadSafe);

        _isByRef = new Lazy<bool>(() => Type is {IsByRef: true}, _threadSafe);
        _isArray = new Lazy<bool>(() => Type is {IsArray: true}, _threadSafe);

        _isDictionary = new Lazy<bool>(() =>
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

            if (dictionaryType.IsAssignableFrom(this))
                return true;

            return false;
        }, _threadSafe);

        _isCollection = new Lazy<bool>(() =>
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
        }, _threadSafe);

        _isEnumerable = new Lazy<bool>(() =>
        {
            if (Type == null)
                return false;

            CachedType enumerableType = _cachedTypes.GetCachedType(typeof(IEnumerable));

            if (enumerableType.IsAssignableFrom(this))
                return true;

            return false;
        }, _threadSafe);

        _isReadOnlyDictionary = new Lazy<bool>(() =>
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
        }, _threadSafe);

        _isExpandoObject = new Lazy<bool>(() =>
        {
            if (Type == null)
                return false;

            return Type == typeof(ExpandoObject);
        }, _threadSafe);

        _isSealed = new Lazy<bool>(() => Type is {IsSealed: true}, _threadSafe);

        _isFunc = new Lazy<bool>(() =>
        {
            if (Type == null)
                return false;

            if (IsGenericType && GetCachedGenericTypeDefinition() == _cachedTypes.GetCachedType(typeof(Func<>)))
                return true;

            return false;
        }, _threadSafe);

        _isClass = new Lazy<bool>(() => Type is {IsClass: true}, _threadSafe);
        _isValueType = new Lazy<bool>(() => Type is {IsValueType: true}, _threadSafe);
        _isPrimitive = new Lazy<bool>(() => Type is {IsPrimitive: true}, _threadSafe);
        _isStaticClass = new Lazy<bool>(() => Type is {IsAbstract: true, IsSealed: true}, _threadSafe);
        _isTuple = new Lazy<bool>(() => Type is {IsGenericType: true} && GetCachedGenericTypeDefinition() == _cachedTypes.GetCachedType(typeof(ValueTuple<>)),
            _threadSafe);
        _isDelegate = new Lazy<bool>(() => _cachedTypes.GetCachedType(typeof(Delegate)).IsAssignableFrom(this), _threadSafe);

        _isAnonymousType = new Lazy<bool>(() =>
        {
            if (Type == null)
                return false;

            return Type.Name.Contains("AnonymousType") && Type.IsSealed && Type.IsGenericType;
        }, _threadSafe);

        _isRecord = new Lazy<bool>(() =>
        {
            if (Type == null)
                return false;

            bool hasCloneMethod = GetCachedMethod("<Clone>$") != null;

            return Type.IsClass && hasCloneMethod;
        }, _threadSafe);

        _isNullableValueType = new Lazy<bool>(() =>
        {
            if (Type == null)
                return false;

            return Nullable.GetUnderlyingType(Type)?.IsValueType == true;
        }, _threadSafe);

        _isObsolete = new Lazy<bool>(() => { return Type?.GetCustomAttribute<ObsoleteAttribute>() != null; }, _threadSafe);

        _isConstructedGenericType = new Lazy<bool>(() => { return Type?.IsConstructedGenericType == true; }, _threadSafe);

        _isAbstractAndSealed = new Lazy<bool>(() => Type is {IsAbstract: true, IsSealed: true}, _threadSafe);

        _isWeakReference = new Lazy<bool>(() =>
        {
            if (Type == null)
                return false;

            // WeakReference is sealed, cannot derive

            if (Type == typeof(WeakReference))
                return true;

            if (IsGenericType && GetCachedGenericTypeDefinition() == _cachedTypes.GetCachedType(typeof(WeakReference<>)))
                return true;

            return false;
        }, _threadSafe);

        _isIntellenumLazy = new Lazy<bool>(() =>
        {
            if (!_isClass.Value)
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
        }, _threadSafe);

        _isSmartEnumLazy = new Lazy<bool>(() =>
        {
            if (!_isClass.Value)
                return false;

            CachedType[] interfaces = GetCachedInterfaces()!;

            for (var index = 0; index < interfaces.Length; index++)
            {
                CachedType i = interfaces[index];

                if (i.Type!.Name == "ISmartEnum")
                    return true;
            }

            return false;
        }, _threadSafe);
    }
}