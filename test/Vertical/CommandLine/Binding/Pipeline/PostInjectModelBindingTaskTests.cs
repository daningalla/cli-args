namespace Vertical.CommandLine.Binding.Pipeline;

public class PostInjectModelBindingTaskTests
{
    public enum Config { Debug, Release }

    public record MyOptions(Config Configuration, FileInfo ProjectPath, bool NoRestore);
}