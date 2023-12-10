namespace Interpreter;
public static class Program
{
    public static void Main(string[] args)
    {
        Run();
        //Test();
    }

    static void Run()
    {
        while (true)
        {
            try
            {
                Console.Write(">");
                string sourceCode = Console.ReadLine()!;
                if (sourceCode == "exit")
                {
                    Environment.Exit(0);
                }
                Lexer lexer = new Lexer(sourceCode!);
                List<Token> tokens = lexer.Tokenize();
                Parser parse = new Parser(tokens);
                Result result = parse.Analyze();
                System.Console.WriteLine(result.GetValue());
            }
            catch (Exception)
            {
                continue;
            }
        }
    }
    static void Test()
    {
        string[] sourceCode = new string[2];

        sourceCode[0] = "function amalia(a) => if(1>=a) 1 else amalia(a-1) + amalia(a-2);";
        sourceCode[1] = " let a=4 in amalia();";
        for (int i = 0; i < sourceCode.Length; i++)
        {
            string sourceCodem = sourceCode[i];
            Lexer lexer = new Lexer(sourceCodem);
            List<Token> tokens = lexer.Tokenize();
            Parser parse = new Parser(tokens);
            Result result = parse.Analyze();
            System.Console.WriteLine(result.GetValue());
            Console.ReadLine();
        }

    }

}