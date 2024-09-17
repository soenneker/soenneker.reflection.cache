using System;

namespace Soenneker.Reflection.Cache.Tests.Objects.Abstract;

public interface ITestType
{
    string PublicProperty1 { get; set; }
    string PublicProperty2 { get; set; }

    void PublicMethod1();
    void PublicMethod2();

    event EventHandler MyEvent;

    string this[int index] { get; set; }
}