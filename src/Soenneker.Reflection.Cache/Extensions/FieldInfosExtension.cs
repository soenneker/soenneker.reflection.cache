using System.Reflection;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the field infos extension.
/// </summary>
public static class FieldInfosExtension
{
    /// <summary>
    /// Executes the to cached fields operation.
    /// </summary>
    /// <param name="fields">The fields.</param>
    /// <param name="cachedTypes">The cached types.</param>
    /// <param name="threadSafe">The thread safe.</param>
    /// <returns>The result of the operation.</returns>
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