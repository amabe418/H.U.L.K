namespace Interpreter;
public static class Program
{
    public static void Main(string[] args)
    {
        string sourceCode = "5*(4+3)^2";
        Lexer lexer = new Lexer(sourceCode);
        List<Token> tokens = lexer.Tokenize();
        Parser parse = new Parser(tokens);
        object result = parse.Analyze();
        System.Console.WriteLine(result);


    }

}