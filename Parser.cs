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
    public Result Analyze()
    {
        return Expression();
    }



    #region AuxiliarMethods

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
        if (option.GetValue().ToString() == "True")
        {
            return true;
        }
        else /* (option.GetValue().ToString() == "false") return false; */
        {
            return false;
        }
    }


    public int ElsePosition(int ifPosition)
    {
        int position = int.MinValue;
        int ifAmount = 1;
        int elseAmount = 0;
        for (int i = ifPosition + 1; i < tokens.Count; i++)
        {
            if (tokens.ElementAt(i).GetType() == TokenType.IfKeyWord)
            {
                ifAmount++;
            }
            if (tokens.ElementAt(i).GetType() == TokenType.ElseKeyWord)
            {
                elseAmount++;
                if (ifAmount == elseAmount)
                {
                    position = i;
                }
            }
        }
        return position + 1;
    }

    #endregion



    #region ParsingMethod
    public Result Expression()
    {
        Result expressionResult = PreviousExpression();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.AndOperator || currentToken.GetType() == TokenType.OrOperator))
        {
            Token op = currentToken;
            Move(1);
            Result nextExpression = PreviousExpression();
            if (expressionResult.GetType() == TokenType.Bool && nextExpression.GetType() == TokenType.Bool)
            {
                switch (op.GetType())
                {
                    case TokenType.AndOperator:
                        expressionResult.SetValue(Bool(expressionResult) && Bool(nextExpression));
                        break;

                    case TokenType.OrOperator:
                        expressionResult.SetValue(Bool(expressionResult) || Bool(nextExpression));
                        break;

                }
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR!: And or Or comparisons can only be performed between boolean expressions");
            }
        }
        return expressionResult;
    }
    public Result PreviousExpression()
    {
        Result previousExpressionResult = BasicExpression();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.DoubleEqualsOperator || currentToken.GetType() == TokenType.GreatherOrEqualsOperator || currentToken.GetType() == TokenType.GreatherThanOperator || currentToken.GetType() == TokenType.LessOrEqualsOperator || currentToken.GetType() == TokenType.LessThanOperator || currentToken.GetType() == TokenType.DistintOperator))
        {
            Token op = currentToken;
            Move(1);
            Result basicExpression = BasicExpression();
            switch (op.GetType())
            {
                case TokenType.DoubleEqualsOperator: // a==b
                    if (previousExpressionResult.GetType() == basicExpression.GetType())
                    {
                        previousExpressionResult.SetValue(previousExpressionResult.GetValue().ToString() == basicExpression.GetValue().ToString());
                        previousExpressionResult.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between different DataTypes");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.DistintOperator: //a!=b
                    if (previousExpressionResult.GetType() == basicExpression.GetType())
                    {
                        previousExpressionResult.SetValue(previousExpressionResult.GetValue().ToString() != basicExpression.GetValue().ToString());
                        previousExpressionResult.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between different DataTypes");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.GreatherOrEqualsOperator: //a>=b
                    if (previousExpressionResult.GetType() == TokenType.Number && basicExpression.GetType() == TokenType.Number)
                    {
                        previousExpressionResult.SetValue(double.Parse(previousExpressionResult.GetValue().ToString()!) >= double.Parse(basicExpression.GetValue().ToString()!));
                        previousExpressionResult.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between DataTypes different to number");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.GreatherThanOperator: //a>b
                    if (previousExpressionResult.GetType() == TokenType.Number && basicExpression.GetType() == TokenType.Number)
                    {
                        previousExpressionResult.SetValue(double.Parse(previousExpressionResult.GetValue().ToString()!) > double.Parse(basicExpression.GetValue().ToString()!));
                        previousExpressionResult.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between DataTypes different to number");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.LessOrEqualsOperator: //a<=b
                    if (previousExpressionResult.GetType() == TokenType.Number && basicExpression.GetType() == TokenType.Number)
                    {
                        previousExpressionResult.SetValue(double.Parse(previousExpressionResult.GetValue().ToString()!) <= double.Parse(basicExpression.GetValue().ToString()!));
                        previousExpressionResult.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between DataTypes different to number");
                        Environment.Exit(0);
                    }
                    break;

                case TokenType.LessThanOperator: //a<b
                    if (previousExpressionResult.GetType() == TokenType.Number && basicExpression.GetType() == TokenType.Number)
                    {
                        previousExpressionResult.SetValue(double.Parse(previousExpressionResult.GetValue().ToString()!) < double.Parse(basicExpression.GetValue().ToString()!));
                        previousExpressionResult.SetType(TokenType.Bool);
                    }
                    else
                    {
                        System.Console.WriteLine("SYNTAX ERROR!: Comparisons can't be performed between DataTypes different to number");
                        Environment.Exit(0);
                    }
                    break;

            }
        }


        return previousExpressionResult;
    }
    public Result BasicExpression()
    {
        Result basicExpressionResult = Addend();
        while (index < tokens.Count && currentToken.GetType() == TokenType.ConcatOperator)
        {
            Move(1);
            Result addend = Addend();
            if (basicExpressionResult.GetType() == TokenType.String || basicExpressionResult.GetType() == TokenType.Number || addend.GetType() == TokenType.String || addend.GetType() == TokenType.Number)
            {
                basicExpressionResult.SetValue(basicExpressionResult.GetValue().ToString() + " " + addend.GetValue().ToString());
                basicExpressionResult.SetType(TokenType.String);
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR!: Data types that are neither numbers or strings can't be concatenated");
                Environment.Exit(0);
            }
        }
        return basicExpressionResult;
    }


    /* este metodo es para concatenar en caso de que sea necesario */
    public Result Addend()
    {
        Result addendResult = Term();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.PlusOperator || currentToken.GetType() == TokenType.MinusOperator))
        {
            Token op = currentToken;
            Move(1);
            Result term = Term();
            if (addendResult.GetType() == TokenType.Number && term.GetType() == TokenType.Number)
            {
                if (op.GetType() == TokenType.PlusOperator)
                {
                    addendResult.SetValue(double.Parse(addendResult.GetValue().ToString()!) + double.Parse(term.GetValue().ToString()!));
                }
                if (op.GetType() == TokenType.MinusOperator)
                {
                    addendResult.SetValue(double.Parse(addendResult.GetValue().ToString()!) - double.Parse(term.GetValue().ToString()!));
                }
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                Environment.Exit(0);
            }

        }

        return addendResult;

    }
    /* este metodo devuelve el valor de una expresion  compuesta por terminos unidos 
    por los operadores de suma o resta, o un factor en caso de que no sea posible realozar las operaciones */
    public Result Term()
    {
        Result termResult = Factor();
        while (index < tokens.Count && (currentToken.GetType() == TokenType.MultOperator || currentToken.GetType() == TokenType.DivideOperator))
        {
            Token op = currentToken;
            Move(1);
            Result factor = Factor();

            if (termResult.GetType() == TokenType.Number && factor.GetType() == TokenType.Number)
            {
                if (op.GetType() == TokenType.MultOperator)
                {
                    termResult.SetValue(double.Parse(termResult.GetValue().ToString()!) * double.Parse(factor.GetValue().ToString()!));
                }
                if (op.GetType() == TokenType.DivideOperator)
                {
                    termResult.SetValue(double.Parse(termResult.GetValue().ToString()!) * double.Parse(factor.GetValue().ToString()!));
                }
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                Environment.Exit(0);
            }
        }

        return termResult;
    }

    /* este metodo es para devolver expresiones que sean terminos numericos unidos por el operador
    de potencia, o una base, en caso de que no haya operadores de potencia al momento de revisar
    una porcion de expresion*/
    public Result Factor()
    {
        Result factorResult = Base();
        Move(1);
        while (index < tokens.Count && (currentToken.GetType() == TokenType.PowerOperator))
        {
            Token op = currentToken;
            Move(1);
            Result secondBase = Base();
            if (factorResult.GetType() == TokenType.Number && secondBase.GetType() == TokenType.Number)
            {
                factorResult.SetValue(Math.Pow(double.Parse(factorResult.GetValue().ToString()!), double.Parse(secondBase.GetValue().ToString()!)));
            }
            else
            {
                System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                Environment.Exit(0);
            }
        }

        return factorResult;
    }

    /* 
     este metodo revisa si hay un numero, un string, un valor booleano o una expresion y lo devuelve.     

    */
    public Result Base()
    {
        Result baseResult = new Result(TokenType.Null, null!);

        switch (currentToken.GetType())
        {
            case TokenType.Number:
                baseResult.SetValue(currentToken.GetValue());
                baseResult.SetType(TokenType.Number);
                break;

            case TokenType.String:
                baseResult.SetValue(currentToken.GetValue());
                baseResult.SetType(TokenType.String);
                break;

            case TokenType.TrueKeyWord:
                baseResult.SetValue(true);
                baseResult.SetType(TokenType.Bool);
                break;

            case TokenType.FalseKeyWord:
                baseResult.SetValue(false);
                baseResult.SetType(TokenType.Bool);
                break;

            case TokenType.PlusOperator:
                Move(1);
                Result _nextToken = Base();
                if (_nextToken.GetType() == TokenType.Number)
                {
                    baseResult.SetType(TokenType.Number);
                    baseResult.SetValue(0 + double.Parse(_nextToken.GetValue().ToString()!));
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
                    baseResult.SetType(TokenType.Number);
                    baseResult.SetValue(0 - double.Parse(nextToken.GetValue().ToString()!));
                }
                else
                {
                    System.Console.WriteLine("SYNTAX ERROR! Mathematical operations can't be performed between data types that are not numerical");
                    Environment.Exit(0);
                }
                break;
            case TokenType.LeftParenthesisIndicator:
                Move(1);
                baseResult = Expression();

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
                    baseResult.SetValue(Bool(nextExpression));
                    baseResult.SetType(TokenType.Bool);
                    Move(1);
                }
                else
                {
                    System.Console.WriteLine("SYNTAX ERROR!: Negation operator can only be placed before a boolean expression ");
                }
                break;
            case TokenType.IfKeyWord:
                Move(1);
                if (currentToken.GetType() != TokenType.LeftParenthesisIndicator)
                {
                    System.Console.WriteLine("SYNTAX ERROR: Left parenthesis expected.");
                    Environment.Exit(0);

                }
                else
                {
                    baseResult = IfExpression();
                }
                break;
            default:
                System.Console.WriteLine("not expected token");
                Environment.Exit(0);
                break;
        }

        return baseResult;
    }

    public Result IfExpression()
    {
        int ifPosition = index - 1;
        Result ifEvaluation = Expression();
        Result ifResult = null!;
        if (ifEvaluation.GetType() != TokenType.Bool)
        {
            System.Console.WriteLine($"SEMANTIC ERROR: can't implicity convert the type {ifEvaluation.GetType()} into bool");
            Environment.Exit(0);
        }
        else
        {
            if (Bool(ifEvaluation) == true)
            {
                ifResult = Expression();
            }
            else
            {
               index = ElsePosition(ifPosition);
               currentToken = tokens.ElementAt(index);
               ifResult = Expression();
            }
        }

        return ifResult;
    }
    #endregion
}