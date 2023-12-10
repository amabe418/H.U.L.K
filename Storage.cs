namespace Interpreter;
public static class Storage
{
    public static List< Dictionary<string, Token>> variables = new List<Dictionary<string, Token>>();
    public static Dictionary<string, Function> functions = new Dictionary<string, Function>();
    

}


public enum TokenType
{
    Number,
    PlusOperator, //+
    MinusOperator, //-
    MultOperator, // *
    DivideOperator, // /
    ModuleOperator, //%
    PowerOperator, //^
    EqualsOperator, // =
    DoubleEqualsOperator, // ==
    PointerOperator, //=>
    LessThanOperator, // <
    LessOrEqualsOperator, //<=
    ConcatOperator, //@
    GreatherThanOperator, //>
    GreatherOrEqualsOperator, //>=
    DistintOperator, // !=
    NonOperator, // !
    AndOperator, //&
    OrOperator, // |
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
    Bool,
    Sin,
    Cos,
    Tan,
    Print,
    PI


}