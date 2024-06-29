using bex.parsers;
using bex.tree;
using bex.tree.operators;
using FluentAssertions;
using FluentAssertions.Extensions;
using Pidgin;

using static bex.parsers.BexHexParser;

namespace bex.tests.parsers;

public partial class BexHexParserTests
{

    [Fact]
    public void Should_BexLogicalOrOperatorParser_Parse_Ok()
    {
        //Arrange
        var expression = "2F||3F";

        // Act
        var result = BexLogicalOrOperatorParser.Parse(expression);
        
        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        result.Value.Token.Should().Be("||");
        result.Value.BType.Should().Be(BexType.Operator);
        result.Value.OperatorType.Should().Be(BexOperatorType.LogicalOr);

        var left = result.Value.LeftOperand;
        left.Should().NotBeNull();
        left.BType.Should().Be(BexType.Literal);
        var leftLiteral = left as BexLiteral;
        leftLiteral.Expected.Should().BeEquivalentTo([0x2F]);
        
        var right = result.Value.RightOperand;
        right.Should().NotBeNull();
        right.BType.Should().Be(BexType.Literal);
        var rightLiteral = right as BexLiteral;
        rightLiteral.Expected.Should().BeEquivalentTo([0x3F]);
    }
    
    [Fact]
    public void Should_BexLogicalAndOperatorParser_Parse_Ok()
    {
        //Arrange
        var expression = "5A&&4F";

        // Act
        var result = BexLogicalAndOperatorParser.Parse(expression);
        
        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        result.Value.Token.Should().Be("&&");
        result.Value.BType.Should().Be(BexType.Operator);
        result.Value.OperatorType.Should().Be(BexOperatorType.LogicalAnd);

        var left = result.Value.LeftOperand;
        left.Should().NotBeNull();
        left.BType.Should().Be(BexType.Literal);
        var leftLiteral = left as BexLiteral;
        leftLiteral.Expected.Should().BeEquivalentTo([0x5A]);
        
        var right = result.Value.RightOperand;
        right.Should().NotBeNull();
        right.BType.Should().Be(BexType.Literal);
        var rightLiteral = right as BexLiteral;
        rightLiteral.Expected.Should().BeEquivalentTo([0x4F]);
    }

    [Fact]
    public void Should_BexOperatorParser_Parse_Ok()
    {
        //Arrange
        var expression = "5A && 4F";

        // Act
        var result = BexOperatorParser.Parse(expression);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        
        result.Value.Token.Should().Be("&&");
        result.Value.BType.Should().Be(BexType.Operator);
        result.Value.OperatorType.Should().Be(BexOperatorType.LogicalAnd);

        var left = result.Value.LeftOperand;
        left.Should().NotBeNull();
        left.BType.Should().Be(BexType.Literal);
        var leftLiteral = left as BexLiteral;
        leftLiteral.Expected.Should().BeEquivalentTo([0x5A]);
        
        var right = result.Value.RightOperand;
        right.Should().NotBeNull();
        right.BType.Should().Be(BexType.Literal);
        var rightLiteral = right as BexLiteral;
        rightLiteral.Expected.Should().BeEquivalentTo([0x4F]);
    }
    
}