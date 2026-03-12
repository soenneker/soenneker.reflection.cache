using Soenneker.Reflection.Cache.Properties.Abstract;
using Soenneker.Reflection.Cache.Types;
using Soenneker.Utils.LazyBools;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Properties;

/// <inheritdoc cref="ICachedProperty"/>
public sealed class CachedProperty : ICachedProperty
{
    public PropertyInfo PropertyInfo { get; }

    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    private int _isDelegate;

    public bool IsDelegate =>
        LazyBoolUtil.GetOrInit(ref _isDelegate, _threadSafe, this, static self => self._cachedTypes.GetCachedType(typeof(Delegate))
                                                                                      .IsAssignableFrom(self.PropertyInfo.PropertyType));

    private int _isCompilerGenerated;

    public bool IsCompilerGenerated =>
        LazyBoolUtil.GetOrInit(ref _isCompilerGenerated, _threadSafe, this,
            static self => Attribute.IsDefined(self.PropertyInfo, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute)));

    public bool IsEqualityContract { get; }
    public bool IsStatic { get; }
    public bool IsVirtual { get; }
    public bool IsIndexer { get; }
    public bool IsReadOnly { get; }
    public bool IsPublic { get; }
    public bool IsProtected { get; }
    public bool IsPrivate { get; }

    public CachedProperty(PropertyInfo propertyInfo, CachedTypes cachedTypes, bool threadSafe)
    {
        PropertyInfo = propertyInfo;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;

        // Cheap checks are evaluated once during construction.
        MethodInfo? getMethod = PropertyInfo.GetMethod;
        MethodInfo? setMethod = PropertyInfo.SetMethod;

        IsEqualityContract = PropertyInfo.Name == "EqualityContract";
        IsStatic = getMethod?.IsStatic ?? false;
        IsVirtual = getMethod?.IsVirtual ?? false;
        IsIndexer = PropertyInfo.GetIndexParameters()
                                .Length > 0;
        IsReadOnly = setMethod == null;
        IsPublic = getMethod?.IsPublic == true;
        IsProtected = getMethod?.IsFamily == true;
        IsPrivate = getMethod?.IsPrivate == true && (setMethod == null || setMethod.IsPrivate);
    }
}