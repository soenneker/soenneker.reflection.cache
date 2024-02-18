using System.Reflection;

namespace Soenneker.Reflection.Cache.Fields;

public class CachedField
{
    public FieldInfo FieldInfo { get; }

    public CachedField(FieldInfo fieldInfo)
    {
        FieldInfo = fieldInfo;
    }
}