using System.Buffers;
using bex.core;
using bex.parsers;
using bex.tree;
using bex.tree.operators;
using FluentAssertions;

namespace bex.tests.tree.operators;

public class BexLogicalOrOperatorTests
{
    [Fact]
    public async Task Should_Parse_Simple_LogicalOr_Ok()
    {
        // Arrange
        var leftLiteral = new BexLiteral([0x2F]);
        var rightLiteral = new BexLiteral([0x1A]);
        
        // Act
        var subject = new BexLogicalOrOperator(leftLiteral, rightLiteral);
        var result = await subject.MatchAsync(new BexContext());
        // Assert
        result.Success.Should().BeTrue();

        var matcher = result.Matcher as BexLogicalOrOperator;
        matcher.Should().BeOfType<BexLogicalOrOperator>();
        matcher.IsOperator().Should().BeTrue();

        matcher.LeftOperand.Should().Be(leftLiteral);
        matcher.RightOperand.Should().Be(rightLiteral);

        matcher.LeftOperand.Previous.Should().BeNull();
        matcher.LeftOperand.Next.Should().BeNull();
        matcher.LeftOperand.Parent.Should().Be(subject);
        
        matcher.RightOperand.Previous.Should().BeNull();
        matcher.RightOperand.Next.Should().BeNull();
        matcher.RightOperand.Parent.Should().Be(subject);

        result.Captured.Should().BeEquivalentTo([0x2F]);
    }

    [Fact]
    public async Task Should_Parse_LogicalOr_with_2_Groups_Ok()
    {
        // Arrange
        var leftGroup = new BexGroup([new BexLiteral([0x01]), new BexLiteral([0x21]), new BexLiteral([0x7F])]);
        var rightGroup = new BexGroup([new BexLiteral([0x21]), new BexLiteral([0x06]), new BexLiteral([0x77])]);

        // Act
        var subject = new BexLogicalOrOperator(leftGroup, rightGroup);
        var ctx = new BexContext();
        var result = await subject.MatchAsync(ctx);

        // Assert
        result.Success.Should().BeTrue();
        result.Captured.Should().BeEquivalentTo([0x21, 0x06, 0x77]);
    }

    [Fact]
    public async Task Should_Parse_LogicalOr_with_1_Literal_and_1_Group_Ok()
    {
        // Arrange
        var leftLiteral = new BexLiteral([0x01]);
        var rightGroup = new BexGroup([new BexLiteral([0x21]), new BexLiteral([0x06]), new BexLiteral([0x77])]);

        var input = new byte[] {0x01, 0x21, 0x06, 0x77, 0x25, 0xAC};
        var ctx01 = new BexContext();

        // Act
        var subject = new BexLogicalOrOperator(leftLiteral, rightGroup);
        var result1 = await subject.MatchAsync(ctx01);

        // Assert
        result1.Success.Should().BeTrue();
        result1.Captured.Should().BeEquivalentTo([0x01]);
    }


    [Fact]
    public async Task Should_Build_and_Compile_3x_single_or_Ok()
    {
        // Arrange
        var bexx = new Bexx("21||14 06||07 77||76");
        var bytes01 = new byte[] {0x21, 0x06, 0x77};
        var bytes02 = new byte[] {0x14, 0x07, 0x76};
        
        // Act
        var result = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes01));
        
        // Assert
        result.Success.Should().BeTrue();
        result.Captured.Should().BeEquivalentTo(bytes01);
    }

 
}