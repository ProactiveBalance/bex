using bex.utils;

namespace bex.tree.classes;

public class BexNegativeByteGroupClass(IEnumerable<Bex> items) : BexByteClass("[^", null)
{
    public IList<Bex> Items { get; internal set; } = items.ToList();

    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        Exception? ex = null;
        var captured = Bytes.Empty;
        var success = true;

        {
            foreach (var bexLiteral in Items)
            {
                var bres = await bexLiteral.MatchAsync(ctx, cancellationToken);

                if (bres.Success)
                {
                    success = false;
                    captured = Bytes.Empty;
                    break;
                }
                else
                {
                    captured = bres.Captured;
                }
            }
        }

        return new BexResult(success, -1, captured, this, ex);
    }
}