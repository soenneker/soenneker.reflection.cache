using System;
using Soenneker.Extensions.Spans.Readonly.Types;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Represents the cached constructor extension.
/// </summary>
public static class CachedConstructorExtension
{
    /// <summary>
    /// Executes the to hash key operation.
    /// </summary>
    /// <param name="cachedConstructor">The cached constructor.</param>
    /// <returns>The result of the operation.</returns>
    public static int ToHashKey(this CachedConstructor cachedConstructor)
    {
        Type[] parameterTypes = cachedConstructor.GetParametersTypes();

        return parameterTypes.ToHashKey();
    }
}