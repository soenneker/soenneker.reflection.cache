using System.Collections;
using System.Collections.Generic;

namespace Soenneker.Reflection.Cache.Tests.Objects;

public class DerivedEnumerable : IEnumerable<string>
{
    public IEnumerator<string> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}