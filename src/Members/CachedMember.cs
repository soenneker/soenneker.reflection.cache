using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Members.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Members;

///<inheritdoc cref="ICachedMember"/>
public sealed class CachedMember : ICachedMember
{
    public MemberInfo? MemberInfo { get; }

    public string? Name => MemberInfo?.Name;

    private readonly Lazy<CachedCustomAttributes>? _attributes;

    public CachedType CachedType { get; }

    public Type Type => CachedType.Type!;

    public int CacheKey { get; }

    public MemberTypes MemberType { get; }

    public bool IsProperty => _isPropertyLazy.Value;
    private readonly Lazy<bool> _isPropertyLazy;

    public bool IsField => _isFieldLazy.Value;
    private readonly Lazy<bool> _isFieldLazy;

    public CachedMember(MemberInfo memberInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        MemberType = memberInfo.MemberType;

        CacheKey = memberInfo.ToHashKey();

        CachedType = cachedTypes.GetCachedType(memberInfo.DeclaringType);
        MemberInfo = memberInfo;

        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, cachedTypes, threadSafe), threadSafe);
        
        _isPropertyLazy = new Lazy<bool>(() => MemberType == MemberTypes.Property, threadSafe);
        _isFieldLazy = new Lazy<bool>(() => MemberType == MemberTypes.Field, threadSafe);
    }

    public CachedCustomAttributes? GetCachedCustomAttributes()
    {
        if (MemberInfo == null)
            return null;

        return _attributes!.Value;
    }

    public object[] GetCustomAttributes()
    {
        if (MemberInfo == null)
            return [];

        return _attributes!.Value.GetCustomAttributes();
    }
}
