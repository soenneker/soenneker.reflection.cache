using System.Reflection;
using Soenneker.Reflection.Cache.Fields;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the cached fields extension.
/// </summary>
public static class CachedFieldsExtension
{
    /// <summary>
    /// Executes the to field infos operation.
    /// </summary>
    /// <param name="cachedFields">The cached fields.</param>
    /// <returns>The result of the operation.</returns>
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