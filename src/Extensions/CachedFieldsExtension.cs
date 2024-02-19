using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Fields;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedFieldsExtension
{
    public static FieldInfo[] ToFieldInfos(this CachedField[] cachedFields)
    {
        ReadOnlySpan<CachedField> span = cachedFields;

        var fieldInfos = new FieldInfo[cachedFields.Length];

        for (var i = 0; i < span.Length; i++)
        {
            fieldInfos[i] = span[i].FieldInfo;
        }

        return fieldInfos;
    }
}