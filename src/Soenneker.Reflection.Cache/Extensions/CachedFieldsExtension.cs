using System.Reflection;
using Soenneker.Reflection.Cache.Fields;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides conversions for arrays of cached fields.
/// </summary>
public static class CachedFieldsExtension
{
    /// <summary>
    /// Extracts the underlying field metadata.
    /// </summary>
    /// <param name="cachedFields">The cached fields.</param>
    /// <returns>The <see cref="FieldInfo"/> values in the original order.</returns>
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
