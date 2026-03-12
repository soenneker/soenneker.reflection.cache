using System.Reflection;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class CachedConstructorsExtension
{
    public static ConstructorInfo?[] ToConstructorInfos(this CachedConstructor[] cachedConstructors)
    {
        int length = cachedConstructors.Length;
        var constructors = new ConstructorInfo?[length];

        for (var i = 0; i < length; i++)
        {
            constructors[i] = cachedConstructors[i].ConstructorInfo;
        }

        return constructors;
    }
}