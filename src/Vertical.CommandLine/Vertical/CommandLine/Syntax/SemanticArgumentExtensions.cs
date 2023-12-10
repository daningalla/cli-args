namespace Vertical.CommandLine.Syntax;

public static class SemanticArgumentExtensions
{
    public static bool TryGetBooleanArgumentValue(this SemanticArgument argument, 
        out bool booleanValue)
    {
        return bool.TryParse(argument.Text, out booleanValue);
    }
}