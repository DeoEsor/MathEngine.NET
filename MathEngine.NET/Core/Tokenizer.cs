using System.Globalization;
using System.Text;

namespace MathEngine.NET.Core;

public class Tokenizer : IDisposable
{
    private readonly TextReader _reader;
    private char _currentChar;

    public Tokenizer(TextReader reader)
    {
        _reader = reader;
        Identifier = Guid.NewGuid().ToString();
        NextChar();
        NextToken();
    }

    public Token Token { get; private set; }

    public double Number { get; private set; }

    public string Identifier { get; private set; }

    private void NextChar()
    {
        var ch = _reader.Read();
        _currentChar = ch < 0 ? '\0' : (char)ch;
    }

    public void NextToken()
    {
        while (char.IsWhiteSpace(_currentChar))
            NextChar();

        switch (_currentChar)
        {
            case '\0':
                Token = Token.Eof;
                return;
            case '+':
                NextChar();
                Token = Token.Add;
                return;

            case '-':
                NextChar();
                Token = Token.Subtract;
                return;

            case '*':
                NextChar();
                Token = Token.Multiply;
                return;

            case '/':
                NextChar();
                Token = Token.Divide;
                return;

            case '(':
                NextChar();
                Token = Token.OpenParens;
                return;

            case ')':
                NextChar();
                Token = Token.CloseParens;
                return;

            case ',':
                NextChar();
                Token = Token.Comma;
                return;
        }

        if (char.IsDigit(_currentChar) || _currentChar == '.')
        {
            var sb = new StringBuilder();
            
            var haveDecimalPoint = false;
            
            while (char.IsDigit(_currentChar) || (!haveDecimalPoint && _currentChar == '.'))
            {
                sb.Append(_currentChar);
                haveDecimalPoint = _currentChar == '.';
                NextChar();
            }

            Number = double.Parse(sb.ToString(), CultureInfo.InvariantCulture);
            Token = Token.Number;
            return;
        }

        if (char.IsLetter(_currentChar) || _currentChar == '_')
        {
            var sb = new StringBuilder();

            while (char.IsLetterOrDigit(_currentChar) || _currentChar == '_')
            {
                sb.Append(_currentChar);
                NextChar();
            }

            Identifier = sb.ToString();
            Token = Token.Identifier;
            return;
        }

        throw new InvalidDataException($"Unexpected character: {_currentChar}");
    }

    public void Dispose() => _reader.Dispose();
}