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

namespace Text_adventure_Script_Interpreter
{
    class Text_adventure_Script_Interpreter_Main
    {





        public static long line;
        public const string interpreterVer = "1.0";
        public static Logger interpretInitLog = new Logger();
        public static void Main(string[] args)
        {
            Console.WriteLine("Doing tests...");
            Tests.NumCalcTests();
            Console.ReadKey(false);
            Console.Clear();

            Console.WriteLine("Write code:");



            Global.InitInternalNamespaces();



            //try
            //{

            bool statementMode = false;
            CommandLine? commandLine = null;
            foreach (Command command in StringProcess.ConvertLineToCommand(Console.ReadLine()))
            {
                if (statementMode)
                {
                    if (command.commandType == Command.CommandTypes.EndCommand)
                    {
                        Statement.StaticStatement(commandLine);
                        statementMode = false;
                        continue;
                    }
                    commandLine.commands.Add(command);
                    continue;
                }


                switch (command.commandType)
                {
                    case Command.CommandTypes.UnknownMethod:
                        new MethodCall(command).DoMethodCall();
                        break;
                    case Command.CommandTypes.Statement:
                        statementMode = true;
                        commandLine = new(new List<Command> { command }, 1);
                        break;
                    default:
                        throw new NotImplementedException($"Internal: Not implemented type: {command.commandType}");
                }
            }
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