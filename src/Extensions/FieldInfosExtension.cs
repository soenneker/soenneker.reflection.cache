using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Fields;

namespace Soenneker.Reflection.Cache.Extensions;

public static class FieldInfosExtension
{
    public static CachedField[] ToCachedFields(this FieldInfo[] fields)
    {
        ReadOnlySpan<FieldInfo> fieldsArray = fields;

        var cachedFields = new CachedField[fieldsArray.Length];

        for (int i = 0; i < fields.Length; i++)
        {
            cachedFields[i] = new CachedField(fields[i]);
        }

        return cachedFields;
    }
}