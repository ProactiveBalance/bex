using bex.tree;
using bex.tree.operators;
using FluentAssertions;

namespace bex.tests.tree.operators;

public class BexLogicalAndOperatorTests
{
    [Fact]
    public async Task Should_Parse_Simple_LogicalAnd_Ok()
    {
        // Arrange
        var leftLiteral = new BexLiteral([0x2F]);
        var rightLiteral = new BexLiteral([0x1A]);
        
        // Act
        var subject = new BexLogicalAndOperator(leftLiteral, rightLiteral);
        var result01 = await subject.MatchAsync(new BexContext());
        // Assert
        result01.Success.Should().BeTrue();

        var matcher = result01.Matcher as BexLogicalAndOperator;
        matcher.Should().BeOfType<BexLogicalAndOperator>();
        matcher.IsOperator().Should().BeTrue();

        matcher.LeftOperand.Should().Be(leftLiteral);
        matcher.RightOperand.Should().Be(rightLiteral);

        matcher.LeftOperand.Previous.Should().BeNull();
        matcher.LeftOperand.Next.Should().BeNull();
        matcher.LeftOperand.Parent.Should().Be(subject);
        
        matcher.RightOperand.Previous.Should().BeNull();
        matcher.RightOperand.Next.Should().BeNull();
        matcher.RightOperand.Parent.Should().Be(subject);

        result01.Captured.Should().BeEquivalentTo([0x2F, 0x1A]);
    }

    [Fact]
    public async Task Should_Parse_LogicalAnd_with_2_Groups_Ok()
    {
        // Arrange
        var leftGroup = new BexGroup([new BexLiteral([0x01]), new BexLiteral([0x21]), new BexLiteral([0x7F])]);
        var rightGroup = new BexGroup([new BexLiteral([0x21]), new BexLiteral([0x06]), new BexLiteral([0x77])]);

        // Act
        var subject = new BexLogicalAndOperator(leftGroup, rightGroup);
        var result01 = await subject.MatchAsync(new BexContext());

        // Assert
        result01.Success.Should().BeTrue();
        result01.Captured.IsEmpty().Should().BeFalse();
    }

    [Fact]
    public async Task Should_Parse_LogicalAnd_with_1_Literal_and_1_Group_Ok()
    {
        // Arrange
        var leftLiteral = new BexLiteral([0x01]);
        var rightGroup = new BexGroup([new BexLiteral([0x21]), new BexLiteral([0x06]), new BexLiteral([0x77])]);

        var input = new byte[] {0x01, 0x21, 0x06, 0x77, 0x25, 0xAC};
        var ctx01 = new BexContext();

        // Act
        var subject = new BexLogicalAndOperator(leftLiteral, rightGroup);
        var result1 = await subject.MatchAsync(ctx01);

        // Assert
        result1.Success.Should().BeTrue();
        result1.Captured.Should().BeEquivalentTo([0x01, 0x21, 0x06, 0x77]);
    }
}