//THIS INTERPRETER IS IN A VERY EARLY STATE!


using System.Diagnostics;
using TASI.debug;
using TASI.Exceptions;
using TASI.InternalLangCoreHandle;
using TASI.RuntimeObjects.FunctionClasses;
using static TASI.Command;

namespace TASI.InterpretStartup
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
            else if (args.Length != 0)
            {
                ArgCheck.InterpretArguments(ArgCheck.TokeniseArgs(args, ArgCheck.argCommandsDefinitions), global);
                PluginManager.PluginManager.CheckPlugins(global.Plugins);
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
                PluginManager.PluginManager.InitFunctionPlugins(global.Plugins, global);

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
                                if (overloadCode.commandType == CommandTypes.FunctionCall) overloadCode.functionCall.SearchCallFunction(namespaceInfo, global);
                                if (overloadCode.commandType == CommandTypes.CodeContainer) overloadCode.initCodeContainerFunctions(namespaceInfo, global);
                                if (overloadCode.commandType == CommandTypes.Calculation) overloadCode.calculation.InitFunctions(namespaceInfo, global);
                            }
                        }
                    }

                }
                foreach (Command command in startValues.Item1)
                {
                    global.CurrentLine = command.commandLine;
                    if (command.commandType == CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(startValues.Item2, global);
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
                AccessableObjects accessableObjects = new(new(), startValues.Item2, global);
                PluginManager.PluginManager.InitBeforeRuntimePlugins(global.Plugins, accessableObjects);
                InterpretMain.InterpretNormalMode(startCode, accessableObjects);
                codeRuntime.Stop();
                Console.WriteLine($"Code finished; Runtime: {codeRuntime.ElapsedMilliseconds} ms");
                Console.ReadKey(false);

            }
            catch (Exception ex)
            {

                Console.Clear();

                switch (ex)
                {

                    case FaultyPluginException faultyPluginException:
                        Console.WriteLine("You tried to load a plugin which was faulty or could not be loaded for another reason.");

                        Console.WriteLine($"Error: {faultyPluginException.Message}");
                        if (faultyPluginException.faultyPlugin.CompatibilityVersion == PluginManager.PluginManager.PLUGIN_COMPATIBILITY_VERSION)
                            Console.WriteLine("This plugin is on the current plugin version, so it should work. Please contact the Author if this is a problem on the plugin-side or the TASI developers if this is a plugin manager problem.");
                        else if (faultyPluginException.faultyPlugin.CompatibilityVersion < PluginManager.PluginManager.PLUGIN_COMPATIBILITY_VERSION && faultyPluginException.faultyPlugin.CompatibilityVersion >= PluginManager.PluginManager.OLDEST_SUPPORTED_PLUGIN_COMPATIBILITY_VERSION)
                            Console.WriteLine("This plugin isn't on the current plugin version but it is still supported, so it should work. Please contact the Author if this is a problem on the plugin-side or the TASI developers if this is a plugin manager problem.");
                        else if (faultyPluginException.faultyPlugin.CompatibilityVersion < PluginManager.PluginManager.OLDEST_SUPPORTED_PLUGIN_COMPATIBILITY_VERSION)
                            Console.WriteLine("This plugin version is no longer supported. Please check for an update from the developer or download an older version of the interpreter.");


                        Console.WriteLine($"Plugin name: {faultyPluginException.faultyPlugin.Name}\nDescription: {faultyPluginException.faultyPlugin.Description}\nVersion: {faultyPluginException.faultyPlugin.Version}\nAuthor: {faultyPluginException.faultyPlugin.Author}\nPlugin Compatibility version: {faultyPluginException.faultyPlugin.CompatibilityVersion}\nPlugin manager Compatibility version: {PluginManager.PluginManager.PLUGIN_COMPATIBILITY_VERSION}");

                        break;
                    case InternalPluginException internalPluginException:
                        Console.WriteLine("There was an internal plugin error.");
                        Console.WriteLine($"Error: {internalPluginException.Message}");
                        Console.WriteLine($"Plugin name: {internalPluginException.plugin.Name}\nDescription: {internalPluginException.plugin.Description}\nVersion: {internalPluginException.plugin.Version}\nAuthor: {internalPluginException.plugin.Author}\nPlugin Compatibility version: {internalPluginException.plugin.CompatibilityVersion}\nPlugin manager Compatibility version: {PluginManager.PluginManager.PLUGIN_COMPATIBILITY_VERSION}");
                        break;

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