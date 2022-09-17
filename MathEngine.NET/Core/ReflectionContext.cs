using MathEngine.NET.Interfaces;

namespace MathEngine.NET.Core;

public sealed class ReflectionContext : IContext
{
    private readonly object _targetObject;

    public ReflectionContext(object targetObject)
    {
        _targetObject = targetObject;
    }

    public double ResolveVariable(string name)
    {
        var pi = _targetObject
            .GetType()
            .GetProperty(name);

        if (pi == null)
            throw new InvalidDataException($"Unknown variable: '{name}'");

        return (double)pi.GetValue(_targetObject);
    }

    public double CallFunction(string name, double[] arguments)
    {
        var mi = _targetObject.GetType().GetMethod(name);
        if (mi == null)
            throw new InvalidDataException($"Unknown function: '{name}'");

        var argObjs = arguments.Select(x => (object)x).ToArray();

        return (double)mi.Invoke(_targetObject, argObjs);
    }
}