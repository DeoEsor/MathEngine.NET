using MathEngine.NET.Interfaces;

namespace MathEngine.NET.Core.ExpressionTree;

internal sealed class NodeNumber : Node
{
    private readonly double _number;

    public NodeNumber(double number)
    {
        _number = number;
    }

    public override double Eval(IContext ctx)
    {
        return _number;
    }
}