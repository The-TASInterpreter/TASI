//THIS INTERPRETER IS IN A VERY EARLY STATE!


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
            string? location = null;
            if (args.Length == 1)
            {
                location = args[0];
            }

            if (location == null)
            {
                Global.currentLine = -1;
                Console.WriteLine("Doing tests...");
                Tests.NumCalcTests();
                //SyntaxAnalysis.AnalyseSyntax(StringProcess.ConvertLineToCommand("set helloWorld [Console.ReadLine];"));
                Console.ReadKey(false);
                Console.Clear();

                Console.WriteLine("Enter file location with code:");
            }


            Global.InitInternalNamespaces();


            Stopwatch codeRuntime = new();




            //Remove comments 
            try
            {
                if (location == null)
                    location = Console.ReadLine() ?? throw new CodeSyntaxException("Code is null.");
                Global.mainFilePath = Path.GetDirectoryName(location);
                List<Command> commands = LoadFile.ByPath(location);
                codeRuntime.Start();


                var startValues = InterpretMain.InterpretHeaders(commands, Global.mainFilePath);
                Global.currentLine = -1;
                var startCode = startValues.Item1;
                if (startCode == null)
                    if (startValues.Item2.namespaceIntend == NamespaceInfo.NamespaceIntend.library)
                        throw new CodeSyntaxException("You can't start a library-type namespace directly.");
                    else if (startValues.Item2.namespaceIntend == NamespaceInfo.NamespaceIntend.story && startValues.Item2.namespaceFuncitons.Any(x => x.funcName == "start"))
                        startCode = StringProcess.ConvertLineToCommand($"[{startValues.Item2.Name}.start]");
                    else
                        throw new CodeSyntaxException("You need to define a start. You can use the start statement to do so.");


                foreach (NamespaceInfo namespaceInfo in Global.Namespaces) //Activate functioncalls after scanning headers to not cause any errors. BTW im sorry
                {
                    foreach (Function function in namespaceInfo.namespaceFuncitons)
                    {
                        foreach (List<Command> functionCodeOverload in function.functionCode)
                        {
                            foreach (Command overloadCode in functionCodeOverload)
                            {
                                Global.currentLine = overloadCode.commandLine;
                                if (overloadCode.commandType == Command.CommandTypes.FunctionCall) overloadCode.FunctionCall.SearchCallFunction(namespaceInfo);
                                if (overloadCode.commandType == CommandTypes.CodeContainer) overloadCode.initCodeContainerFunctions(namespaceInfo);
                                if (overloadCode.commandType == CommandTypes.Calculation) overloadCode.Calculation.InitFunctions(namespaceInfo);
                            }
                        }

                        foreach (CustomStatement customStatement in function.customStatements)
                        {
                            customStatement.treeElement.ActivateFunctions(namespaceInfo);
                        }

                    }


                }
                foreach (Command command in startCode)
                {
                    Global.currentLine = command.commandLine;
                    if (command.commandType == Command.CommandTypes.FunctionCall) command.FunctionCall.SearchCallFunction(startValues.Item2);
                    if (command.commandType == CommandTypes.Calculation) command.Calculation.InitFunctions(startValues.Item2);
                    if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(startValues.Item2);
                }


                InterpretMain.InterpretNormalMode(startCode, new(new(), startValues.Item2, new()));
                codeRuntime.Stop();
                Console.WriteLine($"Code finished; Runtime: {codeRuntime.ElapsedMilliseconds} ms");
                Console.ReadKey(false);

            }
            catch (Exception ex)
            {

                Console.Clear();

                switch (ex)
                {
                    case CodeSyntaxException:
                        if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1 && new Random().Next(0, 20) == 1) //April fools
                        {
                            Console.WriteLine("There was a syntathical error in your code. But it can't be your fault, it's probably just an interpreter error.");
                            Console.WriteLine("April fools, it's your fault :P");
                            Console.WriteLine("--------------");
                        }
                        Console.WriteLine("There was a syntathical error in your code.");
                        if (Global.currentLine != -1)
                            Console.WriteLine($"\nThe error happened on line: {Global.currentLine + 1}");
                        Console.WriteLine("The error message is:");
                        Console.WriteLine(ex.Message);
                        break;
                    default:
                        Console.WriteLine("There was an internal error in the compiler.");
                        Console.WriteLine("Please report this error on github and please include the code and this error message and (if available) you inputs, that lead to this error. You can create a new issue, reporting the error here:\nhttps://github.com/Ekischleki/TASI/issues/new");
                        if (Global.currentLine != -1)
                            Console.WriteLine($"\nThe error happened on line: {Global.currentLine + 1}");
                        Console.WriteLine("The error message is:");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Here is the stack trace:");
                        Console.WriteLine(ex.StackTrace);
                        break;
                    case RuntimeCodeExecutionFailException runtimeException:
                        Console.WriteLine("The code threw a fail, because it couldn't take it anymore or smt...");
                        Console.WriteLine($"The fail type is:\n{runtimeException.exceptionType}");
                        Console.WriteLine($"The fail message is:\n{runtimeException.Message}");
                        break;
                }


                Console.ReadKey();


            }


            return;


        }
    }
}