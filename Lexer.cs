using System.Runtime.Serialization;
using System.Security.Principal;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Interpreter;
/*
   En esta clase se realiza el analisis lexico.
   El análisis léxico es el proceso de convertir una secuencia de caracteres (código fuente) 
   en una secuencia de tokens, que son unidades léxicas con significado en el lenguaje de programación.
   Cada token representa una categoría específica:
   -numeros
   -strings
   -datos booleanos
   -identificadores
   -puntuadores
   -operadores
*/
public abstract class Token
{

    private TokenType Type { get; set; }

    public Token(TokenType type)
    {
        Type = type;
    }

    public virtual TokenType GetType() => Type;

    public abstract object GetValue();

    public virtual string GetName() => throw new NotImplementedException();

    public abstract void SetValue(object value);

}

public class Operator : Token
{
    TokenType Type { get; set; }
    public Operator(TokenType type) : base(type)
    {
        Type = type;
    }

    public override object GetValue()
    {
        throw new NotImplementedException();
    }

    public override void SetValue(object value)
    {
        throw new NotImplementedException();
    }

    public override string ToString() => $"{Type}";
}

public class Indicator : Token
{
    TokenType Type { get; set; }
    public Indicator(TokenType type) : base(type)
    {
        Type = type;
    }

    public override object GetValue()
    {
        throw new NotImplementedException();
    }

    public override void SetValue(object value)
    {
        throw new NotImplementedException();
    }

    public override string ToString() => $"{Type}";
}

public class KeyWord : Token
{
    TokenType Type { get; set; }
    public KeyWord(TokenType type) : base(type)
    {
        Type = type;
    }

    public override object GetValue()
    {
        throw new NotImplementedException();
    }

    public override void SetValue(object value)
    {
        throw new NotImplementedException();
    }

    public override string ToString() => $"{Type}";
}

public class DataType : Token
{
    private object Value { get; set; }

    public DataType(TokenType type, object value) : base(type)
    {
        Value = value;
    }

    public override object GetValue() => Value;

    public override void SetValue(object value) => Value = value;

    public override string ToString()
    {
        TokenType type = GetType();
        object value = GetValue();
        return $"{type} : {value} ";
    }
}


public class VarToken : DataType
{
    private string Name { get; set; }

    public VarToken(TokenType type, object value, string name) : base(type, value)
    {
        Name = name;
    }

    public override string GetName()
    {
        return Name;
    }

    public override string ToString()
    {
        TokenType type = GetType();
        object value = GetValue();
        return $"{type} {Name} = {value}";
    }
}

public enum TokenType
{
    Number,
    PlusOperator,
    MinusOperator,
    MultOperator,
    DivideOperator,
    PowerOperator,
    EqualsOperator,
    DoubleEqualsOperator,
    LessThanOperator,
    LessOrEqualsOperator,
    ConcatOperator,
    GreatherThanOperator,
    GreatherOrEqualsOperator,
    DistintOperator,
    NonOperator,
    AndOperator,
    OrOperator,
    Identifier,
    String,
    LeftParenthesisIndicator,
    RightParenthesisIndicator,
    CommaIndicator,
    SemicolonIndicator,
    EndOfFileIndicator,
    LetKeyWord,
    InKeyWord,
    FunctionKeyWord,
    TrueKeyWord,
    FalseKeyWord,
    IfKeyWord,
    ElseKeyWord,
    Null,
    Bool


}

public class Lexer
{
    private readonly string code;

    private int currentPosition;

    private char currentChar;

    public Lexer(string sourceCode)
    {
        this.code = sourceCode;
        currentPosition = 0;
        currentChar = code[currentPosition];

    }

    private void Move(int positions)
    {
        currentPosition += positions;
        if (currentPosition < code.Length)
            currentChar = code[currentPosition];
    }

    public List<Token> Tokenize()
    {
        List<Token> tokens = new List<Token>();
        int leftParenthesis = 0;
        int rightParenthesis = 0;
        int ifToken = 0;
        int elseToken = 0;

        while (currentPosition < code.Length)
        {

            if (char.IsWhiteSpace(currentChar))
            {
                Move(1);
            }
            else if (char.IsDigit(currentChar))
            {
                AddNumber();
            }
            else if (IsOperator())
            {
                AddOperator();
            }

            else if (char.IsLetter(currentChar) || currentChar == '_')
            {
                AddIdentifier();
            }

            else if (currentChar == '"')
            {
                string quotedString = "";
                Move(1);
                while (currentPosition < code.Length && currentChar != '"')
                {
                    quotedString += currentChar.ToString();
                    Move(1);
                }
                tokens.Add(new DataType(TokenType.String, quotedString));
                Move(1);
            }

            else if (IsIndicator())
            {
                AddIndicator();
            }
        }


        #region AuxiliarMethods



        void AddNumber()
        {
            string number = "";
            while (currentPosition < code.Length && char.IsDigit(currentChar))
            {
                number += currentChar.ToString();
                Move(1);
            }
            tokens.Add(new DataType(TokenType.Number, number));
        }

        bool IsOperator()
        {

            return "+-*/^=<>@|&!".Contains(currentChar);
        }

        void AddOperator()
        {
            switch (currentChar)
            {
                case '+':
                    tokens.Add(new Operator(TokenType.PlusOperator));
                    Move(1);
                    break;

                case '-':
                    tokens.Add(new Operator(TokenType.MinusOperator));
                    Move(1);
                    break;

                case '*':
                    tokens.Add(new Operator(TokenType.MultOperator));
                    Move(1);
                    break;

                case '/':
                    tokens.Add(new Operator(TokenType.DivideOperator));
                    Move(1);
                    break;

                case '^':
                    tokens.Add(new Operator(TokenType.PowerOperator));
                    Move(1);
                    break;

                case '=':
                    Move(1);
                    switch (currentChar)
                    {
                        case '=':
                            tokens.Add(new Operator(TokenType.DoubleEqualsOperator));
                            Move(1);
                            break;
                        default:
                            tokens.Add(new Operator(TokenType.EqualsOperator));
                            break;
                    }

                    break;

                case '<':
                    Move(1);
                    switch (currentChar)
                    {
                        case '=':
                            tokens.Add(new Operator(TokenType.LessOrEqualsOperator));
                            Move(1);
                            break;
                        default:
                            tokens.Add(new Operator(TokenType.LessThanOperator));
                            break;
                    }
                    break;

                case '>':
                    Move(1);
                    switch (currentChar)
                    {
                        case '=':
                            tokens.Add(new Operator(TokenType.GreatherOrEqualsOperator));
                            Move(1);
                            break;
                        default:
                            tokens.Add(new Operator(TokenType.GreatherThanOperator));
                            break;
                    }
                    break;

                case '@':
                    tokens.Add(new Operator(TokenType.ConcatOperator));
                    Move(1);
                    break;
                case '|':
                    tokens.Add(new Operator(TokenType.OrOperator));
                    Move(1);
                    break;
                case '&':
                    tokens.Add(new Operator(TokenType.AndOperator));
                    Move(1);
                    break;
                case '!':
                    Move(1);
                    switch (currentChar)
                    {
                        case '=':
                            tokens.Add(new Operator(TokenType.DistintOperator));
                            Move(1);
                            break;
                        default:
                            tokens.Add(new Operator(TokenType.NonOperator));
                            break;
                    }
                    break;
            }

        }

        void AddIdentifier()
        {
            string identifier = "";
            while (currentPosition < code.Length && (char.IsLetterOrDigit(currentChar) || currentChar == '_') && !char.IsWhiteSpace(currentChar))
            {
                identifier += currentChar.ToString();
                Move(1);
            }
            switch (identifier)
            {
                case "let":
                    tokens.Add(new KeyWord(TokenType.LetKeyWord));

                    break;

                case "in":
                    tokens.Add(new KeyWord(TokenType.InKeyWord));

                    break;

                case "function":
                    tokens.Add(new KeyWord(TokenType.FunctionKeyWord));

                    break;

                case "if":
                    tokens.Add(new KeyWord(TokenType.IfKeyWord));
                    ifToken++;

                    break;

                case "else":
                    tokens.Add(new KeyWord(TokenType.ElseKeyWord));
                    elseToken++;

                    break;

                case "true":
                    tokens.Add(new KeyWord(TokenType.TrueKeyWord));

                    break;

                case "false":
                    tokens.Add(new KeyWord(TokenType.FalseKeyWord));

                    break;
                default:
                    tokens.Add(new VarToken(TokenType.Identifier, null!, identifier));

                    break;

            }


        }

        bool IsIndicator()
        {
            return "(),;".Contains(currentChar);
        }

        void AddIndicator()
        {


            switch (currentChar)
            {
                case '(':
                    tokens.Add(new Indicator(TokenType.LeftParenthesisIndicator));
                    leftParenthesis++;
                    Move(1);
                    break;

                case ')':
                    tokens.Add(new Indicator(TokenType.RightParenthesisIndicator));
                    rightParenthesis++;
                    Move(1);
                    break;

                case ',':
                    tokens.Add(new Indicator(TokenType.CommaIndicator));
                    Move(1);
                    break;

                case ';':
                    tokens.Add(new Indicator(TokenType.SemicolonIndicator));
                    Move(1);
                    break;

            }

        }


        #endregion



        if (leftParenthesis != rightParenthesis)
        {
            System.Console.WriteLine("LEXICAL ERROR: not balanced parenthesis");
            Environment.Exit(0);
        }
        
        if(ifToken!=elseToken)
        {
            System.Console.WriteLine("SYNTAX ERROR: there's an 'else' expression missing");
            Environment.Exit(0);
        }

        if (tokens.ElementAt(tokens.Count - 1).GetType() != TokenType.SemicolonIndicator)
        {
            System.Console.WriteLine("LEXICAL ERROR: ; expected ");
            Environment.Exit(0);
        }

        return tokens;

    }
}