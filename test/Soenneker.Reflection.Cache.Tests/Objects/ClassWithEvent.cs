using System.ComponentModel;

namespace Soenneker.Reflection.Cache.Tests.Objects;

public class ClassWithEvent
{
    public event PropertyChangedEventHandler PropertyChanged
    {
        add => PropertyChanged += value;
        remove => PropertyChanged -= value;
    }
}