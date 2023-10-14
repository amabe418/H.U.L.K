namespace Interpreter;
public static class Storage
{
    public static List< Dictionary<string, VarToken>> variables = new List<Dictionary<string, VarToken>>();
    public static Dictionary<string, Function> functions = new Dictionary<string, Function>();
    

}


public enum TokenType
{
    Number,
    PlusOperator, //+
    MinusOperator, //-
    MultOperator, // *
    DivideOperator, // /
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
    Bool


}