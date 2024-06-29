using bex.tree.operators;
using bex.utils;

namespace bex.tree.quantifiers;

public class BexZeroOrOneQuantifier : BexQuantifier
{

    public BexZeroOrOneQuantifier(Bex leftOperand, bool lazy = false)
        : base("?", leftOperand, lazy)
    {
        Token += lazy ? "?" : "";
    }

    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        //var startOffst = ctx.Offset;
        var success = false;
        var captured = Bytes.Empty;
        Exception? ex = null;

        if (LeftOperand == null)
        {
            ex = new ArgumentNullException(nameof(LeftOperand));
        }
        else
        {
            var leftRes = await LeftOperand.MatchAsync(ctx, cancellationToken);
            if (leftRes.Success)
            {
                captured = leftRes.Captured;
            }
            
            success = true;
        }

        return new BexResult(success, -1, captured, this, ex);
    }
}