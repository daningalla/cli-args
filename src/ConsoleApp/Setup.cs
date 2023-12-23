using Vertical.CommandLine;
using Vertical.CommandLine.Binding;

namespace ConsoleApp;

public class ModelBase
{
    public bool NoRestore { get; set; }
    public bool NoBuild { get; set; }
}

public class Model : ModelBase
{
    public string? Root { get; set; }
    public DateTime? TimeStamp { get; set; }
}

[GeneratedBinding<Model>]
public partial class ModelBinder
{
}