using bex.utils;

namespace bex.tree;

public abstract class Bex(BexType btype, long fetchSize = 1)
{
    public BexType BType { get; internal set; } = btype;

    public long FetchSize { get; internal set; } = fetchSize;

    public Bex? Parent { get; internal set; }
    public Bex? Previous { get; internal set; }
    public Bex? Next { get; internal set; }

    internal virtual async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        return await ValueTask.FromResult(new BexResult(false, -1, Bytes.Empty, this));
    }
    
    
}
