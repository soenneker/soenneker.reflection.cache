using System;
using Soenneker.Reflection.Cache.Constructors.Abstract;

namespace Soenneker.Reflection.Cache.Constructors;

///<inheritdoc cref="ICachedConstructor"/>
public partial class CachedConstructor
{
    public bool IsStatic => _isStatic.Value;
    private Lazy<bool> _isStatic;

    public bool IsPublic => _isPublic.Value;
    private Lazy<bool> _isPublic;

    private void InitializeProperties()
    {
        _isStatic = new Lazy<bool>(() => ConstructorInfo is {IsStatic: true }, _threadSafe);
        _isPublic = new Lazy<bool>(() => ConstructorInfo is {IsPublic: true }, _threadSafe);
    }
}