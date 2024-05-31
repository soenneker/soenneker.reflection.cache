using System.Reflection;
using Soenneker.Reflection.Cache.Constants;

namespace Soenneker.Reflection.Cache.Options;

/// <summary>
/// Represents options for configuring reflection cache behavior.
/// </summary>
public class ReflectionCacheOptions
{
    /// <summary>
    /// Gets or sets the binding flags used for field reflection.
    /// </summary>
    /// <value>
    /// The binding flags used for field reflection. The default value is <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </value>
    public BindingFlags FieldFlags { get; set; } = ReflectionCacheConstants.BindingFlags;

    /// <summary>
    /// Gets or sets the binding flags used for property reflection.
    /// </summary>
    /// <value>
    /// The binding flags used for property reflection. The default value is <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </value>
    public BindingFlags PropertyFlags { get; set; } = ReflectionCacheConstants.BindingFlags;

    /// <summary>
    /// Gets or sets the binding flags used for member reflection.
    /// </summary>
    /// <value>
    /// The binding flags used for member reflection. The default value is <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </value>
    public BindingFlags MemberFlags { get; set; } = ReflectionCacheConstants.BindingFlags;

    /// <summary>
    /// Gets or sets the binding flags used for method reflection.
    /// </summary>
    /// <value>
    /// The binding flags used for method reflection. The default value is <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </value>
    public BindingFlags MethodFlags { get; set; } = ReflectionCacheConstants.BindingFlags;

    /// <summary>
    /// Gets or sets the binding flags used for constructor reflection.
    /// </summary>
    /// <value>
    /// The binding flags used for constructor reflection. The default value is <see cref="ReflectionCacheConstants.BindingFlags"/>.
    /// </value>
    public BindingFlags ConstructorFlags { get; set; } = ReflectionCacheConstants.BindingFlags;
}