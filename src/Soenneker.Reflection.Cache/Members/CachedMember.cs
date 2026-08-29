using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Members.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Reflection.Cache.Utils;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Soenneker.Reflection.Cache.Members;

/// <inheritdoc cref="ICachedMember"/>
public sealed class CachedMember : ICachedMember
{
    public MemberInfo? MemberInfo { get; }

    public string? Name => MemberInfo?.Name;

    private ValueLazy<CachedCustomAttributes> _attributes;

    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    public CachedType CachedType { get; }

    public Type Type => CachedType.Type!;

    public int CacheKey { get; }

    public MemberTypes MemberType { get; }

    public bool IsProperty => MemberType == MemberTypes.Property;

    public bool IsField => MemberType == MemberTypes.Field;

    public CachedMember(MemberInfo memberInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        MemberType = memberInfo.MemberType;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;

        CacheKey = memberInfo.ToHashKey();

        CachedType = cachedTypes.GetCachedType(memberInfo.DeclaringType!);
        MemberInfo = memberInfo;

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedCustomAttributes GetAttributesCache() =>
        _attributes.GetOrCreatePublicationOnly(_threadSafe, this,
            static self => new CachedCustomAttributes(self, self._cachedTypes, self._threadSafe));

    public CachedCustomAttributes? GetCachedCustomAttributes()
    {
        if (MemberInfo == null)
            return null;

        return GetAttributesCache();
    }

    public object[] GetCustomAttributes()
    {
        if (MemberInfo == null)
            return [];

        return GetAttributesCache().GetCustomAttributes();
    }

    public T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute
    {
        if (MemberInfo == null)
            return null;

        return GetAttributesCache().GetCachedCustomAttribute<T>(inherit);
    }
}
