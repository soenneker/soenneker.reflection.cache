using System.Reflection;

namespace Soenneker.Reflection.Cache.Properties;

public class CachedProperty
{
    public PropertyInfo PropertyInfo { get; }

    public CachedProperty(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
    }
}
