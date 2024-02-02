using System.Collections;
using System.Collections.Generic;

namespace Soenneker.Reflection.Cache.Tests.Objects;

public class DerivedCollection : ICollection<string>
{
    public IEnumerator<string> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(string item)
    {
        throw new System.NotImplementedException();
    }

    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    public bool Contains(string item)
    {
        throw new System.NotImplementedException();
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
        throw new System.NotImplementedException();
    }

    public bool Remove(string item)
    {
        throw new System.NotImplementedException();
    }

    public int Count { get; }
    public bool IsReadOnly { get; }
}