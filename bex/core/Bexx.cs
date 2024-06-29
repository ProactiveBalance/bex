using System.Buffers;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Runtime.InteropServices.ComTypes;
using bex.parsers;
using bex.tree;
using bex.utils;

namespace bex.core;

public class Bexx
{
    internal string Expression { get; set; }

    internal BexxOptions Options { get; set; }

    internal BexGroup? Compiled { get; set; }

    public Bexx(string expression, BexxOptions? options = default(BexxOptions))
    {
        Expression = expression;
        Options ??= new();
    }
    
    public void Compile()
    {
        var pres = BexHexParser.Compile(Expression);
        if (pres.Success)
        {
            Compiled = pres.Value;
        }
        else
        {
            throw new BexCompileException(pres.Error!);
        }
    }


    public async ValueTask<BexResult> MatchAsync(byte[] bytes,
        CancellationToken cancellationToken = default)
    {
        return await this.MatchAsync(new ReadOnlySequence<byte>(bytes), cancellationToken);
    }
    
    public async ValueTask<BexResult> MatchAsync(ReadOnlySequence<byte> sequence, CancellationToken cancellationToken = default)
    {
        if (Compiled == null)
        {
            this.Compile();
        }

        var reader = PipeReader.Create(sequence);
        var ctx = new BexContext(reader);
        return await Compiled!.MatchAsync(ctx, cancellationToken);
    }

    public async ValueTask<BexResult> MatchAsync(Stream input,
        CancellationToken cancellationToken = default)
    {
        if (Compiled == null)
        {
            this.Compile();
        }

        var readerOptions = new StreamPipeReaderOptions(Options.Pool, Options.BufferSize, Options.MinimumReadSize,
            Options.LeaveOpen, Options.UseZeroByteReads);
        var reader = PipeReader.Create(input, readerOptions);
        var ctx = new BexContext(reader);
        return await Compiled!.MatchAsync(ctx, cancellationToken);
    }
}