using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class CachedConstructorsExtension
{
    public static ConstructorInfo?[] ToConstructorInfos(this CachedConstructor[] cachedConstructors)
    {
        ReadOnlySpan<CachedConstructor> span = cachedConstructors;

        var constructors = new ConstructorInfo?[cachedConstructors.Length];

        for (var i = 0; i < span.Length; i++)
        {
            constructors[i] = span[i].ConstructorInfo;
        }

        return constructors;
    }
}