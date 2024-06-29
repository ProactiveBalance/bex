using bex.utils;

namespace bex.tree;

public class BexFunction(string name, params string[] arguments) : Bex(BexType.Function)
{
    public string Name { get; internal set; } = name;
    public IEnumerable<string> Arguments { get; internal set; } = arguments;

    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        var captured = Bytes.Empty;
        
        return await ValueTask.FromResult(new BexResult(false, -1, captured, this));
    }

    public override string ToString()
        => $"{Name}()";
}
