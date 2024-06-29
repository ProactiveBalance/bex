using bex.utils;

namespace bex.tree.classes;

public class BexByteClass : Bex
{
    public string Token { get; internal set; }
    
    public Bex? LeftOperand { get; internal set; }
    

    public BexByteClass(string token, Bex? leftOperand)
        : base(BexType.ByteClass)
    {
        Token = token;

        LeftOperand = leftOperand;

        if (LeftOperand != null)
        {
            LeftOperand.Parent = this;
        }
    }

    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        return await ValueTask.FromResult<BexResult>(new BexResult(false, -1, Bytes.Empty, this,
            new NotSupportedException("Base ByteClass should not be called!")));
    }

    public override string ToString()
        => $"{Token}";
}
