using MathEngine.NET.Interfaces;

namespace MathEngine.NET.Core.ExpressionTree;

public class NodeVariable : Node
{
    private readonly string _variableName;

    public NodeVariable(string variableName)
    {
        _variableName = variableName;
    }

    public override double Eval(IContext ctx)
    {
        return ctx.ResolveVariable(_variableName);
    }
}