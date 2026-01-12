using Soenneker.Reflection.Cache.Utils;
using System.Reflection;
using Soenneker.Extensions.MethodInfo;
using Soenneker.Extensions.Spans.Readonly.ParameterInfos;

namespace Soenneker.Reflection.Cache.Extensions;

internal static class MemberInfoExtension
{
    // TODO: I'm pretty sure this is broken in several situations
    internal static int ToHashKey(this MemberInfo memberInfo)
    {
        int result;

        if (memberInfo.MemberType == MemberTypes.Method)
        {
            var methodInfo = (MethodInfo)memberInfo;
            string originalName = methodInfo.ToOriginalMemberName();

            result = ReflectionCacheUtil.GetCacheKeyForMethod(originalName, methodInfo.GetParameters().ToTypes());
            return result;
        }

        result = memberInfo.Name.GetHashCode();
        return result;
    }
}