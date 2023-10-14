using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace Interpreter;


public class Parser
{
    private List<Token> tokens;
    private int index;
    private int varDict = -1;
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
        else
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

            }
            if (ifAmount == elseAmount)
            {
                position = i;
                return position + 1;
            }

        }
        return position + 1;
    }
    public int InPosition(int ifPosition)
    {
        int position = int.MinValue;
        int letAmount = 1;
        int inAmount = 0;
        for (int i = ifPosition + 1; i < tokens.Count; i++)
        {
            if (tokens.ElementAt(i).GetType() == TokenType.LetKeyWord)
            {
                letAmount++;
            }
            if (tokens.ElementAt(i).GetType() == TokenType.InKeyWord)
            {
                inAmount++;

            }
            if (letAmount == inAmount)
            {
                position = i;
                return position + 1;
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
                throw new Exception();
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
                        throw new Exception();
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
                        throw new Exception();
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
                        throw new Exception();
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
                        System.Console.WriteLine("SEMANTIC ERROR!: Comparisons can't be performed between DataTypes different to number");
                        throw new Exception();
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
                        System.Console.WriteLine("SEMANTIC ERROR!: Comparisons can't be performed between DataTypes different to number");
                        throw new Exception();
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
                        System.Console.WriteLine("SEMANTIC ERROR!: Comparisons can't be performed between DataTypes different to number");
                        throw new Exception();
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
                throw new Exception();
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
                throw new Exception();
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
                throw new Exception();
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
                throw new Exception();
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
                Move(1);
                break;

            case TokenType.String:
                baseResult.SetValue(currentToken.GetValue());
                baseResult.SetType(TokenType.String);
                Move(1);
                break;

            case TokenType.TrueKeyWord:
                baseResult.SetValue(true);
                baseResult.SetType(TokenType.Bool);
                Move(1);
                break;

            case TokenType.FalseKeyWord:
                baseResult.SetValue(false);
                baseResult.SetType(TokenType.Bool);
                Move(1);
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
                    throw new Exception();
                }
                Move(1);
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
                    throw new Exception();
                }
                Move(1);
                break;
            case TokenType.LeftParenthesisIndicator:
                Move(1);
                baseResult = Expression();
                if (currentToken.GetType() != TokenType.RightParenthesisIndicator)
                {
                    System.Console.WriteLine("SYNTAX ERROR: Right parenthesis expected");
                    throw new Exception();
                }
                Move(1);
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
                    throw new Exception();
                }
                else
                {
                    baseResult = IfExpression();
                    Move(1);
                }
                break;
            case TokenType.LetKeyWord:
                Move(1);
                baseResult = Variable();
                break;
            case TokenType.Identifier:
                if (varDict > -1)
                {
                    for (int i = varDict; i >= 0; i--)
                    {
                        if (Storage.variables[i].ContainsKey(currentToken.GetName().ToString()!))
                        {
                            baseResult.SetType(Storage.variables[i][currentToken.GetName().ToString()!].GetType());
                            baseResult.SetValue(Storage.variables[i][currentToken.GetName().ToString()!].GetValue());
                            Move(1);
                        }
                    }
                }
                else if (Storage.functions.ContainsKey(currentToken.GetName().ToString()))
                {
                    baseResult = EvaluateFunction(Storage.functions[currentToken.GetName()]);
                    Move(1);
                }
                else
                {
                    System.Console.WriteLine("SYNTAX ERROR: The identifier doesn't exist in the current context");
                    throw new Exception();
                }
                break;
            case TokenType.FunctionKeyWord:
                Move(1);
                CreateFunction();
                System.Console.WriteLine("function created succesfully");
                break;

            default:
                System.Console.WriteLine("not expected token");
                throw new Exception();
                break;
        }

        return baseResult;
    }

    public Result IfExpression()
    {
        int ifPosition = index - 1;
        Result conditionResult = Expression();
        Result ifExpressionResult = null!;
        if (conditionResult.GetType() != TokenType.Bool)
        {
            System.Console.WriteLine($"SEMANTIC ERROR: can't implicity convert the type {conditionResult.GetType()} into bool");
            throw new Exception();
        }


        if (Bool(conditionResult) == true)
        {
            if (currentToken.GetType() == TokenType.IfKeyWord)
            {
                Move(1);
                ifExpressionResult = IfExpression();
            }
            else
            {
                ifExpressionResult = Expression();
            }
        }
        else
        {
            index = ElsePosition(ifPosition);
            currentToken = tokens[index];

            if (currentToken.GetType() == TokenType.IfKeyWord)
            {
                Move(1);
                ifExpressionResult = IfExpression();
            }
            else
            {
                ifExpressionResult = Expression();
            }
        }
        return ifExpressionResult;
    }

    Result Variable()
    {
        int letPosition = index - 1;
        Dictionary<string, VarToken> scopedVariables = new Dictionary<string, VarToken>();
        VarProcess();
        int amount = 1;
        while (currentToken.GetType() == TokenType.CommaIndicator)
        {
            Move(1);
            VarProcess();
            amount++;
        }
        varDict++;
        Storage.variables.Add(scopedVariables);
        index = InPosition(letPosition);
        currentToken = tokens[index];
        Result varResult = Expression();
        /*  while (amount > 0)
         {
             Storage.variables.Remove(Storage.variables.ElementAt(Storage.variables.Count() - amount).Key);
             amount--;
         } */
        Storage.variables.RemoveAt(varDict);
        varDict -= 1;
        return varResult;

        void VarProcess()
        {
            if (currentToken.GetType() != TokenType.Identifier)
            {
                System.Console.WriteLine("SYNTAX ERROR!: there must be an identifier to name a variable with.");
                throw new Exception();
            }

            string name = currentToken.GetName().ToString()!;
            Move(1);
            if (currentToken.GetType() != TokenType.EqualsOperator)
            {
                System.Console.WriteLine("SYNTAX ERROR!: there must be an asignation operator after declaring a variable");
                throw new Exception();
            }
            Move(1);
            if (currentToken.GetType() == TokenType.LetKeyWord)
            {
                System.Console.WriteLine("SYNTAX ERROR! invalid expression term \"let\"");
                throw new Exception();
            }
            Result variable = Expression();
            scopedVariables.Add(name.ToString(), new VarToken(variable.GetType(), variable.GetValue(), name));
        }
    }

    void CreateFunction()
    {
        List<Token> args = new List<Token>();
        List<Token> body = new List<Token>();

        if (currentToken.GetType() != TokenType.Identifier)
        {
            System.Console.WriteLine("SYNTAX ERROR! functions must have a name");
            throw new Exception();
        }
        string functionName = currentToken.GetName();
        Move(1);
        if (currentToken.GetType() != TokenType.LeftParenthesisIndicator)
        {
            System.Console.WriteLine("SYNTAX ERROR! left parenthesis missing");
            throw new Exception();
        }
        AddArgs();
        while (currentToken.GetType() == TokenType.CommaIndicator)
        {
            AddArgs();
        }
        if (currentToken.GetType() != TokenType.RightParenthesisIndicator)
        {
            System.Console.WriteLine("SYNTAX ERROR! right parenthesis missing");
            throw new Exception();
        }
        Move(1);
        if (currentToken.GetType() != TokenType.PointerOperator)
        {
            System.Console.WriteLine("SYNTAX ERROR! pointer missing");
            throw new Exception();
        }
        Move(1);
        while (index < tokens.Count)
        {
            AddBody();
        }

        void AddArgs()
        {
            Move(1);
            args.Add(currentToken);
            Move(1);
        }
        void AddBody()
        {
            body.Add(currentToken);
            Move(1);
        }

        Storage.functions.Add(functionName, new Function(functionName, args, body));


    }

    Result EvaluateFunction(Function function)
    {
        Result functionResult = new Result(TokenType.Null, null!);
        List<Token> tokens = new List<Token>();
        /* 
        el objeto funcion tiene una lista de argumentos y una lista de instrucciones.
        la lista de argumentos lo que tiene son variables que se van a usar en el cuerpo
        de la funcion.
        por lo tanto, como la funcion tiene la forma name(valorarg1, valorarg2);
        lo que va a pasar es que a cada elemento de la lista de argumentos le va a corresponder
        el valor dado en el mismo orden 
        */
        Move(1);
        if (currentToken.GetType() != TokenType.LeftParenthesisIndicator)
        {
            System.Console.WriteLine("SYNTAX ERROR! there must be a left parenthesis to declare the arguments of the function");
        }
        Move(1);
        if (currentToken.GetType() == TokenType.RightParenthesisIndicator)
        {
            System.Console.WriteLine("SYNTAX ERROR! there must be an argument fot the function");
        }

        Dictionary<string, Token> argsValue = new Dictionary<string, Token>();
        Result arg = new Result(TokenType.Null, null!);
        Token token = new DataType(TokenType.Null, null!);
        GetArgs();
        GetTokens();

        Parser parser = new Parser(tokens);
        System.Console.WriteLine(String.Join('\n', tokens));
        functionResult = parser.Analyze();
        return functionResult;

        void GetArgs()
        {
            for (int i = 0; i < function.Argument.Count; i++)
            {
                arg = Expression();
                token.SetValue(arg.GetValue());
                token.SetType(arg.GetType());
                argsValue.Add(function.Argument[i].GetName(), token);
                Move(1);
            }
        }

        void GetTokens()
        {
            for (int i = 0; i < function.Body.Count; i++)
            {
                if (function.Body[i].GetType() == TokenType.Identifier)
                {
                    if (argsValue.ContainsKey(function.Body[i].GetName()))
                    {
                        tokens.Add(argsValue[function.Body[i].GetName()]);
                    }
                    else
                    {
                        tokens.Add(function.Body[i]);
                    }
                }
                else
                {
                    tokens.Add(function.Body[i]);
                }
            }
        }
    }
}
#endregion
