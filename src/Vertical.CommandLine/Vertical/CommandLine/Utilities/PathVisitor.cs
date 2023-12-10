using System.Collections.Immutable;

namespace Vertical.CommandLine.Utilities;

internal sealed class PathVisitor<T>
{
    private readonly Func<T, IEnumerable<T>> _iterator;
    private readonly Action<IEnumerable<T>> _visitor;

    internal PathVisitor(Func<T, IEnumerable<T>> iterator, Action<IEnumerable<T>> visitor)
    {
        _iterator = iterator;
        _visitor = visitor;
    }

    internal void Visit(T node) => Visit(ImmutableList<T>.Empty.Add(node), node);

    private void Visit(ImmutableList<T> path, T node)
    {
        var children = _iterator(node).ToArray();

        if (children.Length == 0)
        {
            _visitor(path);
            return;
        }

        foreach (var child in children)
        {
            Visit(path.Add(child), child);
        }
    }
}