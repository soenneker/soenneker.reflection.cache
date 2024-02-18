using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Fields;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedFieldsExtension
{
    public static FieldInfo[] ToFieldInfos(this CachedField[] cachedField)
    {
        ReadOnlySpan<CachedField> span = cachedField;

        var fieldInfos = new FieldInfo[cachedField.Length];

        for (var i = 0; i < span.Length; i++)
        {
            fieldInfos[i] = span[i].FieldInfo;
        }

        return fieldInfos;
    }
}