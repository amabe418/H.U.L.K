namespace Interpreter;

public class Result
{
    TokenType type { get; set; }
    object value { get; set; }

    public Result(TokenType Type, object Value)
    {
        type = Type;
        value = Value;

    }
    public void SetValue(object Value)
    {
        value = Value;
    }

    public void SetType(TokenType Type)
    {
        type = Type;
    }

    public TokenType GetType() => type;

    public object GetValue() => value;

}
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

    public Result Analyze()
    {
        return Expression();
    }

    public Result Expression()
    {
        Result result = Addend();
        while (index < tokens.Count && currentToken.GetType() == TokenType.ConcatOperator)
        {
            Move(1);
            Result addend = Addend();
            if (result.GetType() == TokenType.String || result.GetType() == TokenType.Number || addend.GetType() == TokenType.String || addend.GetType() == TokenType.Number)
            {
              result.SetValue(result.GetValue().ToString() + " " + addend.GetValue().ToString());
              result.SetType(TokenType.String);
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR!: Data types that are neither numbers or strings can't be concatenated");
                Environment.Exit(0);
            }
        }
        return result;
    }



    public Result Addend()
    {
        Result result = Term();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.PlusOperator || currentToken.GetType() == TokenType.MinusOperator))
        {
            Token op = currentToken;
            Move(1);
            Result term = Term();
            if (result.GetType() == TokenType.Number && term.GetType() == TokenType.Number)
            {
                if (op.GetType() == TokenType.PlusOperator)
                {
                    result.SetValue(double.Parse(result.GetValue().ToString()!) + double.Parse(term.GetValue().ToString()!));
                }
                if (op.GetType() == TokenType.MinusOperator)
                {
                    result.SetValue(double.Parse(result.GetValue().ToString()!) - double.Parse(term.GetValue().ToString()!));
                }
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                Environment.Exit(0);
            }

        }

        return result;

    }

    public Result Term()
    {
        Result result = Factor();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.MultOperator || currentToken.GetType() == TokenType.DivideOperator))
        {
            Token op = currentToken;
            Move(1);
            Result factor = Factor();

            if (result.GetType() == TokenType.Number && factor.GetType() == TokenType.Number)
            {
                if (op.GetType() == TokenType.MultOperator)
                {
                    result.SetValue(double.Parse(result.GetValue().ToString()!) * double.Parse(factor.GetValue().ToString()!));
                }
                if (op.GetType() == TokenType.DivideOperator)
                {
                    result.SetValue(double.Parse(result.GetValue().ToString()!) * double.Parse(factor.GetValue().ToString()!));
                }
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                Environment.Exit(0);
            }
        }

        return result;
    }


    public Result Factor()
    {
        Result result = Base();
        Move(1);
        while (index < tokens.Count && (currentToken.GetType() == TokenType.PowerOperator))
        {
            Token op = currentToken;
            Move(1);
            Result secondBase = Base();
            if (result.GetType() == TokenType.Number && secondBase.GetType() == TokenType.Number)
            {
                result.SetValue(Math.Pow(double.Parse(result.GetValue().ToString()!), double.Parse(secondBase.GetValue().ToString()!)));
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                Environment.Exit(0);
            }
        }

        return result;
    }

    /* 
     este metodo revisa si hay un numero, un string, un valor booleano o una expresion y lo devuelve.     

    */
    public Result Base()
    {
        Result result = new Result(TokenType.Null, null!);

        switch (currentToken.GetType())
        {
            case TokenType.Number:
                result.SetValue(currentToken.GetValue());
                result.SetType(TokenType.Number);
                break;

            case TokenType.String:
                result.SetValue(currentToken.GetValue());
                result.SetType(TokenType.String);
                break;

            case TokenType.TrueKeyWord:
                result.SetValue(true);
                result.SetType(TokenType.TrueKeyWord);
                break;

            case TokenType.FalseKeyWord:
                result.SetValue(false);
                result.SetType(TokenType.FalseKeyWord);
                break;

            case TokenType.PlusOperator:
                Move(1);
                Result _nextToken = Base();
                if (_nextToken.GetType() == TokenType.Number)
                {
                    result.SetType(TokenType.Number);
                    result.SetValue(0 + double.Parse(_nextToken.GetValue().ToString()!));
                }
                else
                {
                    System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                    Environment.Exit(0);
                }

                break;

            case TokenType.MinusOperator:
                Move(1);
                Result nextToken = Base();
                if (nextToken.GetType() == TokenType.Number)
                {
                    result.SetType(TokenType.Number);
                    result.SetValue(0 - double.Parse(nextToken.GetValue().ToString()!));
                }
                else
                {
                    System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                    Environment.Exit(0);
                }
                break;
            case TokenType.LeftParenthesisIndicator:
                Move(1);
                result = Expression();

                if (currentToken.GetType() != TokenType.RightParenthesisIndicator)
                {
                    System.Console.WriteLine("SYNTAX ERROR: Right parenthesis expected");
                    Environment.Exit(0);
                }
                break;
            default:
                System.Console.WriteLine("not expected token");
                Environment.Exit(0);
                break;
        }

        return result!;
    }
}