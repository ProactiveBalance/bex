using System.Collections;
using System.Text;
using bex.utils;

namespace bex.tree;

public class BexGroup : Bex
{
    
    public IList<Bex> Items { get; }
    
    public string? Name { get; }

    
    public BexGroup(IEnumerable<Bex> items, string? name = null) 
        : base(BexType.Group)
    {
        Name = name;
        Items = items.ToList();
        
        for (int i = 0; i<Items.Count; i++)
        {
            var item = Items[i];
            item.Parent = this;

            if (i > 0)
            {
                item.Previous = Items[i - 1];
                item.Previous.Next = item;
            }

            if (i < Items.Count-1)
            {
                item.Next = Items[i + 1];
                item.Next.Previous = item;
            }
        }

        ConcatSuccesiveLiteralsToOne(Items);
    }


    private void ConcatSuccesiveLiteralsToOne(IList<Bex> items)
    {
        var concatedLiterals = new List<Bex>();

        BexLiteral? firstLiteral = null;

        foreach (var item in items)
        {
            if (item is BexLiteral literal)
            {
                if (firstLiteral == null)
                {
                    firstLiteral = literal;
                }
                else
                {
                    firstLiteral.Expected = firstLiteral.Expected.Concat(literal.Expected).ToArray();
                    concatedLiterals.Add(literal);
                    
                    firstLiteral.Next = literal.Next;

                    if (literal.Next != null)
                    {
                        literal.Next.Previous = firstLiteral;
                    }
                }
            }
            else 
            {
                firstLiteral = null;
            }
        }
        
        concatedLiterals.ForEach(cl => items.Remove(cl));
    }

    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        // var startOffset = ctx.Offset;
        var success = false;
        Exception? ex = null;
        var captured = Bytes.Empty;
        
        {
            for (var l=0;l<Items.Count;l++)
            {
                var item = Items[l];
                var itemResult = await item.MatchAsync(ctx, cancellationToken);
                if (itemResult.Success)
                {
                    //ctx.Offset += itemResult.Captured.Count;
                    captured.AddRange(itemResult.Captured);
                    
                    if (itemResult.Exception != null)
                    {
                        captured = Bytes.Empty;
                        return itemResult;
                    }

                    success = true;
                }
                else
                {
                    success = false;
                    break;
                }
            }
        }

        var res = new BexResult(success, -1, captured, this, ex);

        if (success)
        {
            //ctx.Offset += captured.Count;
            
            if (!String.IsNullOrWhiteSpace(Name))
            {
                res.Group[Name] = new BexGroupResult(Name, res);
            }
        }

        return res;
    }


    public override string ToString()
    {
        return "(" + string.Join(" ", Items) + ")";
    }
}