using bex.utils;

namespace bex.tree.operators;

public class BexLogicalOrOperator(Bex? left, Bex? right = null) : BexOperator("||", BexOperatorType.LogicalOr, left, right)
{
    
    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        //var startOffset = ctx.Offset;
        Exception? ex = null;
        bool success = false;
        var captured = Bytes.Empty;
        
        if (LeftOperand == null)
        {
            ex = new ArgumentNullException(nameof(LeftOperand));
        }
        else if (RightOperand == null)
        {
            ex = new ArgumentNullException(nameof(RightOperand));
        }
        else
        {
            var lOpRes = await LeftOperand.MatchAsync(ctx, cancellationToken);

            if (lOpRes.Success)
            {
                success = true;
                captured = lOpRes.Captured;
            }
            else
            {
                var rOpRes = await RightOperand.MatchAsync(ctx, cancellationToken);

                if (rOpRes.Success)
                {
                    success = true;
                    captured = rOpRes.Captured;
                }
            }
        }

        return new BexResult(success, -1, captured, this, ex);
    }

}
