using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Reflection.Cache.Abstract;

namespace Soenneker.Reflection.Cache.Registrars;

/// <summary>
/// The fastest .NET Reflection cache
/// </summary>
public static class ReflectionCacheRegistrar
{
    /// <summary>
    /// Adds <see cref="IReflectionCache"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddReflectionCacheAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<IReflectionCache, ReflectionCache>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IReflectionCache"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddReflectionCacheAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<IReflectionCache, ReflectionCache>();

        return services;
    }
}