namespace Text_adventure_Script_Interpreter
{
    internal class InterpretMain
    {
        public static List<NamespaceInfo> allNamespaces = new List<NamespaceInfo>();
        public static List<Var> allPublicVars = new List<Var>();
        public static void InterpretFile(List<string> file)
        {

        }

        public static void InterpretClass(List<string> classText)
        {

        }

        public static void InterpretLine(string line)
        {
            List<Command> command = new List<Command>();
        }


        public static void GetUnknownMethods(List<Command> commandList)
        {

        }

        public static void UseFunc(Command func) //This will be a complex one lol
        {
            string[] funcParts = func.commandText.Split(':');
            string[] funcArgs;
            if (funcParts.Length > 1) 
                funcArgs = funcParts[1].Split(',');
            else
                funcArgs = new string[0];
            List<List<Command>> funcPartsCommand = new List<List<Command>>(funcParts.Length);
            List<Command> currentArg = new List<Command>();
            List<VarDef> methodInput;
            foreach (string funcArg in funcArgs)
            {
                foreach (Command funcPartCommand in StringProcess.ConvertLineToCommand(funcArg))
                    currentArg.Add(funcPartCommand);
                funcPartsCommand.Add(new List<Command>(currentArg));
                currentArg.Clear();
            }

            return;
        }

        public static List<UnspecifiedMethod> FindAllMethods(List<Command> commands)
        {
            Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Searching all methods");
            Command.CommandTypes lastCommandType = Command.CommandTypes.EndCommand;
            int methodDeph = 0;
            int methodDefMode = 0;
            string currentMethodName = "Leopold"; //lol
            List<Command> currentMethod = new List<Command>();
            List<UnspecifiedMethod> result = new List<UnspecifiedMethod>();
            foreach (Command command in commands)
            {
                switch (methodDefMode)
                {
                    case 1:
                        if (command.commandType == Command.CommandTypes.Statement) // The method def should be method <method_name> {
                        {
                            currentMethodName = command.commandText;
                            Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Found new method:\n{command.commandText}");
                            methodDefMode++;
                        }
                        else
                        {
                            Text_adventure_Script_Interpreter_Main.line = command.commandLine;
                            throw new Exception($"Invalid method definition type at statement part 2 ({command.commandType}). The type should be {Command.CommandTypes.Statement}. The method statement should be used like this: \"method <method_name> {{\". E.U.0004");
                        }
                        break;
                    case 2:
                        if (command.commandType == Command.CommandTypes.Brace)
                        {
                            if (command.commandText == "{")
                            {
                                methodDefMode++;
                                methodDeph = 1;
                            }
                            else
                            {
                                Text_adventure_Script_Interpreter_Main.line = command.commandLine;
                                throw new Exception($"Invalid method definition brace at statement part 3 ({command.commandText}). The brace should be {{. The method statement should be used like this: \"method <method_name> {{\". E.U.0006");
                            }

                        }
                        else
                        {
                            Text_adventure_Script_Interpreter_Main.line = command.commandLine;
                            throw new Exception($"Invalid method definition type at statement part 3 ({command.commandType}). The type should be {Command.CommandTypes.Brace}. The method statement should be used like this: \"method <method_name> {{\". E.U.0005");
                        }
                        break;
                    case 3:
                        if (command.commandType == Command.CommandTypes.Brace)
                        {
                            if (command.commandText == "}")
                            {
                                methodDeph--;
                                if (methodDeph == 0)
                                {
                                    result.Add(new UnspecifiedMethod(currentMethodName, currentMethod));
                                    currentMethod.Clear();
                                    methodDefMode = 0;
                                    lastCommandType = command.commandType;
                                    continue;
                                }
                            }
                            else if (command.commandText == "{")
                                methodDeph++;
                        }
                        currentMethod.Add(command);


                        break;
                }
                if (methodDefMode != 0)
                {
                    lastCommandType = command.commandType;
                    continue;
                }

                if ((lastCommandType == Command.CommandTypes.EndCommand || lastCommandType == Command.CommandTypes.Brace) && (command.commandText == "method"))
                    methodDefMode = 1;

                lastCommandType = command.commandType;
            }
            if (methodDefMode != 0)
            {
                Text_adventure_Script_Interpreter_Main.line = -1;
                throw new Exception($"The method \"{currentMethodName}\" doesn't end at the end of the file. E.U.0007");
            }
            return result;
        }

        public static List<Command> ConvertCommandLineToCommand(List<CommandLine> commands)
        {
            List<Command> result = new List<Command>();
            foreach (CommandLine commandLine in commands)
            {
                foreach (Command command in commandLine.commands)
                {
                    result.Add(new Command(command.commandType, command.commandText, commandLine.lineIDX));
                }
            }
            return result;
        }

        public static Method FindMethodUsingMethodPath(string methodPath)
        {
            Method currentmethod;
            foreach (Method method in Global.Namespaces[].namespaceMethods)
            {
                
                currentmethod = method;
                while (true)
                {
                    if (method.methodLocation == methodPath)
                        return method;
                    if (method.subMethods != 0)
                }
            }
        }
        public static Method SearchAllSubmethodsForPath(string methodPath, List<Method> searchMethods)
        {
            foreach (Method method in searchMethods)
            {
                if (method.methodLocation == searchMethods)
                        return method;
                SearchAllSubmethodsForPath(methodPath, method.subMethods)
            }
        }
    }
}
