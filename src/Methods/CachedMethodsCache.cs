using System.Collections.Frozen;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Methods;

internal sealed class CachedMethodsCache
{
    public readonly CachedMethod[] Methods;
    public readonly FrozenDictionary<string, CachedMethod[]> MethodsByName;
    public readonly MethodInfo?[] MethodInfos;

    public CachedMethodsCache(CachedMethod[] methods, FrozenDictionary<string, CachedMethod[]> methodsByName, MethodInfo?[] infos)
    {
        Methods = methods;
        MethodsByName = methodsByName;
        MethodInfos = infos;
    }
}