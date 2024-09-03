using System;

namespace Soenneker.Reflection.Cache.Tests.Objects;

public class ClassWithDelegateProperty
{
    public Action DelegateProperty { get; set; }

    public int NonDelegateProperty { get; set; }
}