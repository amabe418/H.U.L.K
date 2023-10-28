namespace Interpreter;
// en esta clase estan los tipos de objetos que se utilizan en el proyecto.
// las clases que heredan de token fueron hechos para diferenciar los posibles tipos de datos
// con los que es posible trabajar en este lenguaje.
public abstract class Token
{
    /* 
     este objeto token solo tiene un tipo, y todos los tokens que hereden de el 
     tambien lo tendran  
    */
    private TokenType Type { get; set; }

    public Token(TokenType type)
    {
        Type = type;
    }

    public virtual TokenType GetType() => Type;
    public virtual void SetType(TokenType type) => Type = type;
    public abstract object GetValue();

    public virtual string GetName() => throw new NotImplementedException();

    public abstract void SetValue(object value);



}

//esta clase esta hecha para los operdores +-<>=  ,etc.
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

// esta para los indicadores ();
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
 
 // esta es para las palabras reservadas del lenguaje, 
 // como function, true, if, else.
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

// este es para tipos de datos, como podria ser un numero, un bool.
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

// esta es para las variables.
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

// esta espara el objeto result, que es el tipo de dato con el que trabaja el parser
// tiene un  tipo y un valor, el tipo lo vi necesario para revisar si era posible realizar operaciones
// entre los distintos resultados que se fueran obteniendo. 
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
// este es el objeto funcion, tiene un nombre, una lista de tokens que serian sus argumentos, y otra que seria su cuerpo.
public class Function
{
    string Name { get; set; }
    public List<Token> Argument { get; set; }

    public List<Token> Body { get; set; }


    public Function(string name, List<Token> arg, List<Token> body)
    {
        Name = name;
        Argument = arg;
        Body = body;
    }

}