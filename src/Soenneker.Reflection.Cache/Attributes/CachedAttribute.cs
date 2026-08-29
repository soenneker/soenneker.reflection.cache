using System;
using Soenneker.Reflection.Cache.Attributes.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Reflection.Cache.Utils;
using System.Runtime.CompilerServices;

namespace Soenneker.Reflection.Cache.Attributes;

/// <inheritdoc cref="ICachedAttribute"/>
public sealed class CachedAttribute : ICachedAttribute
{
    public object Attribute { get; }

    public CachedType CachedType => GetCachedType();
    private ValueLazy<CachedType> _cachedType;
    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    public Type Type { get; }

    public string Name => Type.Name;

    public CachedAttribute(object attribute, CachedTypes cachedTypes, bool threadSafe = true)
    {
        Attribute = attribute;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;
        Type = attribute.GetType();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedType GetCachedType() =>
        _cachedType.GetOrCreatePublicationOnly(_threadSafe, this, static self => self._cachedTypes.GetCachedType(self.Type));
}
