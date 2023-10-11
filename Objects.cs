namespace Interpreter;

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

public class Function
{
    string Name { get; set; }
    List<Token> Argument { get; set; }

    List<Token> Body { get; set; }


    public Function(string name, List<Token> arg, List<Token> body)
    {
        Name = name;
        Argument = arg;
        Body = body;
    }

    int GetArgsAmount()
    {
        int amount = 0;
        foreach (var item in Argument)
        {
            if (item.GetType() == TokenType.CommaIndicator)
            {
             amount++;
            }
        }
        return amount;
    }


}