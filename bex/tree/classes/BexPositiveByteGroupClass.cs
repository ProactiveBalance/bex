using bex.utils;

namespace bex.tree.classes;

public class BexPositiveByteGroupClass(IEnumerable<BexLiteral> items) : BexByteClass("[", null)
{
    public IList<BexLiteral> Items { get; internal set; } = items.ToList();

    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        //var startOffset = ctx.Offset;
        Exception? ex = null;
        var captured = Bytes.Empty;
        var success = false;

        {
            foreach (var bexLiteral in Items)
            {
                var bres = await bexLiteral.MatchAsync(ctx, cancellationToken);

                if (bres.Success)
                {
                    success = true;
                    captured = bres.Captured;
                    break;
                }
            }
        }

        return new BexResult(success, -1, captured, this, ex);
    }
}