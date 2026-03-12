using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Extensions;
using Soenneker.Reflection.Cache.Members.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Utils.LazyBools;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Members;

///<inheritdoc cref="ICachedMember"/>
public sealed class CachedMember : ICachedMember
{
    public MemberInfo? MemberInfo { get; }

    public string? Name => MemberInfo?.Name;

    private readonly Lazy<CachedCustomAttributes>? _attributes;

    private readonly bool _threadSafe;

    public CachedType CachedType { get; }

    public Type Type => CachedType.Type!;

    public int CacheKey { get; }

    public MemberTypes MemberType { get; }

    private int _isProperty;
    public bool IsProperty =>
        LazyBoolUtil.GetOrInit(ref _isProperty, _threadSafe, this, static self => self.MemberType == MemberTypes.Property);

    private int _isField;
    public bool IsField =>
        LazyBoolUtil.GetOrInit(ref _isField, _threadSafe, this, static self => self.MemberType == MemberTypes.Field);

    public CachedMember(MemberInfo memberInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        MemberType = memberInfo.MemberType;
        _threadSafe = threadSafe;

        CacheKey = memberInfo.ToHashKey();

        CachedType = cachedTypes.GetCachedType(memberInfo.DeclaringType);
        MemberInfo = memberInfo;

        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, cachedTypes, threadSafe), threadSafe);
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
