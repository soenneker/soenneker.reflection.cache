using System.Reflection;
using Soenneker.Reflection.Cache.Constants;

namespace Soenneker.Reflection.Cache.Options;

/// <summary>
/// Configures which members are discovered and cached for each reflection category.
/// </summary>
public sealed class ReflectionCacheOptions
{
    /// <summary>
    /// Gets or sets the binding flags used to discover fields. Defaults to <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </summary>
    public BindingFlags FieldFlags { get; set; } = ReflectionCacheConstants.BindingFlags;

    /// <summary>
    /// Gets or sets the binding flags used to discover properties. Defaults to <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </summary>
    public BindingFlags PropertyFlags { get; set; } = ReflectionCacheConstants.BindingFlags;

    /// <summary>
    /// Gets or sets the binding flags used to discover members. Defaults to <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </summary>
    public BindingFlags MemberFlags { get; set; } = ReflectionCacheConstants.BindingFlags;

    /// <summary>
    /// Gets or sets the binding flags used to discover methods. Defaults to <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </summary>
    public BindingFlags MethodFlags { get; set; } = ReflectionCacheConstants.BindingFlags;

    /// <summary>
    /// Gets or sets the binding flags used to discover constructors. Defaults to <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </summary>
    public BindingFlags ConstructorFlags { get; set; } = ReflectionCacheConstants.BindingFlags;
}
