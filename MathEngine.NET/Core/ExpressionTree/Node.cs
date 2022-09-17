using MathEngine.NET.Interfaces;

namespace MathEngine.NET.Core.ExpressionTree;

public abstract class Node
{
    public abstract double Eval(IContext ctx);
}