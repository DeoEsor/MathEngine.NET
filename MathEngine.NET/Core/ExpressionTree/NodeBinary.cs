using MathEngine.NET.Interfaces;

namespace MathEngine.NET.Core.ExpressionTree;

internal class NodeBinary : Node
{
    private readonly Node _lhs;
    private readonly Func<double, double, double>? _op;
    private readonly Node _rhs;

    public NodeBinary(Node lhs, Node rhs, Func<double, double, double>? op)
    {
        _lhs = lhs;
        _rhs = rhs;
        _op = op;
    }

    public override double Eval(IContext ctx)
    {
        var lhsVal = _lhs.Eval(ctx);
        var rhsVal = _rhs.Eval(ctx);

        var result = _op(lhsVal, rhsVal);
        return result;
    }
}