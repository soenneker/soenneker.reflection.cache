using System;
using Soenneker.Reflection.Cache.Attributes.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Attributes;

///<inheritdoc cref="ICachedAttribute"/>
public sealed class CachedAttribute : ICachedAttribute
{
    public object Attribute { get; }

    public Type AttributeType => _attributeTypeLazy.Value;
    private readonly Lazy<Type> _attributeTypeLazy;

    public CachedType CachedAttributeType => _lazyCachedAttributeType.Value;
    private readonly Lazy<CachedType> _lazyCachedAttributeType;

    public CachedAttribute(object attribute, CachedTypes cachedTypes, bool threadSafe = true)
    {
        Attribute = attribute;

        _lazyCachedAttributeType = new Lazy<CachedType>(() => cachedTypes.GetCachedType(AttributeType), threadSafe);

        _attributeTypeLazy = new Lazy<Type>(() => _lazyCachedAttributeType.Value.Type!, threadSafe);
    }
}