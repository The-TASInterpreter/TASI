//THIS INTERPRETER IS IN A VERY EARLY STATE!
// E.U.0001: Still in string mode even at end of line (Try to add a "\"")
// E.U.0002: Still in method mode even at end of line (Try to add a "]")
// E.U.0003: Still in NumCalculation mode even at end of line (Try to add a ")")
// E.U.0004: Invalid Command.CommandType at method Statement part 2
// E.U.0005: Invalid Command.CommandType at method Statement part 3
// E.U.0006: Invalid Brace direction at method Statement part 3
// E.U.0007: Still in method mode even at end of file/parent method (Try to add a "}")
// E.U.0008: Can't create a variable with the type void
// E.U 0009: Can't create an array with the variable type "Void". Will that even be possible? Idk!

using System.Security.Cryptography.X509Certificates;

namespace Text_adventure_Script_Interpreter
{
    class Text_adventure_Script_Interpreter_Main
    {
        public static long line;
        public const string interpreterVer = "1.0";
        public static Logger interpretInitLog = new Logger();
        public static void Main(string[] args)
        {




            
            try
            {
                Global.InitInternalNamespaces();
                interpretInitLog.path = "C:\\Users\\ewolf\\AppData\\Roaming\\text_adventure_launcher\\temp\\HarryPotterComicSimVer1.6\\HarryPotterComicSim\\interpretInitLog.txt";
                interpretInitLog.loggerEnabled = true;
                Console.WriteLine(StringProcess.GetConcatInside('[', ']', "[Console.Writeline [Random.Int.Between 1, 10]]"));
                NamespaceInfo testNamespace = new NamespaceInfo(NamespaceInfo.NamespaceIntend.Main, "testNamespace");

                List<Command> lineCommandTest = new List<Command>(StringProcess.ConvertLineToCommand("[TASI.DecFunc \"Main\",\"void\",[SArray.DecArray \"array cum, string cool\"]]"));

                List<CommandLine> testReadFile = new List<CommandLine>(ScanFile.ScanFilePathToCommand("C:\\Users\\ewolf\\AppData\\Roaming\\text_adventure_launcher\\temp\\HarryPotterComicSimVer1.6\\HarryPotterComicSim\\main.TASI"));

                foreach (CommandLine line in testReadFile)
                {
                    line.SaveToFile("C:\\Users\\ewolf\\AppData\\Roaming\\text_adventure_launcher\\temp\\HarryPotterComicSimVer1.6\\HarryPotterComicSim\\cached.txt");
                }
                List<UnspecifiedMethod> testScanMethods = InterpretMain.FindAllMethods(InterpretMain.ConvertCommandLineToCommand(testReadFile));
                InterpretMain.UseFunc(new Command(Command.CommandTypes.VoidMethod, "INF.DefFunc:\"Main\",,\"void\", new array string {\"string\"; \"string_argument\"; \"num\"; \"num_argument\";};"));
                Console.WriteLine("Success!");
                interpretInitLog.Flush();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Error while interpreting:\n");
                if (line != -1)
                    Console.WriteLine("Error at line: " + line + "\n");
                Console.WriteLine(ex.Message);
                
                Console.ReadKey();
            }











            if (false)
            {

                System.ConsoleKeyInfo key;
                GetKeys getKey = new GetKeys();
                Task getKeys = new Task(getKey.GetPressedKeys);
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        key = Console.ReadKey(true);
                        Console.WriteLine(key.Key);
                    }
                    Console.WriteLine(getKey.PressedKey);
                    Console.WriteLine("test");
                    Thread.Sleep(1000);

                }

            }
        }



    }
}