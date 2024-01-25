using System;

namespace Soenneker.Reflection.Cache.Extensions;

public static class ObjectExtension
{
    public static Type[] ToTypes(this object[] objects)
    {
        var parameterTypes = new Type[objects.Length];

        for (var i = 0; i < objects.Length; i++)
        {
            parameterTypes[i] = objects[i].GetType();
        }

        return parameterTypes;
    }
}