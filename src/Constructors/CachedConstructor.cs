using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Soenneker.Reflection.Cache.Attributes;
using Soenneker.Reflection.Cache.Constructors.Abstract;
using Soenneker.Reflection.Cache.Parameters;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Constructors;

///<inheritdoc cref="ICachedConstructor"/>
public sealed partial class CachedConstructor : ICachedConstructor
{
    public ConstructorInfo? ConstructorInfo { get; }

    private readonly Lazy<CachedCustomAttributes>? _attributes;

    private readonly Lazy<CachedParameters>? _parameters;

    private readonly bool _threadSafe;

    // Arity-specialized invokers avoid params object[] allocations for common cases (1..4 args).
    private readonly Lazy<Func<object?, object?>?>? _invoke1;
    private readonly Lazy<Func<object?, object?, object?>?>? _invoke2;
    private readonly Lazy<Func<object?, object?, object?, object?>?>? _invoke3;
    private readonly Lazy<Func<object?, object?, object?, object?, object?>?>? _invoke4;

    public CachedConstructor(ConstructorInfo? constructorInfo, CachedTypes cachedTypes, bool threadSafe = true)
    {
        ConstructorInfo = constructorInfo;
        _threadSafe = threadSafe;

        InitializeProperties();

        if (constructorInfo == null)
            return;

        _attributes = new Lazy<CachedCustomAttributes>(() => new CachedCustomAttributes(this, cachedTypes, threadSafe), threadSafe);
        _parameters = new Lazy<CachedParameters>(() => new CachedParameters(this, cachedTypes, threadSafe), threadSafe);

        _invoke1 = new Lazy<Func<object?, object?>?>(() => BuildCtorInvoker1(constructorInfo), threadSafe);
        _invoke2 = new Lazy<Func<object?, object?, object?>?>(() => BuildCtorInvoker2(constructorInfo), threadSafe);
        _invoke3 = new Lazy<Func<object?, object?, object?, object?>?>(() => BuildCtorInvoker3(constructorInfo), threadSafe);
        _invoke4 = new Lazy<Func<object?, object?, object?, object?, object?>?>(() => BuildCtorInvoker4(constructorInfo), threadSafe);
    }

    public CachedParameter[] GetCachedParameters()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetCachedParameters();
    }

    public ParameterInfo[] GetParameters()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetParameters();
    }

    public CachedAttribute[] GetCachedCustomAttributes()
    {
        if (ConstructorInfo == null)
            return [];

        return _attributes!.Value.GetCachedCustomAttributes();
    }

    public object[] GetCustomAttributes()
    {
        if (ConstructorInfo == null)
            return [];

        return _attributes!.Value.GetCustomAttributes();
    }

    public Type[] GetParametersTypes()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetParameterTypes();
    }

    public CachedType[] GetCachedParameterTypes()
    {
        if (ConstructorInfo == null)
            return [];

        return _parameters!.Value.GetCachedParameterTypes();
    }

    public object? Invoke()
    {
        if (ConstructorInfo == null)
            return null;

        return ConstructorInfo.Invoke(null);
    }

    public object? Invoke(params object[] param)
    {
        if (ConstructorInfo == null)
            return null;

        return ConstructorInfo.Invoke(param);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object? arg0)
    {
        if (ConstructorInfo is null)
            return null;

        Func<object?, object?>? f = _invoke1!.Value;
        if (f is not null)
            return f(arg0);

        return ConstructorInfo.Invoke([arg0]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object? arg0, object? arg1)
    {
        if (ConstructorInfo is null)
            return null;

        Func<object?, object?, object?>? f = _invoke2!.Value;
        if (f is not null)
            return f(arg0, arg1);

        return ConstructorInfo.Invoke([arg0, arg1]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object? arg0, object? arg1, object? arg2)
    {
        if (ConstructorInfo is null)
            return null;

        Func<object?, object?, object?, object?>? f = _invoke3!.Value;
        if (f is not null)
            return f(arg0, arg1, arg2);

        return ConstructorInfo.Invoke([arg0, arg1, arg2]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object? Invoke(object? arg0, object? arg1, object? arg2, object? arg3)
    {
        if (ConstructorInfo is null)
            return null;

        Func<object?, object?, object?, object?, object?>? f = _invoke4!.Value;
        if (f is not null)
            return f(arg0, arg1, arg2, arg3);

        return ConstructorInfo.Invoke([arg0, arg1, arg2, arg3]);
    }

    public T? Invoke<T>()
    {
        return (T?) Invoke();
    }

    public T? Invoke<T>(params object[] param)
    {
        return (T?) Invoke(param);
    }

    public T? Invoke<T>(object? arg0) => (T?) Invoke(arg0);

    public T? Invoke<T>(object? arg0, object? arg1) => (T?) Invoke(arg0, arg1);

    public T? Invoke<T>(object? arg0, object? arg1, object? arg2) => (T?) Invoke(arg0, arg1, arg2);

    public T? Invoke<T>(object? arg0, object? arg1, object? arg2, object? arg3) => (T?) Invoke(arg0, arg1, arg2, arg3);

    // ---------- invoker builders ----------

    private static bool IsByRefLikeOrByRef(Type t)
    {
        if (t.IsByRef)
            return true;

        // Keep it conservative; span-like can't be boxed and expression compilation will fail.
        string? fullName = t.FullName;
        return fullName is not null && (fullName.StartsWith("System.Span`1", StringComparison.Ordinal) ||
                                        fullName.StartsWith("System.ReadOnlySpan`1", StringComparison.Ordinal));
    }

    private static Func<object?, object?>? BuildCtorInvoker1(ConstructorInfo ctor)
    {
        try
        {
            ParameterInfo[] ps = ctor.GetParameters();
            if (ps.Length != 1 || IsByRefLikeOrByRef(ps[0].ParameterType))
                return null;

            ParameterExpression a0 = Expression.Parameter(typeof(object), "a0");
            UnaryExpression arg0 = Expression.Convert(a0, ps[0].ParameterType);
            NewExpression newExpr = Expression.New(ctor, arg0);
            Expression body = Expression.Convert(newExpr, typeof(object));
            return Expression.Lambda<Func<object?, object?>>(body, a0).Compile();
        }
        catch
        {
            return null;
        }
    }

    private static Func<object?, object?, object?>? BuildCtorInvoker2(ConstructorInfo ctor)
    {
        try
        {
            ParameterInfo[] ps = ctor.GetParameters();
            if (ps.Length != 2 || IsByRefLikeOrByRef(ps[0].ParameterType) || IsByRefLikeOrByRef(ps[1].ParameterType))
                return null;

            ParameterExpression a0 = Expression.Parameter(typeof(object), "a0");
            ParameterExpression a1 = Expression.Parameter(typeof(object), "a1");
            UnaryExpression arg0 = Expression.Convert(a0, ps[0].ParameterType);
            UnaryExpression arg1 = Expression.Convert(a1, ps[1].ParameterType);
            NewExpression newExpr = Expression.New(ctor, arg0, arg1);
            Expression body = Expression.Convert(newExpr, typeof(object));
            return Expression.Lambda<Func<object?, object?, object?>>(body, a0, a1).Compile();
        }
        catch
        {
            return null;
        }
    }

    private static Func<object?, object?, object?, object?>? BuildCtorInvoker3(ConstructorInfo ctor)
    {
        try
        {
            ParameterInfo[] ps = ctor.GetParameters();
            if (ps.Length != 3 || IsByRefLikeOrByRef(ps[0].ParameterType) || IsByRefLikeOrByRef(ps[1].ParameterType) ||
                IsByRefLikeOrByRef(ps[2].ParameterType))
                return null;

            ParameterExpression a0 = Expression.Parameter(typeof(object), "a0");
            ParameterExpression a1 = Expression.Parameter(typeof(object), "a1");
            ParameterExpression a2 = Expression.Parameter(typeof(object), "a2");
            UnaryExpression arg0 = Expression.Convert(a0, ps[0].ParameterType);
            UnaryExpression arg1 = Expression.Convert(a1, ps[1].ParameterType);
            UnaryExpression arg2 = Expression.Convert(a2, ps[2].ParameterType);
            NewExpression newExpr = Expression.New(ctor, arg0, arg1, arg2);
            Expression body = Expression.Convert(newExpr, typeof(object));
            return Expression.Lambda<Func<object?, object?, object?, object?>>(body, a0, a1, a2).Compile();
        }
        catch
        {
            return null;
        }
    }

    private static Func<object?, object?, object?, object?, object?>? BuildCtorInvoker4(ConstructorInfo ctor)
    {
        try
        {
            ParameterInfo[] ps = ctor.GetParameters();
            if (ps.Length != 4 || IsByRefLikeOrByRef(ps[0].ParameterType) || IsByRefLikeOrByRef(ps[1].ParameterType) ||
                IsByRefLikeOrByRef(ps[2].ParameterType) || IsByRefLikeOrByRef(ps[3].ParameterType))
                return null;

            ParameterExpression a0 = Expression.Parameter(typeof(object), "a0");
            ParameterExpression a1 = Expression.Parameter(typeof(object), "a1");
            ParameterExpression a2 = Expression.Parameter(typeof(object), "a2");
            ParameterExpression a3 = Expression.Parameter(typeof(object), "a3");
            UnaryExpression arg0 = Expression.Convert(a0, ps[0].ParameterType);
            UnaryExpression arg1 = Expression.Convert(a1, ps[1].ParameterType);
            UnaryExpression arg2 = Expression.Convert(a2, ps[2].ParameterType);
            UnaryExpression arg3 = Expression.Convert(a3, ps[3].ParameterType);
            NewExpression newExpr = Expression.New(ctor, arg0, arg1, arg2, arg3);
            Expression body = Expression.Convert(newExpr, typeof(object));
            return Expression.Lambda<Func<object?, object?, object?, object?, object?>>(body, a0, a1, a2, a3).Compile();
        }
        catch
        {
            return null;
        }
    }
}