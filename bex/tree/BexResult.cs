using System.Collections;
using bex.utils;

namespace bex.tree;

public class BexResult(bool success, long offset, Bytes captured, Bex matcher, Exception? exception = null)
{
    public bool Success { get; internal set; } = success;

    public long Offset { get; internal set; } = offset;
    
    public Bytes Captured { get; internal set; } = captured;

    public BexGroupResultList Group { get; internal set; } = new BexGroupResultList();
        
    public Bex Matcher { get; internal set; } = matcher;
    
    public Exception? Exception { get; internal set; } = exception;
}


public class BexGroupResultList : List<BexGroupResult?>
{
    public BexGroupResult? this[string name]
    {
        get
        {
            return this.FirstOrDefault(x =>
                x != null && x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        internal set
        {
            var idx = this.FindIndex(x => x != null && x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (idx > -1)
            {
                this[idx] = value;
            }
        }
    }
}

public class BexGroupResult(string name, BexResult result) 
{
    public string Name { get; internal set; } = name;

    public BexResult Result { get; internal set; } = result;
    
    public BexGroupResultList Group { get; internal set; } = new BexGroupResultList();
}
