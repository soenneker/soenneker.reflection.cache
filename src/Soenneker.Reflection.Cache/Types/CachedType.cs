using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Arguments;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Interfaces;
using Soenneker.Reflection.Cache.Members;
using Soenneker.Reflection.Cache.Methods;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types.Abstract;
using Soenneker.Reflection.Cache.Utils;

namespace Soenneker.Reflection.Cache.Types;

/// <inheritdoc cref="ICachedType"/>
public partial class CachedType : ICachedType
{
    public Type? Type { get; }

    public Type? BaseType => Type?.BaseType;

    public CachedType? CachedBaseType { [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires preserved members and statically available generic code.")] get => GetCachedBaseType(); }

    private ValueNullableLazy<CachedType> _cachedBaseType;

    public int? CacheKey => Type?.GetHashCode();

    private ValueLazy<CachedProperties> _cachedProperties;
    private ValueLazy<CachedMethods> _cachedMethods;
    private ValueLazy<CachedFields> _cachedFields;
    private ValueLazy<CachedCustomAttributes> _cachedAttributes;
    private ValueLazy<CachedInterfaces> _cachedInterfaces;
    private ValueLazy<CachedConstructors> _cachedConstructors;
    private ValueLazy<CachedMembers> _cachedMembers;
    private ValueLazy<CachedGenericArguments> _cachedGenericArguments;
    private ValueLazy<CachedGenericTypeDefinition> _cachedGenericTypeDefinition;
    private ValueLazy<CachedIsAssignableFrom> _cachedIsAssignableFrom;
    private ValueLazy<CachedMakeGenericType> _cachedMakeGenericType;
    private ValueLazy<CachedGetElementType> _cachedGetElementType;
    private ValueAtomicLock _initializationLock;

    private readonly bool _threadSafe;
    private readonly CachedTypes _cachedTypes;

    public CachedType(Type? type, CachedTypes cachedTypes, bool threadSafe = true)
    {
        Type = type;
        _cachedTypes = cachedTypes;
        _threadSafe = threadSafe;

    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedType? GetCachedBaseType() =>
        _cachedBaseType.GetOrCreatePublicationOnly(_threadSafe, this,
            static self => self.Type?.BaseType is { } baseType ? self._cachedTypes.GetCachedType(baseType) : null);

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedProperties GetPropertiesCache() =>
        _cachedProperties.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedProperties(self, self._cachedTypes, self._threadSafe));

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedMethods GetMethodsCache() =>
        _cachedMethods.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedMethods(self, self._cachedTypes, self._threadSafe));

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedFields GetFieldsCache() =>
        _cachedFields.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedFields(self, self._cachedTypes, self._threadSafe));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedCustomAttributes GetAttributesCache() =>
        _cachedAttributes.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedCustomAttributes(self, self._cachedTypes, self._threadSafe));

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedInterfaces GetInterfacesCache() =>
        _cachedInterfaces.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedInterfaces(self, self._cachedTypes, self._threadSafe));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedConstructors GetConstructorsCache() =>
        _cachedConstructors.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedConstructors(self, self._cachedTypes, self._threadSafe));

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedMembers GetMembersCache() =>
        _cachedMembers.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedMembers(self, self._cachedTypes, self._threadSafe));

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedGenericArguments GetGenericArgumentsCache() =>
        _cachedGenericArguments.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedGenericArguments(self, self._cachedTypes, self._threadSafe));

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedGenericTypeDefinition GetGenericTypeDefinitionCache() =>
        _cachedGenericTypeDefinition.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedGenericTypeDefinition(self, self._cachedTypes, self._threadSafe));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedIsAssignableFrom GetIsAssignableFromCache() =>
        _cachedIsAssignableFrom.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedIsAssignableFrom(self, self._threadSafe));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedMakeGenericType GetMakeGenericTypeCache() =>
        _cachedMakeGenericType.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedMakeGenericType(self, self._cachedTypes, self._threadSafe));

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CachedGetElementType GetElementTypeCache() =>
        _cachedGetElementType.GetOrCreate(_threadSafe, ref _initializationLock, this,
            static self => new CachedGetElementType(self, self._cachedTypes, self._threadSafe));

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public PropertyInfo? GetProperty(string property)
    {
        if (Type == null)
            return null;

        return GetPropertiesCache().GetProperty(property);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedProperty? GetCachedProperty(string property)
    {
        if (Type == null)
            return null;

        return GetPropertiesCache().GetCachedProperty(property);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public PropertyInfo[]? GetProperties()
    {
        if (Type == null)
            return null;

        return GetPropertiesCache().GetProperties();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedProperty[]? GetCachedProperties()
    {
        if (Type == null)
            return null;

        return GetPropertiesCache().GetCachedProperties();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedMethod? GetCachedMethod(string methodName)
    {
        if (Type == null)
            return null;

        return GetMethodsCache().GetCachedMethod(methodName);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedMethod? GetCachedMethod(string methodName, Type[] parameters)
    {
        if (Type == null)
            return null;

        return GetMethodsCache().GetCachedMethod(methodName, parameters);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedMethod? GetCachedMethod(string methodName, CachedType[] parameters)
    {
        if (Type == null)
            return null;

        return GetMethodsCache().GetCachedMethod(methodName, parameters);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedField[]? GetCachedFields()
    {
        if (Type == null)
            return null;

        CachedField[] result = GetFieldsCache().GetCachedFields();

        return result;
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public FieldInfo[]? GetFields()
    {
        if (Type == null)
            return null;

        FieldInfo[] result = GetFieldsCache().GetFields();
        
        return result;
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedField? GetCachedField(string fieldName)
    {
        if (Type == null)
            return null;

        return GetFieldsCache().GetCachedField(fieldName);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public FieldInfo? GetField(string fieldName)
    {
        if (Type == null)
            return null;

        return GetFieldsCache().GetField(fieldName);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public MethodInfo? GetMethod(string methodName)
    {
        if (Type == null)
            return null;

        return GetMethodsCache().GetMethod(methodName);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public MethodInfo? GetMethod(string methodName, Type[] parameterTypes)
    {
        if (Type == null)
            return null;

        return GetMethodsCache().GetMethod(methodName, parameterTypes);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedMethod[]? GetCachedMethods()
    {
        if (Type == null)
            return null;

        return GetMethodsCache().GetCachedMethods();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public MethodInfo?[]? GetMethods()
    {
        if (Type == null)
            return null;

        return GetMethodsCache().GetMethods();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? GetCachedInterface(string typeName)
    {
        if (Type == null)
            return null;

        return GetInterfacesCache().GetCachedInterface(typeName);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType[]? GetCachedInterfaces()
    {
        if (Type == null)
            return null;

        return GetInterfacesCache().GetCachedInterfaces();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? GetInterface(string typeName)
    {
        if (Type == null)
            return null;

        return GetInterfacesCache().GetInterface(typeName);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type[]? GetInterfaces()
    {
        if (Type == null)
            return null;

        return GetInterfacesCache().GetInterfaces();
    }

    public CachedAttribute[]? GetCachedCustomAttributes()
    {
        if (Type == null)
            return null;

        return GetAttributesCache().GetCachedCustomAttributes();
    }

    public object[]? GetCustomAttributes()
    {
        if (Type == null)
            return null;

        return GetAttributesCache().GetCustomAttributes();
    }

    public T? GetCachedCustomAttribute<T>(bool inherit = true) where T : Attribute
    {
        if (Type == null)
            return null;

        return GetAttributesCache().GetCachedCustomAttribute<T>(inherit);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedConstructor? GetCachedConstructor(Type[] parameterTypes)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetCachedConstructor(parameterTypes);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedConstructor? GetCachedConstructor(Type t0)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetCachedConstructor(t0);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetCachedConstructor(t0, t1);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetCachedConstructor(t0, t1, t2);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedConstructor? GetCachedConstructor(Type t0, Type t1, Type t2, Type t3)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetCachedConstructor(t0, t1, t2, t3);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public ConstructorInfo? GetConstructor(Type[]? parameterTypes = null)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetConstructor(parameterTypes);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public ConstructorInfo? GetConstructor(Type t0)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetConstructor(t0);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public ConstructorInfo? GetConstructor(Type t0, Type t1)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetConstructor(t0, t1);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetConstructor(t0, t1, t2);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public ConstructorInfo? GetConstructor(Type t0, Type t1, Type t2, Type t3)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetConstructor(t0, t1, t2, t3);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedConstructor[]? GetCachedConstructors()
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetCachedConstructors();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public ConstructorInfo?[]? GetConstructors()
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().GetConstructors();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public object? CreateInstance()
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().CreateInstance();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public T? CreateInstance<T>()
    {
        if (Type == null)
            return default;

        return GetConstructorsCache().CreateInstance<T>();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public object? CreateInstance(params object[] parameters)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().CreateInstance(parameters);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public T? CreateInstance<T>(params object[] parameters)
    {
        if (Type == null)
            return default;

        return GetConstructorsCache().CreateInstance<T>(parameters);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public object? CreateInstance(object? arg0)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().CreateInstance(arg0);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public object? CreateInstance(object? arg0, object? arg1)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().CreateInstance(arg0, arg1);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public object? CreateInstance(object? arg0, object? arg1, object? arg2)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().CreateInstance(arg0, arg1, arg2);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public object? CreateInstance(object? arg0, object? arg1, object? arg2, object? arg3)
    {
        if (Type == null)
            return null;

        return GetConstructorsCache().CreateInstance(arg0, arg1, arg2, arg3);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public T? CreateInstance<T>(object? arg0)
    {
        object? obj = CreateInstance(arg0);
        return obj is null ? default : (T?) obj;
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public T? CreateInstance<T>(object? arg0, object? arg1)
    {
        object? obj = CreateInstance(arg0, arg1);
        return obj is null ? default : (T?) obj;
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public T? CreateInstance<T>(object? arg0, object? arg1, object? arg2)
    {
        object? obj = CreateInstance(arg0, arg1, arg2);
        return obj is null ? default : (T?) obj;
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public T? CreateInstance<T>(object? arg0, object? arg1, object? arg2, object? arg3)
    {
        object? obj = CreateInstance(arg0, arg1, arg2, arg3);
        return obj is null ? default : (T?) obj;
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? GetCachedGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        return GetGenericTypeDefinitionCache().GetCachedGenericTypeDefinition();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? GetGenericTypeDefinition()
    {
        if (Type == null)
            return null;

        return GetGenericTypeDefinitionCache().GetGenericTypeDefinition();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType[]? GetCachedGenericArguments()
    {
        if (Type == null)
            return null;

        return GetGenericArgumentsCache().GetCachedGenericArguments();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type[]? GetGenericArguments()
    {
        if (Type == null)
            return null;

        return GetGenericArgumentsCache().GetGenericArguments();
    }

    //public CachedMember? GetCachedMember(string name)
    //{
    //    if (Type == null)
    //        return null;

    //    return _cachedMembers!.Value.GetCachedMember(name);
    //}

    //public MemberInfo? GetMember(string name)
    //{
    //    if (Type == null)
    //        return null;

    //    return _cachedMembers!.Value.GetMember(name);
    //}

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedMember[]? GetCachedMembers()
    {
        if (Type == null)
            return null;

        return GetMembersCache().GetCachedMembers();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public MemberInfo[]? GetMembers()
    {
        if (Type == null)
            return null;

        return GetMembersCache().GetMembers();
    }

    public bool IsAssignableFrom(Type derivedType)
    {
        if (Type == null)
            return false;

        return GetIsAssignableFromCache().IsAssignableFrom(derivedType);
    }

    public bool IsAssignableFrom(CachedType cachedDerivedType)
    {
        if (Type == null)
            return false;

        return GetIsAssignableFromCache().IsAssignableFrom(cachedDerivedType);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(params Type[] typeArguments)
    {
        return GetMakeGenericTypeCache().MakeGenericCachedType(typeArguments);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(params CachedType[] typeArguments)
    {
        return GetMakeGenericTypeCache().MakeGenericCachedType(typeArguments);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(Type t0) => GetMakeGenericTypeCache().MakeGenericCachedType(t0);

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(Type t0, Type t1) => GetMakeGenericTypeCache().MakeGenericCachedType(t0, t1);

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(Type t0, Type t1, Type t2) => GetMakeGenericTypeCache().MakeGenericCachedType(t0, t1, t2);

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(Type t0, Type t1, Type t2, Type t3) => GetMakeGenericTypeCache().MakeGenericCachedType(t0, t1, t2, t3);

    // ---- allocation-reducing overloads (avoid params CachedType[] allocations) ----

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(CachedType t0)
    {
        return GetMakeGenericTypeCache().MakeGenericCachedType(t0);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(CachedType t0, CachedType t1)
    {
        return GetMakeGenericTypeCache().MakeGenericCachedType(t0, t1);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(CachedType t0, CachedType t1, CachedType t2)
    {
        return GetMakeGenericTypeCache().MakeGenericCachedType(t0, t1, t2);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? MakeCachedGenericType(CachedType t0, CachedType t1, CachedType t2, CachedType t3)
    {
        return GetMakeGenericTypeCache().MakeGenericCachedType(t0, t1, t2, t3);
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? MakeGenericType(params Type[] typeArguments)
    {
        return GetMakeGenericTypeCache().MakeGenericType(typeArguments);
    }

    // ---- allocation-reducing overloads (avoid params Type[] allocations) ----

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? MakeGenericType(Type t0) => GetMakeGenericTypeCache().MakeGenericType(t0);

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? MakeGenericType(Type t0, Type t1) => GetMakeGenericTypeCache().MakeGenericType(t0, t1);

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? MakeGenericType(Type t0, Type t1, Type t2) => GetMakeGenericTypeCache().MakeGenericType(t0, t1, t2);

    [global::System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Runtime reflection can require generic instantiations unavailable with Native AOT. Use statically registered types and delegates instead.")]
    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? MakeGenericType(Type t0, Type t1, Type t2, Type t3) => GetMakeGenericTypeCache().MakeGenericType(t0, t1, t2, t3);

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public CachedType? GetCachedElementType()
    {
        return GetElementTypeCache().GetCachedElementType();
    }

    [global::System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Runtime reflection requires members that trimming may remove. Use statically registered metadata instead.")]
    public Type? GetElementType()
    {
        return GetElementTypeCache().GetElementType();
    }

    public override string ToString()
    {
        return Type == null ? "null" : Type.Name;
    }
}
