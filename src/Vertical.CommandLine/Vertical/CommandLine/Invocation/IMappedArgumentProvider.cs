using Vertical.CommandLine.Binding;

namespace Vertical.CommandLine.Invocation;

/// <summary>
/// Provides values mapped from command line arguments.
/// </summary>
public interface IMappedArgumentProvider
{
    /// <summary>
    /// Gets a single arity value.
    /// </summary>
    /// <param name="bindingId">The parameter id that corresponds to the defined argument, option, or switch.</param>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <returns>The value.</returns>
    T GetValue<T>(string bindingId);

    /// <summary>
    /// Gets a multi arity value as an array.
    /// </summary>
    /// <param name="bindingId">The parameter id that corresponds to the defined argument, option, or switch.</param>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <returns>The value.</returns>
    T[] GetValueArray<T>(string bindingId);

    /// <summary>
    /// Gets a multi arity value as a list.
    /// </summary>
    /// <param name="bindingId">The parameter id that corresponds to the defined argument, option, or switch.</param>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <returns>The value.</returns>
    List<T> GetValueList<T>(string bindingId);

    /// <summary>
    /// Gets a multi arity value as a linked list.
    /// </summary>
    /// <param name="bindingId">The parameter id that corresponds to the defined argument, option, or switch.</param>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <returns>The value.</returns>
    LinkedList<T> GetValueLinkedList<T>(string bindingId);

    /// <summary>
    /// Gets a multi arity value as a hash set.
    /// </summary>
    /// <param name="bindingId">The parameter id that corresponds to the defined argument, option, or switch.</param>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <returns>The value.</returns>
    HashSet<T> GetValueHashSet<T>(string bindingId);

    /// <summary>
    /// Gets a multi arity value as a sorted set.
    /// </summary>
    /// <param name="bindingId">The parameter id that corresponds to the defined argument, option, or switch.</param>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <returns>The value.</returns>
    SortedSet<T> GetValueSortedSet<T>(string bindingId);

    /// <summary>
    /// Gets a multi arity value as a stack.
    /// </summary>
    /// <param name="bindingId">The parameter id that corresponds to the defined argument, option, or switch.</param>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <returns>The value.</returns>
    Stack<T> GetValueStack<T>(string bindingId);

    /// <summary>
    /// Gets a multi arity value as a queue.
    /// </summary>
    /// <param name="bindingId">The parameter id that corresponds to the defined argument, option, or switch.</param>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <returns>The value.</returns>
    Queue<T> GetValueQueue<T>(string bindingId);

    /// <summary>
    /// Gets the binding dictionary keyed by parameter name.
    /// </summary>
    BindingDictionary<IArgumentValueBinding> Bindings { get; }

    /// <summary>
    /// Gets binding services.
    /// </summary>
    BindingServiceCollection Services { get; }
}