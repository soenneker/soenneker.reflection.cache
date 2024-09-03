using System;
using System.Reflection;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Fields;

public class CachedField
{
    public FieldInfo FieldInfo { get; }

    public bool IsDelegate => _isDelegate.Value;
    private readonly Lazy<bool> _isDelegate;

    public CachedField(FieldInfo fieldInfo, CachedTypes cachedTypes, bool threadSafe)
    {
        FieldInfo = fieldInfo;

        _isDelegate = new Lazy<bool>(() => cachedTypes.GetCachedType(typeof(Delegate)).IsAssignableFrom(FieldInfo.FieldType), threadSafe);
    }
}