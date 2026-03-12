using System.Reflection;
using Soenneker.Reflection.Cache.Fields;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedFieldsExtension
{
    public static FieldInfo[] ToFieldInfos(this CachedField[] cachedFields)
    {
        int length = cachedFields.Length;
        var fieldInfos = new FieldInfo[length];

        for (var i = 0; i < length; i++)
        {
            fieldInfos[i] = cachedFields[i].FieldInfo;
        }

        return fieldInfos;
    }
}