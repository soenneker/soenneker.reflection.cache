using Soenneker.Reflection.Cache.Types;
using Soenneker.Utils.LazyBools;
using System;
using System.Reflection;

namespace Soenneker.Reflection.Cache.Fields;

public sealed class CachedField
{
    public FieldInfo FieldInfo { get; }
    
    private readonly CachedTypes _cachedTypes;
    private readonly bool _threadSafe;

    private int _isDelegate;

    public bool IsDelegate =>
        LazyBoolUtil.GetOrInit(
            ref _isDelegate,
            _threadSafe,
            this,
            static self =>
                self._cachedTypes
                    .GetCachedType(typeof(Delegate))
                    .IsAssignableFrom(self.FieldInfo.FieldType));

    public CachedField(FieldInfo fieldInfo, CachedTypes cachedTypes, bool threadSafe)
    {
        FieldInfo = fieldInfo;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;
    }
}