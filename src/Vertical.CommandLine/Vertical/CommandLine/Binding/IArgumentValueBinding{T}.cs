﻿using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Binding;

public interface IArgumentValueBinding<T> : IArgumentValueBinding
{
    /// <summary>
    /// Gets the binding symbol.
    /// </summary>
    CliBindingSymbol<T> Symbol { get; }
    
    /// <summary>
    /// Gets the argument values.
    /// </summary>
    T[] ArgumentValues { get; }
}