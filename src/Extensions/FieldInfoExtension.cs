using System.Diagnostics.Contracts;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

public static class FieldInfoExtension
{
    [Pure]
    public static bool IsConstant(this FieldInfo field)
    {
        return field.IsLiteral && !field.IsInitOnly;
    }
}