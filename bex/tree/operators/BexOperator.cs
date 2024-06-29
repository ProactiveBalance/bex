using bex.utils;

namespace bex.tree.operators;

public class BexOperator : Bex
{
    public string Token { get; internal set; }
    public BexOperatorType OperatorType { get; internal set; }
    
    public Bex? LeftOperand { get; internal set; }
    public Bex? RightOperand { get; internal set; }

    public BexOperator(string token, BexOperatorType operatorType, Bex? leftOperand, Bex? rightOperand = null)
        : base(BexType.Operator)
    {
        OperatorType = operatorType;
        
        Token = token;

        LeftOperand = leftOperand;
        RightOperand = rightOperand;

        if (LeftOperand != null)
        {
            LeftOperand.Parent = this;
        }

        if (RightOperand != null)
        {
            RightOperand.Parent = this;
        }
    }

    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        return await ValueTask.FromResult(new BexResult(false, -1, Bytes.Empty, this,
            new NotSupportedException("Base Operator class should not be called!")));
    }

    public override string ToString()
        => $"{LeftOperand}{Token}{RightOperand}";
}
