namespace MathEngine.NET.Core;

public enum Token
{
    Eof,
    Add,
    Subtract,
    Multiply,
    Divide,
    OpenParens,
    CloseParens,
    Comma,
    Identifier,
    Number
}