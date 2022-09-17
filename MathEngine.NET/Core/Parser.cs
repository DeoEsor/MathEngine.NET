using MathEngine.NET.Core.ExpressionTree;
using MathEngine.NET.Exceptions;

namespace MathEngine.NET.Core;

public class Parser
{
    private readonly Tokenizer _tokenizer;

    public Parser(Tokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public Node ParseExpression()
    {
        var expr = ParseAddSubtract();

        if (_tokenizer.Token != Token.Eof)
            throw new SyntaxException("Unexpected characters at end of expression");

        return expr;
    }

    private Node ParseAddSubtract()
    {
        var lhs = ParseMultiplyDivide();

        while (true)
        {
            Func<double, double, double>? op = _tokenizer.Token switch
            {
                Token.Add => (a, b) => a + b,
                Token.Subtract => (a, b) => a - b,
                _ => null
            };

            if (op == null)
                return lhs;

            _tokenizer.NextToken();

            var rhs = ParseMultiplyDivide();

            lhs = new NodeBinary(lhs, rhs, op);
        }
    }

    private Node ParseMultiplyDivide()
    {
        var lhs = ParseUnary();

        while (true)
        {
            Func<double, double, double>? op = _tokenizer.Token switch
            {
                Token.Multiply => (a, b) => a * b,
                Token.Divide => (a, b) => a / b,
                _ => null
            };

            if (op == null)
                return lhs;

            _tokenizer.NextToken();

            var rhs = ParseUnary();

            lhs = new NodeBinary(lhs, rhs, op);
        }
    }


    private Node ParseUnary()
    {
        while (true)
        {
            if (_tokenizer.Token == Token.Add)
            {
                _tokenizer.NextToken();
                continue;
            }

            if (_tokenizer.Token != Token.Subtract)
                return ParseLeaf();

            _tokenizer.NextToken();

            var rhs = ParseUnary();

            return new NodeUnary(rhs, a => -a);
        }
    }

    private Node ParseLeaf()
    {
        switch (_tokenizer.Token)
        {
            case Token.Number:
            {
                var node = new NodeNumber(_tokenizer.Number);
                _tokenizer.NextToken();
                return node;
            }
            case Token.OpenParens:
            {
                _tokenizer.NextToken();

                var node = ParseAddSubtract();

                if (_tokenizer.Token != Token.CloseParens)
                    throw new SyntaxException("Missing close parenthesis");
                _tokenizer.NextToken();

                return node;
            }
            case Token.Identifier:
            {
                var name = _tokenizer.Identifier;
                _tokenizer.NextToken();

                if (_tokenizer.Token != Token.OpenParens)
                    return new NodeVariable(name);

                _tokenizer.NextToken();

                var arguments = new List<Node>();
                while (true)
                {
                    arguments.Add(ParseAddSubtract());

                    if (_tokenizer.Token == Token.Comma)
                    {
                        _tokenizer.NextToken();
                        continue;
                    }

                    break;
                }

                if (_tokenizer.Token != Token.CloseParens)
                    throw new SyntaxException("Missing close parenthesis");
                _tokenizer.NextToken();

                return new NodeFunctionCall(name, arguments.ToArray());
            }
            case Token.Eof:
            case Token.Add:
            case Token.Subtract:
            case Token.Multiply:
            case Token.Divide:
            case Token.CloseParens:
            case Token.Comma:
            default:
                throw new SyntaxException($"Unexpect token: {_tokenizer.Token}");
        }
    }


    #region Convenience Helpers

    public static Node Parse(string str)
    {
        return Parse(new Tokenizer(new StringReader(str)));
    }

    public static Node Parse(Tokenizer tokenizer)
    {
        var parser = new Parser(tokenizer);
        return parser.ParseExpression();
    }

    #endregion
}