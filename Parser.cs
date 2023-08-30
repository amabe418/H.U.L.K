namespace Interpreter;
public class Parser
{
    private List<Token> tokens;
    private int index;
    private Token currentToken;

    public Parser(List<Token> Tokens)
    {
        this.tokens = Tokens;
        index = 0;
        currentToken = tokens[index];

    }

    private void Move(int position)
    {
        index += position;
        if (index < tokens.Count)
        {
            currentToken = tokens[index];
        }
    }

    public object Analyze()
    {
        return Expression();
    }

    public object Expression()
    {
        object result = Term();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.PlusOperator || currentToken.GetType() == TokenType.MinusOperator))
        {
            Token op = currentToken;
            Move(1);
            object term = Term();

            if (op.GetType() == TokenType.PlusOperator)
            {
                result = double.Parse(result.ToString()!) + double.Parse(term.ToString()!);
            }
            else if (op.GetType() == TokenType.MinusOperator)
            {
                result = double.Parse(result.ToString()!) - double.Parse(term.ToString()!);
            }

        }

        return result;

    }

    public object Term()
    {
        object result = Factor();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.MultOperator || currentToken.GetType() == TokenType.DivideOperator))
        {
            Token op = currentToken;
            Move(1);
            object factor = Factor();

            if (op.GetType() == TokenType.MultOperator)
            {
                result = double.Parse(result.ToString()!) * double.Parse(factor.ToString()!);
            }
            else if (op.GetType() == TokenType.DivideOperator)
            {
                result = double.Parse(result.ToString()!) / double.Parse(factor.ToString()!);
            }

        }

        return result;
    }


    public object Factor()
    {
        object result = Base();
        Move(1);
        while (index < tokens.Count && (currentToken.GetType() == TokenType.PowerOperator))
        {
            Token op = currentToken;
            Move(1);
            result = Math.Pow(double.Parse(result.ToString()!), double.Parse(Factor().ToString()!));
        }

        return result;
    }

    public object Base()
    {
        object result = null!;

        switch (currentToken.GetType())
        {
            case TokenType.Number:
                result = currentToken.GetValue();
                break;
                case TokenType.String:
                result  = currentToken.GetValue();
                break;
            case TokenType.LeftParenthesisIndicator:
                Move(1);
                result = Expression();

                if (currentToken.GetType() != TokenType.RightParenthesisIndicator)
                {
                    System.Console.WriteLine("SYNTAX ERROR");
                }
                break;
            default:
                System.Console.WriteLine("not expected token");
                break;
        }

        return result!;
    }
}