using NSubstitute;
using Vertical.CommandLine.Binding;

namespace Vertical.CommandLine.Test;

public static class Substitutes
{
    public static T CreateBindingService<T>() where T : class, IBindingService
    {
        var mock = Substitute.For<T>();
        mock.ServiceType.Returns(typeof(T));
        return mock;
    }
}