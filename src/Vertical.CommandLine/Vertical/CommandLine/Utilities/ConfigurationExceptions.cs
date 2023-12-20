using System.Reflection;
using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Utilities;

internal static class ConfigurationExceptions
{
    public static Exception InvalidIdentifierName(CliSymbol symbol, string name)
    {
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.AppendLine($"Invalid identifier in {symbol.SymbolType}: \"{name}\"");
            sb.AppendLine("Valid identifiers for this symbol type must:");
            switch (symbol.SymbolType)
            {
                case CliSymbolType.Command:
                case CliSymbolType.Argument:
                    sb.AppendLine("\t- contain only lower case letters, numbers, or hyphens ('-').");
                    sb.AppendLine("\t- start with a lower case letter.");
                    break;
                
                default:
                    sb.AppendLine("\t- contain only lower case letters, numbers, or hyphens ('-').");
                    sb.AppendLine("\t- start with a valid prefix: '-', '--', or '/'.");
                    break;
            }
        });

        return new ArgumentException(message);
    }

    public static Exception NoCommandHandler(Command command)
    {
        var message = $"Command {command} must define a delegate handler because it has binding symbols scoped to " +
                      "itself.";
        return new InvalidOperationException(message);
    }

    public static Exception DuplicateChildCommandIdentifier(string identifier, Command[] commands)
    {
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.AppendLine($"Found duplicate identifier \"{identifier}\" in the following commands:");
            foreach (var command in commands)
            {
                sb.AppendLine($"\t-> {command}");
            }
        });
        return new InvalidOperationException(message);
    }

    public static Exception NoCommandHandlerInPath(IEnumerable<Command> path)
    {
        var pathString = string.Join(" -> ", path.Select(cmd => cmd.Id));
        var message = $"The following command path does not define a delegate handler: {pathString}.";
        return new InvalidOperationException(message);
    }

    public static Exception MismatchedHandlerReturnType(IEnumerable<Command> commands)
    {
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.AppendLine("The following command handler signatures are not compatible:");
            foreach (var command in commands)
            {
                var method = command.Handler!.Method;
                var parameterString = string.Join(',', method.GetParameters().Select(p => p.ParameterType));
                sb.AppendLine($"\t-> {command} ({parameterString}) => {method.ReturnType}");
            }

            sb.AppendLine("All handlers in the configuration must have the same return type.");
        });
        return new InvalidOperationException(message);
    }

    public static Exception InvalidArgumentArityOrder(Command command, CliBindingSymbol[] argumentBindings)
    {
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.AppendLine($"The following argument configurations for command {command} are invalid:");
            foreach (var binding in argumentBindings)
            {
                sb.AppendLine($" {binding} (arity = {binding.Arity})");
            }

            sb.AppendLine("When multiple argument bindings are defined in a command, only the last argument ");
            sb.AppendLine("can have an arity that represents an optional or multi-valued argument. All arguments");
            sb.AppendLine("before the last argument must have an arity that represents an exact count.");
        });
        return new InvalidOperationException(message);
    }

    public static Exception HandlerHasUnbindableParameters(
        Command command,
        MethodInfo method,
        IReadOnlyCollection<ParameterInfo> parameters)
    {
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.AppendLine($"Command {command} handler parameter is unbindable.");
            sb.AppendLine("Handler parameters:");
            foreach (var parameter in method.GetParameters())
            {
                var unbindable = parameters.Contains(parameter);
                sb.Append(unbindable ? "\t--> " : "\t");
                
                var attribute = parameter.GetCustomAttribute<BindingAttribute>();
                if (attribute != null)
                {
                    sb.Append($"[Bind(\"{attribute.BindingId}\")] ");
                }
                sb.Append($"{parameter.ParameterType} {parameter.Name}");

                sb.AppendLine(unbindable ? " (*no matching symbol)" : string.Empty);
            }
        });

        return new InvalidOperationException(message);
    }

    public static Exception IncompatibleParameterType(
        Command command,
        MethodInfo method,
        ParameterInfo parameter,
        CliBindingSymbol binding)
    {
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.AppendLine($"Command {command} delegate handler is unbindable.");
            foreach (var methodParameter in method.GetParameters())
            {
                var unbindable = methodParameter == parameter;
                
                sb.Append(unbindable ? "\t--> " : "\t");
                sb.Append($"{parameter.ParameterType} {parameter.Name}");
                sb.AppendLine(unbindable ? $" (*incompatible with {binding}={binding.ValueType})" : string.Empty);
            }
        });
        return new InvalidOperationException(message);
    }

    public static Exception MissingValueConverters(
        IEnumerable<Command> path,
        IGrouping<Type, (Command command, CliBindingSymbol value)>[] groups)
    {
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.AppendLine("The following types are unbindable because they do not have value converters " +
                          "or model binders defined:");
            
            foreach (var group in groups)
            {
                sb.AppendLine($"\t{group.Key}, defined in:");
                foreach (var (command, symbol) in group)
                {
                    var commandPath = path
                        .TakeWhile(cmd => cmd != command)
                        .Append(command);
                    var commandPathString = string.Join(" -> ", commandPath.Select(cmd => cmd.Id));
                    var symbolPathString = $"{commandPathString} -> {symbol}";
                    sb.AppendLine($"\t\t{symbolPathString}");
                }       
            }
        });

        return new InvalidOperationException(message);
    }

    public static Exception NoBindingSymbolForId(string symbolId)
    {
        return new InvalidOperationException($"Cannot resolve binding symbol \"{symbolId}\".");
    }

    public static Exception IncompatibleBindingSymbolType(CliBindingSymbol symbol, Type type)
    {
        throw new NotImplementedException();
    }
}