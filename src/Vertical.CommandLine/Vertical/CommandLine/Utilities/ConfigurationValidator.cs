using System.Reflection;
using Vertical.CommandLine.Analysis;
using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Conversion;

namespace Vertical.CommandLine.Utilities;

#if DEBUG
public static class ConfigurationValidator
{
    private sealed class ExceptionComparer : IEqualityComparer<Exception>
    {
        /// <inheritdoc />
        public bool Equals(Exception? x, Exception? y) => StringComparer.Ordinal.Equals(x?.Message, y?.Message);

        /// <inheritdoc />
        public int GetHashCode(Exception obj) => obj.Message.GetHashCode();
    }
    
    public static IReadOnlyCollection<Exception> Validate(RootCommand rootCommand)
    {
        var exceptions = new HashSet<Exception>(new ExceptionComparer());
        var pathVisitor = new PathVisitor<Command>(command => command.Commands, path =>
            ValidateCommandPath(path, exceptions));
        
        pathVisitor.Visit(rootCommand);

        return exceptions;
    }

    private static void ValidateCommandPath(IEnumerable<Command> path, ICollection<Exception> exceptions)
    {
        foreach (var command in path)
        {
            ValidateCommand(command, exceptions);
        }

        ValidateHandlerSignatures(path, exceptions);
        ValidateParameterBindings(path, exceptions);
        ValidateConverterServices(path, exceptions);
    }

    private static void ValidateConverterServices(IEnumerable<Command> path, ICollection<Exception> exceptions)
    {
        var bindings = path.SelectMany(command => command.Bindings.Select(binding => (command, value: binding)));
        var converterServiceTypes = new HashSet<Type>(path
            .SelectMany(command => command.Converters)
            .Select(converter => converter.ValueType));
        var unsupportedBindings = bindings
            .Where(binding => !binding.value.HasConverter 
                              && !converterServiceTypes.Contains(binding.value.ValueType)
                              && !DefaultValueConverter.CanConvert(binding.value.ValueType));
        
        var typeGroups = unsupportedBindings.GroupBy(binding => binding.value.ValueType);
        if (typeGroups.Any())
        {
            exceptions.Add(ConfigurationExceptions.MissingValueConverters(path, typeGroups.ToArray()));
        }        
    }

    private static void ValidateParameterBindings(IEnumerable<Command> path, ICollection<Exception> exceptions)
    {
        var bindings = path.SelectMany(command => command.Bindings);
        var bindingDictionary = BindingDictionary<CliBindingSymbol>.Create(bindings, item => item);

        foreach (var command in path.Where(command => command.Handler is not null))
        {
            var method = command.Handler!.Method;
            var parameters = method.GetParameters();
            var unbindableParameters = new List<ParameterInfo>();

            foreach (var parameter in parameters.Where(p => p.ParameterType != typeof(CancellationToken)))
            {
                var bindingName = parameter.GetCustomAttribute<BindingAttribute>()?.BindingId
                                  ?? parameter.Name
                                  ?? string.Empty;

                if (!bindingDictionary.TryGetValue(bindingName, out var symbol))
                {
                    unbindableParameters.Add(parameter);
                    continue;
                }

                if (IsParameterCompatibleWithSymbolType(parameter, symbol))
                    continue;
                
                exceptions.Add(ConfigurationExceptions.IncompatibleParameterType(command, method, parameter, symbol));
            }

            if (unbindableParameters.Count == 0) 
                continue;
            
            exceptions.Add(ConfigurationExceptions.HandlerHasUnbindableParameters(
                command,
                method,
                unbindableParameters));
        }        
    }

    private static bool IsParameterCompatibleWithSymbolType(ParameterInfo parameter, CliBindingSymbol binding)
    {
        return binding.ValueType.IsAssignableFrom(parameter.ParameterType);
    }

    private static void ValidateHandlerSignatures(IEnumerable<Command> path, ICollection<Exception> exceptions)
    {
        var handledCommands = path.Where(command => command.Handler is not null);
        var handlerMethods = handledCommands.Select(command => command.Handler!.Method).ToArray();

        if (handlerMethods.Length == 0)
        {
            exceptions.Add(ConfigurationExceptions.NoCommandHandlerInPath(path));
            return;
        }

        var returnTypeCount = handlerMethods
            .Select(method => method.ReturnType)
            .Distinct()
            .Count();

        if (returnTypeCount != 1)
        {
            exceptions.Add(ConfigurationExceptions.MismatchedHandlerReturnType(handledCommands));
        }
    }

    private static void ValidateCommand(Command command, ICollection<Exception> exceptions)
    {
        ValidateCommandIdentifiers(command, exceptions);
        ValidateUniqueChildCommandIdentifiers(command, exceptions);
        ValidateHandlerRequirement(command, exceptions);
        ValidateArgumentArityConfiguration(command, exceptions);
    }

    private static void ValidateArgumentArityConfiguration(Command command, ICollection<Exception> exceptions)
    {
        var argumentBindings = command
            .Bindings
            .Where(binding => binding.SymbolType == CliSymbolType.Argument)
            .ToArray();

        for (var c = 0; c < argumentBindings.Length - 1; c++)
        {
            var arity = argumentBindings[c].Arity;

            if (arity.MinCount > 0 && arity.MinCount == arity.MaxCount)
                continue;
            
            exceptions.Add(ConfigurationExceptions.InvalidArgumentArityOrder(command, argumentBindings));
        }
    }

    private static void ValidateHandlerRequirement(Command command, ICollection<Exception> exceptions)
    {
        var handlerRequired = command.Bindings.Any(binding => binding.Scope is 
            BindingScope.Self or BindingScope.SelfAndDescendents);

        if (handlerRequired && command.Handler is null)
        {
            exceptions.Add(ConfigurationExceptions.NoCommandHandler(command));
        }
    }

    private static void ValidateUniqueChildCommandIdentifiers(Command command, ICollection<Exception> exceptions)
    {
        // Validate distinct child command identifiers and aliases
        var duplicateChildIdentifierGroupings = command
            .Commands
            .SelectMany(child => child
                .Aliases
                .Append(child.Id)
                .Select(identifier => (child, identifier)))
            .GroupBy(item => item.identifier, item => item.child)
            .Where(grouping => grouping.Count() > 1);

        foreach (var grouping in duplicateChildIdentifierGroupings)
        {
            exceptions.Add(ConfigurationExceptions.DuplicateChildCommandIdentifier(
                grouping.Key,
                grouping.ToArray()));
        }
    }

    private static void ValidateCommandIdentifiers(CliSymbol command, ICollection<Exception> exceptions)
    {
        if (command is RootCommand)
            return;
        
        // Validate naming
        foreach (var identifier in command.Aliases.Append(command.Id))
        {
            if (NamingAnalysis.IsValidNonPrefixedIdentifier(identifier))
                continue;

            exceptions.Add(ConfigurationExceptions.InvalidIdentifierName(command, identifier));
        }
    }
}
#endif