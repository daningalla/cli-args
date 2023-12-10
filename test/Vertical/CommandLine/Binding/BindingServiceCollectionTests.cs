using FluentAssertions;

namespace Vertical.CommandLine.Binding;

public class BindingServiceCollectionTests
{
    private interface IMyService<T> : IBindingService {}

    private sealed class MyService<T> : IMyService<T>
    {
        /// <inheritdoc />
        public Type ServiceType => typeof(IMyService<T>);
    }

    private readonly BindingServiceCollection _instance = new();

    [Fact]
    public void AddRange_Registers_Services_By_IBindingService_ServiceType()
    {
        // arrange
        var (stringService, boolService) = (new MyService<string>(), new MyService<bool>());

        // act
        _instance.AddRange(new IBindingService[]{ stringService, boolService });
        
        // assert
        _instance.GetService<IMyService<string>>().Should().BeSameAs(stringService);
        _instance.GetService<IMyService<bool>>().Should().BeSameAs(boolService);
    }
}