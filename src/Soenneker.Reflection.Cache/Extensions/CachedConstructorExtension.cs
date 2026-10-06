using System;
using Soenneker.Extensions.Spans.Readonly.Types;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

/// <summary>
/// Provides signature helpers for cached constructors.
/// </summary>
public static class CachedConstructorExtension
{
    /// <summary>
    /// Computes a hash key from the constructor parameter types.
    /// </summary>
    /// <param name="cachedConstructor">The cached constructor.</param>
    /// <returns>A hash representing the constructor signature.</returns>
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public static int ToHashKey(this CachedConstructor cachedConstructor)
    {
        Type[] parameterTypes = cachedConstructor.GetParametersTypes();

        return parameterTypes.ToHashKey();
    }
}
