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

    public bool Bool(Result option)
    {
        if (option.GetValue().ToString() == "true") return true;
        else if (option.GetValue().ToString() == "falsee") return false;

        return false;
    }
    public Result Analyze()
    {
        return Expression();
    }
    public Result Expression()
    {
        Result result = PreviousExpression();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.AndOperator || currentToken.GetType() == TokenType.OrOperator))
        {
            Token op = currentToken;
            Move(1);
            Result nextExpression = PreviousExpression();
            if (result.GetType() == TokenType.Bool && nextExpression.GetType() == TokenType.Bool)
            {
                switch (op.GetType())
                {
                    case TokenType.AndOperator:
                        result.SetValue(Bool(result) && Bool(nextExpression));
                        break;

                    case TokenType.OrOperator:
                        result.SetValue(Bool(result) || Bool(nextExpression));
                        break;

                }
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR!: And or Or comparisons can only be performed between boolean expressions");
            }
        }
        return result;
    }
    public Result PreviousExpression()
    {
        Result result = BasicExpression();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.DoubleEqualsOperator || currentToken.GetType() == TokenType.GreatherOrEqualsOperator || currentToken.GetType() == TokenType.GreatherThanOperator || currentToken.GetType() == TokenType.LessOrEqualsOperator || currentToken.GetType() == TokenType.LessThanOperator || currentToken.GetType() == TokenType.DistintOperator))
        {
            Token op = currentToken;
            Move(1);
            Result basicExpression = BasicExpression();
            switch (op.GetType())
            {
                case TokenType.DoubleEqualsOperator: // a==b
                    if (result.GetType() == basicExpression.GetType())
                    {
                        result.SetValue(result.GetValue().ToString() == basicExpression.GetValue().ToString());
                        result.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between different DataTypes");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.DistintOperator: //a!=b
                    if (result.GetType() == basicExpression.GetType())
                    {
                        result.SetValue(result.GetValue().ToString() != basicExpression.GetValue().ToString());
                        result.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between different DataTypes");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.GreatherOrEqualsOperator: //a>=b
                    if (result.GetType() == TokenType.Number && basicExpression.GetType() == TokenType.Number)
                    {
                        result.SetValue(double.Parse(result.GetValue().ToString()!) >= double.Parse(basicExpression.GetValue().ToString()!));
                        result.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between DataTypes different to number");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.GreatherThanOperator: //a>b
                    if (result.GetType() == TokenType.Number && basicExpression.GetType() == TokenType.Number)
                    {
                        result.SetValue(double.Parse(result.GetValue().ToString()!) > double.Parse(basicExpression.GetValue().ToString()!));
                        result.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between DataTypes different to number");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.LessOrEqualsOperator: //a<=b
                    if (result.GetType() == TokenType.Number && basicExpression.GetType() == TokenType.Number)
                    {
                        result.SetValue(double.Parse(result.GetValue().ToString()!) <= double.Parse(basicExpression.GetValue().ToString()!));
                        result.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between DataTypes different to number");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.LessThanOperator: //a<b
                    if (result.GetType() == TokenType.Number && basicExpression.GetType() == TokenType.Number)
                    {
                        result.SetValue(double.Parse(result.GetValue().ToString()!) < double.Parse(basicExpression.GetValue().ToString()!));
                        result.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between DataTypes different to number");
                        Environment.Exit(0);
                    }
                    break;

            }
        }


        return result;
    }
    public Result BasicExpression()
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


    /* este metodo es para concatenar en caso de que sea necesario */
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
    /* este metodo devuelve el valor de una expresion  compuesta por terminos unidos 
    por los operadores de suma o resta, o un factor en caso de que no sea posible realozar las operaciones */
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

    /* este metodo es para devolver expresiones que sean terminos numericos unidos por el operador
    de potencia, o una base, en caso de que no haya operadores de potencia al momento de revisar
    una porcion de expresion*/
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
                result.SetType(TokenType.Bool);
                break;

            case TokenType.FalseKeyWord:
                result.SetValue(false);
                result.SetType(TokenType.Bool);
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
            case TokenType.NonOperator:
                Move(1);
                Result nextExpression = Base();
                if (nextExpression.GetType() == TokenType.Bool)
                {
                    result.SetValue(!Bool(nextExpression));
                    result.SetType(TokenType.Bool);
                    Move(1);
                }
                else
                {
                    System.Console.WriteLine("SYNTAX ERROR!: Negation operator can only be placed before a boolean expression ");
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