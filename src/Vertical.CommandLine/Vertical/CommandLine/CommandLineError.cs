namespace Vertical.CommandLine;

/// <summary>
/// Defines command line errors.
/// </summary>
public enum CommandLineError
{
    ConversionFailed,
    ValidationFailed,
    MinimumArityNotMet,
    MaximumArityExceeded,
    InvalidArgument,
    MissingOperand
}