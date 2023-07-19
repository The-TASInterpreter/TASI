//THIS INTERPRETER IS IN A VERY EARLY STATE!


using System.Diagnostics;
using TASI.debug;
using static TASI.Command;

namespace TASI
{
    class TASI_Main
    {





        public const string interpreterVer = "1.0";
        public static Logger interpretInitLog = new();
        public static void Main(string[] args)
        {
            Global global = new Global();
            string? location = null;
            if (args.Length == 1)
            {
                location = args[0];
            }

            if (location == null)
            {
                global.CurrentLine = -1;
                Console.Clear();

                Console.WriteLine("Enter file location with code:");
            }




            Stopwatch codeRuntime = new();


            //Remove comments 
            try
            {
                if (location == null)
                    location = (Console.ReadLine() ?? throw new CodeSyntaxException("Code is null.")).Replace("\"", "");
                global.MainFilePath = Path.GetDirectoryName(location);
                List<Command> commands = LoadFile.ByPath(location, global);

                codeRuntime.Start();


                var startValues = InterpretMain.InterpretHeaders(commands, global.MainFilePath, global);
                Task.WhenAll(global.ProcessFiles).Wait();
                global.CurrentLine = -1;
                var startCode = startValues.Item1;
                if (startCode == null)
                    if (startValues.Item2.namespaceIntend == NamespaceInfo.NamespaceIntend.library)
                        throw new CodeSyntaxException("You can't start a library-type namespace directly.");
                    else
                        throw new CodeSyntaxException("You need to define a start. You can use the start statement to do so.");


                foreach (NamespaceInfo namespaceInfo in global.Namespaces) //Activate functioncalls after scanning headers to not cause any errors. BTW im sorry
                {
                    foreach (Function function in namespaceInfo.namespaceFuncitons)
                    {
                        foreach (List<Command> functionCodeOverload in function.functionCode)
                        {
                            foreach (Command overloadCode in functionCodeOverload)
                            {
                                global.CurrentLine = overloadCode.commandLine;
                                if (overloadCode.commandType == Command.CommandTypes.FunctionCall) overloadCode.functionCall.SearchCallFunction(namespaceInfo, global);
                                if (overloadCode.commandType == CommandTypes.CodeContainer) overloadCode.initCodeContainerFunctions(namespaceInfo, global);
                                if (overloadCode.commandType == CommandTypes.Calculation) overloadCode.calculation.InitFunctions(namespaceInfo, global);
                            }
                        }
                    }

                }
                foreach (Command command in startValues.Item1)
                {
                    global.CurrentLine = command.commandLine;
                    if (command.commandType == Command.CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(startValues.Item2, global);
                    if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(startValues.Item2, global);
                    if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(startValues.Item2, global);
                }
                int line = -1;
                /*
                while (true)
                {
                    ConsoleHelper.ClearConsole();



                    Format.PrintFormatedString(Format.FormatCommands(commands, line).Item1);
                    line++;

                    Console.ResetColor();



                    Console.ReadKey();
                }
                */

                InterpretMain.InterpretNormalMode(startCode, new(new(), startValues.Item2, global));
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
                        if (global.CurrentLine != -1)
                            Console.WriteLine($"\nThe error happened on line: {global.CurrentLine + 1}");
                        Console.WriteLine("The error message is:");
                        Console.WriteLine(ex.Message);
                        break;
                    default:
                    case InternalInterpreterException:
                        Console.WriteLine("There was an internal error in the compiler.");
                        Console.WriteLine("Please report this error on github and please include the code and this error message and (if available) you inputs, that lead to this error. You can create a new issue, reporting the error here:\nhttps://github.com/Ekischleki/TASI/issues/new");
                        if (global.CurrentLine != -1)
                            Console.WriteLine($"\nThe error happened on line: {global.CurrentLine + 1}");
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