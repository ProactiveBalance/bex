using System.Buffers;
using bex.core;
using bex.parsers;
using bex.tree;
using bex.tree.classes;
using FluentAssertions;

namespace bex.tests.tree.classes;

public class BexNegativeByteGroupClassTests
{
    [Fact]
    public async Task Should_Compile_Ok()
    {
        // Arrange
        var lit01 = new BexLiteral([0x21]);
        var lit02 = new BexLiteral([0x06]);
        var lit03 = new BexLiteral([0x77]);
        var subject = new BexNegativeByteGroupClass([lit01, lit02, lit03]);
        var ctx = new BexContext();
        
        // Act
        var result01 = await subject.MatchAsync(ctx);

        // Assert
        result01.Success.Should().BeFalse();
        result01.Captured.IsEmpty().Should().BeTrue();
    }
    
    [Fact]
    public async Task Should_Build_and_Compile_Ok()
    {
        // Arrange
        var bexx = new Bexx("[^05 A1 77]");
        var bytes01 = new byte[] {0x05, 0xFF, 0xA1, 0x00, 0x77};
        
        // Act
        var bres01 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes01));

        // Assert
        bres01.Success.Should().BeFalse();
        bres01.Captured.IsEmpty().Should().BeTrue();
    }
    
}