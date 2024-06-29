namespace bex.core;

public class BexException(string message, Exception? inner = null) : ApplicationException(message, inner)
{
    
}