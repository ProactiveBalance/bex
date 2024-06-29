using bex.tree.operators;

namespace bex.tree;

public static class BexExtensions
{

    public static bool IsLiteral(this Bex bex)
    {
        return bex is BexLiteral;
    }
    
    public static bool IsGroup(this Bex bex)
    {
        return bex is BexGroup;
    }
    
    public static bool IsOperator(this Bex bex)
    {
        return bex is BexOperator;
    }
    
    public static bool IsFunction(this Bex bex)
    {
        return bex is BexFunction;
    }
    
}