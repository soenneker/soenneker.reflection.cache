using System.Collections.Frozen;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Fields;

internal sealed class CachedFieldsCache
{
    public readonly CachedField[] CachedArray;
    public readonly FrozenDictionary<string, CachedField> MapByName;
    public readonly FieldInfo[] FieldInfos;

    public CachedFieldsCache(CachedField[] cachedArray, FrozenDictionary<string, CachedField> mapByName, FieldInfo[] fieldInfos)
    {
        CachedArray = cachedArray;
        MapByName = mapByName;
        FieldInfos = fieldInfos;
    }
}

