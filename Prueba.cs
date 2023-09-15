namespace Interpreter;
public static class Program
{  //me quede en que ya hace todas las expresiones algebraicas y booleanas, parsea if-else 
   // espressions
    public static void Main(string[] args)
    {
        string sourceCode = "if(true) if(false) 1 else if(false) 2 else 3 else 4;";
        Lexer lexer = new Lexer(sourceCode);
        List<Token> tokens = lexer.Tokenize();
        System.Console.WriteLine(String.Join('\n', tokens));
        Parser parse = new Parser(tokens);
        Result result = parse.Analyze();
        System.Console.WriteLine(sourceCode);
        System.Console.WriteLine(result.GetType());
        System.Console.WriteLine(result.GetValue());


    }

}