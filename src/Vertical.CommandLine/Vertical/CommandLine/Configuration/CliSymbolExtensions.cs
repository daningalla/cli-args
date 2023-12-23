namespace Vertical.CommandLine.Configuration;

internal static class CliSymbolExtensions
{
    internal static bool IsMatchToAnyIdentifier(this CliSymbol symbol, string identifier)
    {
        return symbol.Id.Equals(identifier) || symbol.Aliases.Any(alias => alias.Equals(identifier));
    }

    internal static void ValidateArity(this CliBindingSymbol symbol, int count, Func<IEnumerable<string?>> args)
    {
        if (count >= symbol.Arity.MinCount)
            return;

        if (count <= (symbol.Arity.MaxCount ?? int.MaxValue))
            return;
        
        throw new CommandLineArityException(symbol, args());
    }
}