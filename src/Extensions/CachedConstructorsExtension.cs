using System.Reflection;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class CachedConstructorsExtension
{
    public static ConstructorInfo?[] ToConstructorInfos(this CachedConstructor[] cachedConstructors)
    {
        int count = cachedConstructors.Length;

        var constructors = new ConstructorInfo?[count];

        for (var i = 0; i < count; i++)
        {
            constructors[i] = cachedConstructors[i].ConstructorInfo;
        }

        return constructors;
    }
}