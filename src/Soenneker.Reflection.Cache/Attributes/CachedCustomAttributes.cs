using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Attributes.Abstract;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Members;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Attributes;

/// <inheritdoc cref="ICachedCustomAttributes"/>
public sealed class CachedCustomAttributes : ICachedCustomAttributes
{
    private readonly CachedType? _cachedType;
    private readonly CachedMethod? _cachedMethod;
    private readonly CachedConstructor? _cachedConstructor;
    private readonly CachedMember? _cachedMember;

    private ValueLazy<CachedAttribute[]> _cachedCustomAttributes;
    private ValueLazy<object[]> _cachedObjects;
    private ValueAtomicLock _sync;
    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    public CachedCustomAttributes(CachedType cachedType, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedType = cachedType;
        _threadSafe = threadSafe;
    }

    public CachedCustomAttributes(CachedMethod cachedMethod, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedMethod = cachedMethod;
        _threadSafe = threadSafe;
    }

    public CachedCustomAttributes(CachedConstructor cachedConstructor, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedConstructor = cachedConstructor;
        _threadSafe = threadSafe;
    }

    public CachedCustomAttributes(CachedMember cachedMember, CachedTypes cachedTypes, bool threadSafe = true)
    {
        _cachedTypes = cachedTypes;
        _cachedMember = cachedMember;
        _threadSafe = threadSafe;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private object[] GetObjectsCache() =>
        _cachedObjects.GetOrCreate(_threadSafe, ref _sync, this, static self => self.LoadObjects());

    private object[] LoadObjects()
    {
        if (_cachedType?.Type is { } type)
            return type.GetCustomAttributes(inherit: true);

        if (_cachedMethod?.MethodInfo is { } method)
            return method.GetCustomAttributes(inherit: true);

        if (_cachedConstructor?.ConstructorInfo is { } constructor)
            return constructor.GetCustomAttributes(inherit: true);

        if (_cachedMember?.MemberInfo is { } member)
            return member.GetCustomAttributes(inherit: true);

        return [];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedAttribute[] GetCachedAttributesCache() =>
        _cachedCustomAttributes.GetOrCreate(_threadSafe, ref _sync, this,
            static self => self.BuildCachedAttributes(self.GetObjectsCache()));

    private CachedAttribute[] BuildCachedAttributes(object[] attributes)
    {
        var result = new CachedAttribute[attributes.Length];

        for (var i = 0; i < attributes.Length; i++)
            result[i] = new CachedAttribute(attributes[i], _cachedTypes, _threadSafe);

        return result;
    }

    public T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute
    {
        if (inherit)
        {
            object[] attrs = GetObjectsCache();

            for (var i = 0; i < attrs.Length; i++)
            {
                if (attrs[i] is T match)
                    return match;
            }

            return null;
        }

        if (_cachedType?.Type is { } type)
            return type.GetCustomAttribute<T>(inherit: false);

        if (_cachedMethod?.MethodInfo is { } method)
            return method.GetCustomAttribute<T>(inherit: false);

        if (_cachedConstructor?.ConstructorInfo is { } ctor)
            return ctor.GetCustomAttribute<T>(inherit: false);

        if (_cachedMember?.MemberInfo is { } member)
            return member.GetCustomAttribute<T>(inherit: false);

        return null;
    }

    public CachedAttribute[] GetCachedCustomAttributes()
    {
        return GetCachedAttributesCache();
    }

    public object[] GetCustomAttributes()
    {
        return GetObjectsCache();
    }
}
