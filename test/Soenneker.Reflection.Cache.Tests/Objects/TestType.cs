﻿using Soenneker.Reflection.Cache.Tests.Objects.Abstract;
using System;
using System.Runtime.Serialization;

namespace Soenneker.Reflection.Cache.Tests.Objects;

[Custom("Test")]
[DataContract]
public class TestType : ITestType
{
    public const string Locator = "Soenneker.Reflection.Cache.Tests.Objects.TestType, Soenneker.Reflection.Cache.Tests";

    private string _privateField;

    public int PublicField;

    [DataMember]
    public string PublicProperty1 { get; set; }

    [DataMember]
    public string PublicProperty2 { get; set; }

    private int? PrivateProperty1 { get; set; }

    private double? PrivateProperty2 { get; set; }

    public event EventHandler? MyEvent;

    protected internal int ProtectedInternalProperty { get; set; }

    internal int _internalField;

    public TestType()
    {
        // Initialize some values in the constructor
        PublicField = 42;
        _privateField = "PrivateFieldValue";
        PublicProperty1 = "PublicPropertyValue";
        PrivateProperty1 = 42;
        PrivateProperty2 = 3.14;
        ProtectedInternalProperty = 5;
        _internalField = 10;
    }

    public TestType(int publicField, string privateField, string publicProperty1, double privateProperty)
    {
        PublicField = publicField;
        _privateField = privateField;
        PublicProperty1 = publicProperty1;
        PrivateProperty2 = privateProperty;
    }

    public void PublicMethod1()
    {
        Console.WriteLine($"{nameof(PublicMethod1)} called");
    }

    public void PublicMethod2()
    {
        Console.WriteLine($"{nameof(PublicMethod2)} called");
    }

    public void PublicMethodWithParameters(int test)
    {
        Console.WriteLine($"{nameof(PublicMethodWithParameters)} called");
    }

    public void PublicMethodWithParameters(int test1, double test2)
    {
        Console.WriteLine($"{nameof(PublicMethodWithParameters)} called");
    }

    private void PrivateMethod()
    {
        Console.WriteLine("PrivateMethod called");
    }

    private static void PrivateStaticMethod()
    {
        Console.WriteLine("StaticPrivateMethod called");
    }

    public static void PublicStaticMethod()
    {
        Console.WriteLine("PublicStaticMethod called");
    }

    public string this[int index]
    {
        get { return $"Value at index {index}"; }
        set { Console.WriteLine($"Setting value at index {index}: {value}"); }
    }
}