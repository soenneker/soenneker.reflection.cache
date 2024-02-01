using Soenneker.Reflection.Cache.Utils;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class MemberInfoExtension
{
    // TODO: I'm pretty sure this is broken in several situations
    internal static int ToCacheKey(this MemberInfo memberInfo)
    {
        int result;

        if (memberInfo.MemberType == MemberTypes.Method)
        {
            var methodInfo = (MethodInfo)memberInfo;
            string originalName = methodInfo.ToOriginalMemberName();

            result = ReflectionCacheUtil.GetCacheKeyForMethod(originalName, methodInfo.GetParameters().ToParametersTypes());
            return result;
        }

        result = memberInfo.Name.GetHashCode();
        return result;
    }


}