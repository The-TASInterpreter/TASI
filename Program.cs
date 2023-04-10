//THIS INTERPRETER IS IN A VERY EARLY STATE!
// E.U.0001: Still in string mode even at end of line (Try to add a "\"")
// E.U.0002: Still in function mode even at end of line (Try to add a "]")
// E.U.0003: Still in Calculation mode even at end of line (Try to add a ")")
// E.U.0004: Invalid Command.CommandType at function Statement part 2
// E.U.0005: Invalid Command.CommandType at function Statement part 3
// E.U.0006: Invalid CodeContainer direction at function Statement part 3
// E.U.0007: Still in function mode even at end of file/parent function (Try to add a "}")
// E.U.0008: Can't create a variable with the type void
// E.U 0009: Can't create an array with the variable type "Void". Will that even be possible? Idk!

using System.Diagnostics;
using static TASI.Command;

namespace TASI
{
    class TASI_Main
    {





        public const string interpreterVer = "1.0";
        public static Logger interpretInitLog = new();
        public static void Main(string[] args)
        {
            Global.currentLine = -1;
            Console.WriteLine("Doing tests...");
            Tests.NumCalcTests();
            //SyntaxAnalysis.AnalyseSyntax(StringProcess.ConvertLineToCommand("set helloWorld [Console.ReadLine];"));
            Console.ReadKey(false);
            Console.Clear();

            Console.WriteLine("Enter file location with code:");



            Global.InitInternalNamespaces();


            Stopwatch codeRuntime = new();


            //Remove comments 
            try
            {
                string location = Console.ReadLine() ?? throw new Exception("Code is null.");
                Global.mainFilePath = Path.GetDirectoryName(location);
                List<Command> commands = LoadFile.ByPath(location);
                codeRuntime.Start();


                var startValues = InterpretMain.InterpretHeaders(commands);
                Global.currentLine = -1;
                var startCode = startValues.Item1;
                if (startCode == null)
                    if (startValues.Item2.namespaceIntend == NamespaceInfo.NamespaceIntend.library)
                        throw new Exception("You can't start a library-type namespace directly.");
                    else
                        throw new Exception("You need to define a start. You can use the start statement to do so.");


                foreach (NamespaceInfo namespaceInfo in Global.Namespaces) //Activate functioncalls after scanning headers to not cause any errors. BTW im sorry
                {
                    foreach (Function function in namespaceInfo.namespaceFuncitons)
                    {
                        foreach (List<Command> functionCodeOverload in function.functionCode)
                        {
                            foreach (Command overloadCode in functionCodeOverload)
                            {
                                Global.currentLine = overloadCode.commandLine;
                                if (overloadCode.commandType == Command.CommandTypes.FunctionCall) overloadCode.functionCall.SearchCallFunction(namespaceInfo);
                                if (overloadCode.commandType == CommandTypes.CodeContainer) overloadCode.initCodeContainerFunctions(namespaceInfo);
                                if (overloadCode.commandType == CommandTypes.Calculation) overloadCode.calculation.InitFunctions(namespaceInfo);
                            }
                        }
                    }

                }
                foreach (Command command in startValues.Item1)
                {
                    Global.currentLine = command.commandLine;
                    if (command.commandType == Command.CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(startValues.Item2);
                    if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(startValues.Item2);
                    if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(startValues.Item2);
                }


                InterpretMain.InterpretNormalMode(startCode, new(new(), startValues.Item2));
                codeRuntime.Stop();
                Console.WriteLine($"Code finished; Runtime: {codeRuntime.ElapsedMilliseconds} ms");
                Console.ReadKey(false);

            }
            catch (NotImplementedException e)
            {

                Console.Clear();
                Console.WriteLine("There was an error interpreting your code:\n");
                Console.WriteLine(e.Message);
                if (Global.currentLine != -1)
                    Console.WriteLine($"\nThe error happened on line: {Global.currentLine + 1}");
                Console.ReadKey();


            }


            return;


        }
    }
}