using MathEngine.NET.Interfaces;

namespace MathEngine.NET.Core.ExpressionTree;

internal sealed class NodeUnary : Node
{
    private readonly Func<double, double> _op;

    private readonly Node _rhs;

    public NodeUnary(Node rhs, Func<double, double> op)
    {
        _rhs = rhs;
        _op = op;
    }

    public override double Eval(IContext ctx)
    {
        var rhsVal = _rhs.Eval(ctx);

        var result = _op(rhsVal);
        return result;
    }
}