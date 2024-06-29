using System.Buffers;
using bex.core;
using bex.parsers;
using bex.tree;
using bex.tree.classes;
using FluentAssertions;

namespace bex.tests.tree.classes;

public class BexPositiveByteGroupClassTests
{
    [Fact]
    public async Task Should_Compile_Ok()
    {
        // Arrange
        var lit01 = new BexLiteral([0x21]);
        var ctx01 = new BexContext();
        var subject = new BexPositiveByteGroupClass([lit01]);
        
        // Act
        var result01 = await subject.MatchAsync(ctx01);

        // Assert
        result01.Success.Should().BeTrue();
        result01.Captured.Should().BeEquivalentTo([0x77]);
    }
    
    [Fact]
    public async Task Should_Build_and_Compile_Ok()
    {
        // Arrange
        var bexx = new Bexx("[05 A1 77]");
        var bytes01 = new byte[] {0x05, 0xFF, 0xA1, 0x00, 0x77};
        
        // Act
        var bres01 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes01));

        // Assert
        bres01.Success.Should().BeTrue();
        bres01.Captured.Should().BeEquivalentTo([0x05]);
    }
    
}