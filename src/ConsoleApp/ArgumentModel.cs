using Vertical.CommandLine.Binding;

namespace ConsoleApp;

[GeneratedBinding]
public class PushArguments
{
    public PushArguments(FileInfo root)
    {
        Root = root;
    }

    public FileInfo Root { get; }
    
    public bool DisableBuffering { get; init; }
    
    public bool Interactive { get; init; }
    
    public string? ApiKey { get; init; }
    
    public string? Source { get; init; }
    
    public bool SkipDuplicates { get; init; }
    
    public TimeSpan? Timeout { get; init; }
    
    public Point? Coordinates { get; init; }
}

[GeneratedBinding<PushArguments>]
public partial class PushArgumentsBinder
{
}