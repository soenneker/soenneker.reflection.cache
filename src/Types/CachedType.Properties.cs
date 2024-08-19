using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

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
    }
}