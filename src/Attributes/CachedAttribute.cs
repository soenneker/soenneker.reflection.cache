using System;
using Soenneker.Reflection.Cache.Attributes.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Attributes;

///<inheritdoc cref="ICachedAttribute"/>
public sealed class CachedAttribute : ICachedAttribute
{
    public object Attribute { get; }

    public CachedType CachedType => _lazyCachedType.Value;
    private readonly Lazy<CachedType> _lazyCachedType;

    public Type Type => _typeLazy.Value;
    private readonly Lazy<Type> _typeLazy;

    public string Name => _lazyName.Value;
    private readonly Lazy<string> _lazyName;

    public CachedAttribute(object attribute, CachedTypes cachedTypes, bool threadSafe = true)
    {
        Attribute = attribute;

        _lazyCachedType = new Lazy<CachedType>(() => cachedTypes.GetCachedType(Attribute.GetType()), threadSafe);

        _typeLazy = new Lazy<Type>(() => _lazyCachedType.Value.Type!, threadSafe);

        _lazyName = new Lazy<string>(() => _typeLazy.Value.Name, threadSafe);
    }
}