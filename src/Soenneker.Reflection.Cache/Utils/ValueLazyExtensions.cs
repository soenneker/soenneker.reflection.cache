global using Soenneker.Atomics.ValueLazys;
global using Soenneker.Atomics.ValueLocks;
global using Soenneker.Atomics.ValueNullableLazys;
using System;
using System.Runtime.CompilerServices;

namespace Soenneker.Reflection.Cache.Utils;

internal static class ValueLazyExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetOrCreate<T, TState>(this ref ValueLazy<T> lazy, bool threadSafe, ref ValueAtomicLock sync, TState state,
        Func<TState, T> factory) where T : class =>
        threadSafe ? lazy.GetOrCreate(ref sync, state, factory) : lazy.GetOrCreateUnsafe(state, factory);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetOrCreate<T, TState>(this ref ValueNullableLazy<T> lazy, bool threadSafe, ref ValueAtomicLock sync, TState state,
        Func<TState, T?> factory) where T : class =>
        threadSafe ? lazy.GetOrCreate(ref sync, state, factory) : lazy.GetOrCreateUnsafe(state, factory);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetOrCreatePublicationOnly<T, TState>(this ref ValueLazy<T> lazy, bool threadSafe, TState state, Func<TState, T> factory)
        where T : class =>
        threadSafe ? lazy.GetOrCreatePublicationOnly(state, factory) : lazy.GetOrCreateUnsafe(state, factory);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetOrCreatePublicationOnly<T, TState>(this ref ValueNullableLazy<T> lazy, bool threadSafe, TState state,
        Func<TState, T?> factory) where T : class =>
        threadSafe ? lazy.GetOrCreatePublicationOnly(state, factory) : lazy.GetOrCreateUnsafe(state, factory);
}
