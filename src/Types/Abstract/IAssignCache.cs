using System;

namespace Soenneker.Reflection.Cache.Types.Abstract;

internal interface IAssignCache
{
    bool TryGet(RuntimeTypeHandle key, out bool value);
    void SetIfAbsent(RuntimeTypeHandle key, bool value);
}

