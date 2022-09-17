namespace MathEngine.NET.Interfaces;

public interface IContext
{
    double ResolveVariable(string name);
    double CallFunction(string name, double[] arguments);
}