using Vertical.CommandLine.Invocation;

namespace Vertical.CommandLine.Binding;

/// <summary>
/// Represents an object that binds mapped arguments to models.
/// </summary>
public interface IModelBinder : IBindingService
{
    /// <summary>
    /// Binds a model instance.
    /// </summary>
    /// <param name="argumentProvider">Argument provider.</param>
    /// <returns><see cref="IModelValue{T}"/></returns>
    IModelValue BindInstanceBase(IMappedArgumentProvider argumentProvider);
    
    /// <summary>
    /// Gets the value type.
    /// </summary>
    Type ValueType { get; }
}