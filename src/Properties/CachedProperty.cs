using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Properties.Abstract;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Properties;

public sealed class CachedProperty : ICachedProperty
{
    public PropertyInfo PropertyInfo { get; }

    public bool IsDelegate => _isDelegate.Value;
    private readonly Lazy<bool> _isDelegate;

    public bool IsEqualityContract => _isEqualityContract.Value;
    private readonly Lazy<bool> _isEqualityContract;

    public bool IsStatic => _isStatic.Value;
    private readonly Lazy<bool> _isStatic;

    public bool IsVirtual => _isVirtual.Value;
    private readonly Lazy<bool> _isVirtual;

    public bool IsIndexer => _isIndexer.Value;
    private readonly Lazy<bool> _isIndexer;

    public bool IsCompilerGenerated => _isCompilerGenerated.Value;
    private readonly Lazy<bool> _isCompilerGenerated;

    public bool IsReadOnly => _isReadOnly.Value;
    private readonly Lazy<bool> _isReadOnly;

    public bool IsPublic => _isPublic.Value;
    private readonly Lazy<bool> _isPublic;

    public bool IsProtected => _isProtected.Value;
    private readonly Lazy<bool> _isProtected;

    public bool IsPrivate => _isPrivate.Value;
    private readonly Lazy<bool> _isPrivate;

    public CachedProperty(PropertyInfo propertyInfo, CachedTypes cachedTypes, bool threadSafe)
    {
        PropertyInfo = propertyInfo;

        _isDelegate = new Lazy<bool>(() => cachedTypes.GetCachedType(typeof(Delegate)).IsAssignableFrom(PropertyInfo.PropertyType), threadSafe);

        _isEqualityContract = new Lazy<bool>(() => PropertyInfo.Name == "EqualityContract", threadSafe);

        _isStatic = new Lazy<bool>(() => PropertyInfo.GetMethod?.IsStatic ?? false, threadSafe);

        _isVirtual = new Lazy<bool>(() => PropertyInfo.GetMethod?.IsVirtual ?? false, threadSafe);

        _isIndexer = new Lazy<bool>(() => PropertyInfo.GetIndexParameters().Length > 0, threadSafe);

        _isCompilerGenerated = new Lazy<bool>(() => Attribute.IsDefined(PropertyInfo, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute)), threadSafe);

        _isReadOnly = new Lazy<bool>(() => PropertyInfo.SetMethod == null, threadSafe);

        _isPublic = new Lazy<bool>(() => PropertyInfo.GetMethod?.IsPublic == true, threadSafe);

        _isProtected = new Lazy<bool>(() => PropertyInfo.GetMethod?.IsFamily == true, threadSafe);

        _isPrivate = new Lazy<bool>(() =>
                PropertyInfo.GetMethod?.IsPrivate == true && (PropertyInfo.SetMethod == null || PropertyInfo.SetMethod?.IsPrivate == true),
            threadSafe);
    }
}