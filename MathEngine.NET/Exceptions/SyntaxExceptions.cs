namespace MathEngine.NET.Exceptions;

public class SyntaxException : AggregateException
{
    public SyntaxException(string message) 
        : base(message)
    {
    }
}