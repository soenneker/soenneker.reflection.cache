using System;
using Soenneker.Reflection.Cache.Attributes.Abstract;

namespace Soenneker.Reflection.Cache.Attributes;

///<inheritdoc cref="ICachedAttribute"/>
public class CachedAttribute : ICachedAttribute
{
    public object Attribute { get; }

    // TODO: use cache?
    public Type AttributeType => Attribute.GetType();

    public CachedAttribute(object attribute)
    {
        Attribute = attribute;
    }
}