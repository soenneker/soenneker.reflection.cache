namespace Soenneker.Reflection.Cache.Tests.Objects;

public class ClassWithGenericMethod
{
    public T GenericMethod<T>()
    {
        return default!;
    }
}