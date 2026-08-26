using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Reflection.Cache.Abstract;

namespace Soenneker.Reflection.Cache.Registrars;

/// <summary>
/// Provides dependency-injection registration methods for the reflection cache.
/// </summary>
public static class ReflectionCacheRegistrar
{
    /// <summary>
    /// Registers <see cref="IReflectionCache"/> as a singleton when no implementation is already registered.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddReflectionCacheAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<IReflectionCache, ReflectionCache>();

        return services;
    }

    /// <summary>
    /// Registers <see cref="IReflectionCache"/> as a scoped service when no implementation is already registered.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddReflectionCacheAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<IReflectionCache, ReflectionCache>();

        return services;
    }
}
