using MathEngine.NET.Interfaces;

namespace MathEngine.NET.Core.ExpressionTree;

public sealed class NodeFunctionCall : Node
{
    private readonly Node[] _arguments;

    private readonly string _functionName;

    public NodeFunctionCall(string functionName, Node[] arguments)
    {
        _functionName = functionName;
        _arguments = arguments;
    }

    public override double Eval(IContext ctx)
    {
        var argValues = new double[_arguments.Length];
        for (var i = 0; i < _arguments.Length; i++)
            argValues[i] = _arguments[i].Eval(ctx);

        return ctx.CallFunction(_functionName, argValues);
    }
}