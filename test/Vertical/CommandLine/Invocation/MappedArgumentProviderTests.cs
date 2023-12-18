using FluentAssertions;

namespace Vertical.CommandLine.Invocation;

public class MappedArgumentProviderTests
{
    private static readonly RootCommand Command = new()
    {
        Bindings =
        {
            new Option<string>("--value"),
            new Option<string>("--collection", arity: Arity.ZeroOrMany )
        },
        Handler = () => { }
    };

    [Fact]
    public void GetValue_Returns_String() => AssertStringValue("red");

    [Fact]
    public void GetValue_Returns_Null() => AssertStringValue(null);

    [Fact]
    public void GetValueArray_Returns_Expected() => AssertCollection((p, arg) => p.GetValueArray<string>(arg), 
        typeof(string[]),
        "red", "green", "blue");

    [Fact]
    public void GetList_Returns_Expected() => AssertCollection((p, arg) => p.GetValueList<string>(arg),
        typeof(List<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetList_Returns_Empty() => AssertCollection((p, arg) => p.GetValueList<string>(arg),
        typeof(List<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetStack_Returns_Expected() => AssertCollection((p, arg) => p.GetValueStack<string>(arg),
        typeof(Stack<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetStack_Returns_Empty() => AssertCollection((p, arg) => p.GetValueStack<string>(arg),
        typeof(Stack<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetQueue_Returns_Expected() => AssertCollection((p, arg) => p.GetValueQueue<string>(arg),
        typeof(Queue<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetQueue_Returns_Empty() => AssertCollection((p, arg) => p.GetValueQueue<string>(arg),
        typeof(Queue<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetHashSet_Returns_Expected() => AssertCollection((p, arg) => p.GetValueHashSet<string>(arg),
        typeof(HashSet<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetHashSet_Returns_Empty() => AssertCollection((p, arg) => p.GetValueHashSet<string>(arg),
        typeof(HashSet<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetSortedSet_Returns_Expected() => AssertCollection((p, arg) => p.GetValueSortedSet<string>(arg),
        typeof(SortedSet<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetSortedSet_Returns_Empty() => AssertCollection((p, arg) => p.GetValueSortedSet<string>(arg),
        typeof(SortedSet<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetLinkedList_Returns_Expected() => AssertCollection((p, arg) => p.GetValueLinkedList<string>(arg),
        typeof(LinkedList<string>),
        "red", "green", "blue");
    
    [Fact]
    public void GetLinkedList_Returns_Empty() => AssertCollection((p, arg) => p.GetValueLinkedList<string>(arg),
        typeof(LinkedList<string>),
        "red", "green", "blue");
    
    private static void AssertStringValue(string? arg)
    {
        // arrange/act
        var context = InvocationContextBuilder.Create(Command, arg != null 
            ? new[]{ $"--value={arg}" } 
            : Array.Empty<string>());

        // assert
        var provider = context.ArgumentProvider;
        provider.GetValue<string?>("--value").Should().Be(arg);
    }

    private static void AssertCollection(
        Func<IMappedArgumentProvider, string, IEnumerable<string>> collectionSelector,
        Type collectionType,
        params string[] values)
    {
        // arrange/act
        var args = values.Select(arg => $"--collection={arg}").ToArray();
        var context = InvocationContextBuilder.Create(Command, args);
        
        // assert
        var provider = context.ArgumentProvider;
        var collection = collectionSelector(provider, "--collection");
        collection
            .OrderBy(value => value)
            .Should()
            .Equal(values.OrderBy(value => value));
        collection.Should().BeOfType(collectionType);
    }
}