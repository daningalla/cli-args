using FluentAssertions;
using Vertical.CommandLine.Conversion;

namespace Vertical.CommandLine;

public class ValueConverterTests
{
    private enum MyEnum
    {
        Red,
        Green,
        Blue
    };
    
    [Fact]
    public void GetInstanceOrDefault_Returns_Enum_Converter()
    { 
        // arrange
        var converter = DefaultValueConverter<MyEnum>.Instance;
        
        // act
        var color = converter!.Convert(new ConversionContext<MyEnum>("green", default!));
        
        // assert
        color.Should().Be(MyEnum.Green);
    }
}