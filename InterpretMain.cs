namespace TASI
{
    internal class InterpretMain
    {
        public static List<NamespaceInfo> allNamespaces = new();
        public static List<Var> allPublicVars = new();

        public static Var InterpretNormalMode(List<Command> commands)
        {
            //More or less the core of the language. It uses a Command-List and loops over every command, it then checks the command type and calls the corrosponding internal methods to the code.
            bool statementMode = false;
            Var returnValue;
            CommandLine? commandLine = new(new(), -1);
            foreach (Command command in commands)
            {
                if (statementMode)
                {
                    if (command.commandType == Command.CommandTypes.EndCommand)
                    {
                        if (Global.debugMode)
                        {
                            
                        }

                        returnValue = Statement.StaticStatement(commandLine);
                        if (returnValue.varDef.varType == VarDef.EvarType.Return) 
                            return returnValue;

                        statementMode = false;
                        continue;
                    }
                    commandLine.commands.Add(command);
                    continue;
                }


                switch (command.commandType)
                {
                    case Command.CommandTypes.MethodCall:
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
            return new();
        }
        public static Method? FindMethodUsingMethodPath(string methodPath)
        {
            foreach (Method method in Global.AllMethods)
                if (method.methodLocation == methodPath)
                    return method;
            return null;
        }
    }
}
