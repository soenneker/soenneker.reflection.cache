using System;

namespace Soenneker.Reflection.Cache.Tests.Objects;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class CustomAttribute : Attribute
{
    public string Description { get; }

    public CustomAttribute(string description)
    {
        Description = description;
    }
}