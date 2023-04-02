//THIS INTERPRETER IS IN A VERY EARLY STATE!
// E.U.0001: Still in string mode even at end of line (Try to add a "\"")
// E.U.0002: Still in method mode even at end of line (Try to add a "]")
// E.U.0003: Still in NumCalculation mode even at end of line (Try to add a ")")
// E.U.0004: Invalid Command.CommandType at method Statement part 2
// E.U.0005: Invalid Command.CommandType at method Statement part 3
// E.U.0006: Invalid CodeContainer direction at method Statement part 3
// E.U.0007: Still in method mode even at end of file/parent method (Try to add a "}")
// E.U.0008: Can't create a variable with the type void
// E.U 0009: Can't create an array with the variable type "Void". Will that even be possible? Idk!

using System.Diagnostics;
using Text_adventure_Script_Interpreter;

namespace TASI
{
    class TASI_Main
    {





        public static long line;
        public const string interpreterVer = "1.0";
        public static Logger interpretInitLog = new();
        public static void Main(string[] args)
        {
            Console.WriteLine("Doing tests...");
            Tests.NumCalcTests();
            SyntaxAnalysis.AnalyseSyntax(StringProcess.ConvertLineToCommand("set helloWorld [Console.ReadLine];"));
            Console.ReadKey(false);
            Console.Clear();

            Console.WriteLine("Enter file location with code:");



            Global.InitInternalNamespaces();
            string location = Console.ReadLine() ?? throw new Exception("Code is null.");
            if (!File.Exists(location)) throw new Exception("The user entered file doesn't exist.");
            List<string> codeFile = File.ReadAllLines(location).ToList();

            Stopwatch codeRuntime = new();
            codeRuntime.Start();
            string allFileCode = "";
            for (int i = 0; i < codeFile.Count; i++)
            {
                string line = codeFile[i];
                List<LetterByLetterAnalysis> letters = LetterByLetterAnalysis.AnalyseString(line, i + 1);
                foreach (LetterByLetterAnalysis letter in letters)
                {
                    if (letter.lastLetterType == LetterByLetterAnalysis.LastLetterType.statement && letter.letterChar == '#')
                        break;
                    allFileCode += letter.letterChar;
                    
                }
            }
            InterpretMain.InterpretNormalMode(StringProcess.ConvertLineToCommand(allFileCode));
            codeRuntime.Stop();
            Console.WriteLine($"Runtime: {codeRuntime.ElapsedMilliseconds} ms");

            //try
            //{

            //} catch (Exception ex)
            //{
            //   Console.WriteLine("There was an error:");
            // Console.WriteLine(ex.Message);
            //  Console.WriteLine("Press any key to continue.");
            //   Console.ReadKey();
            // }

            return;


        }
    }
}