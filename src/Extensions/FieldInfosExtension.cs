using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class FieldInfosExtension
{
    public static CachedField[] ToCachedFields(this FieldInfo[] fields, CachedTypes cachedTypes, bool threadSafe)
    {
        ReadOnlySpan<FieldInfo> fieldsArray = fields;

        var cachedFields = new CachedField[fieldsArray.Length];

        for (var i = 0; i < fields.Length; i++)
        {
            cachedFields[i] = new CachedField(fields[i], cachedTypes, threadSafe);
        }

        return cachedFields;
    }
}