using System.Reflection;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

public static class FieldInfosExtension
{
    public static CachedField[] ToCachedFields(this FieldInfo[] fields, CachedTypes cachedTypes, bool threadSafe)
    {
        int length = fields.Length;
        var cachedFields = new CachedField[length]; 

        for (var i = 0; i < length; i++)
        {
            cachedFields[i] = new CachedField(fields[i], cachedTypes, threadSafe);
        }

        return cachedFields;
    }
}