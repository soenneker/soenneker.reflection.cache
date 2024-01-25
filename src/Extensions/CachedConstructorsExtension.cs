using System.Reflection;
using Soenneker.Reflection.Cache.Constructors;

namespace Soenneker.Reflection.Cache.Extensions;

public static class CachedConstructorsExtension
{
    public static ConstructorInfo?[] ToConstructors(this CachedConstructor[] cachedConstructors)
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