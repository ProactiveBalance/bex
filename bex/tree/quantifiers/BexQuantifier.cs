using bex.utils;

namespace bex.tree.quantifiers;

public class BexQuantifier : Bex
{
    public string Token { get; internal set; }
    
    public Bex? LeftOperand { get; internal set; }
    
    public bool Lazy { get; internal set; }
    

    public BexQuantifier(string token, Bex? leftOperand, bool lazy = false)
        : base(BexType.Quantifier)
    {
        Token = token;
        Lazy = lazy;

        LeftOperand = leftOperand;

        if (LeftOperand != null)
        {
            LeftOperand.Parent = this;
        }
    }

    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        return await ValueTask.FromResult(new BexResult(false, -1, Bytes.Empty, this,
            new NotSupportedException("Base Quantifier class should not be called!")));
    }

    public override string ToString()
        => $"{LeftOperand}{Token}";
}
