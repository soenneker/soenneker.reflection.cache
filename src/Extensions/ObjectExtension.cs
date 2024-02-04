using System;

namespace Soenneker.Reflection.Cache.Extensions;

public static class ObjectExtension
{
    public static Type[] ToTypes(this object[] objects)
    {
        ReadOnlySpan<object> span = objects;

        var parameterTypes = new Type[span.Length];

        for (var i = 0; i < span.Length; i++)
        {
            parameterTypes[i] = span[i].GetType();
        }

        return parameterTypes;
    }
}