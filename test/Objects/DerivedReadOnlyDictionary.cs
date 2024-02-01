using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Soenneker.Reflection.Cache.Tests.Objects;

public class DerivedReadOnlyDictionary : ReadOnlyDictionary<string, object>
{
    public DerivedReadOnlyDictionary(IDictionary<string, object> dictionary) : base(dictionary)
    {
    }
}