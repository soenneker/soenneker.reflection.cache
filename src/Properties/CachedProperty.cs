using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Properties;

public class CachedProperty
{
    public PropertyInfo PropertyInfo { get; }

    public bool IsDelegate => _isDelegate.Value;
    private readonly Lazy<bool> _isDelegate;

    public CachedProperty(PropertyInfo propertyInfo, CachedTypes cachedTypes, bool threadSafe)
    {
        PropertyInfo = propertyInfo;

        _isDelegate = new Lazy<bool>(() => cachedTypes.GetCachedType(typeof(Delegate)).IsAssignableFrom(PropertyInfo.PropertyType), threadSafe);
    }
}
