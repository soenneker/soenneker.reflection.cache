using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Utils.LazyBools;

namespace Soenneker.Reflection.Cache.Constructors;

/// <inheritdoc cref="ICachedConstructor"/>
public sealed partial class CachedConstructor
{
    private int _isStatic;
    private int _isPublic;

    public bool IsStatic =>
        LazyBoolUtil.GetOrInit(ref _isStatic, _threadSafe, this, static self => self.ConstructorInfo is { IsStatic: true });

    public bool IsPublic =>
        LazyBoolUtil.GetOrInit(ref _isPublic, _threadSafe, this, static self => self.ConstructorInfo is { IsPublic: true });
}