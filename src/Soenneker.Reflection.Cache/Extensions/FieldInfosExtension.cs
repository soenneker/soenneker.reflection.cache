using System.Reflection;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides conversion helpers for field metadata.
/// </summary>
public static class FieldInfosExtension
{
    /// <summary>
    /// Creates cached field wrappers for the supplied metadata.
    /// </summary>
    /// <param name="fields">The field metadata to wrap.</param>
    /// <param name="cachedTypes">The type cache shared by the wrappers.</param>
    /// <param name="threadSafe">Whether lazily initialized wrapper state must be thread-safe.</param>
    /// <returns>The cached field wrappers in the original order.</returns>
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
