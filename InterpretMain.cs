namespace TASI
{
    internal class InterpretMain
    {
        public static List<NamespaceInfo> allNamespaces = new();
        public static List<Var> allPublicVars = new();



        public static List<VarDef> InterpretVarDef(List<Command> commands)
        {
            bool statementMode = false;
            CommandLine? commandLine = new(new(), -1);
            List<VarDef> result = new();
            foreach (Command command in commands)
            {

                if (statementMode)
                {
                    if (command.commandType == Command.CommandTypes.EndCommand)
                    {
                        if (commandLine.commands.Count != 2) throw new Exception("Invalid VarDef statement.\nRight way of using it:<statemt: var type> <statement: var name>");
                        if (commandLine.commands[0].commandType != Command.CommandTypes.Statement || commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new Exception("Invalid VarDef statement.\nRight way of using it:<statemt: var type> <statement: var name>");
                        if (!Enum.TryParse<VarDef.EvarType>(commandLine.commands[0].commandText.ToLower(), out VarDef.EvarType varType) || commandLine.commands[0].commandText == "return") throw new Exception($"The variable type \"{commandLine.commands[0].commandText.ToLower()}\" is invalid.");
                        result.ForEach(x =>
                        {
                            if (x.varName == commandLine.commands[1].commandText.ToLower()) throw new Exception($"A variable with the name {commandLine.commands[1].commandText.ToLower()}. Keep in mind, that variable names are not case sensitive.");
                        });

                        result.Add(new(varType, commandLine.commands[1].commandText.ToLower()));
                        statementMode = false;
                        continue;
                    }
                    else
                    {
                        commandLine.commands.Add(command);
                        continue;
                    }
                }


                switch (command.commandType)
                {
                    case Command.CommandTypes.Statement:
                        Global.currentLine = command.commandLine;
                        statementMode = true;
                        commandLine = new(new List<Command> { command }, 1);
                        break;
                    default:
                        throw new NotImplementedException($"You can only use statements in VarDef-mode.");
                }
            }
            if (statementMode)
            {
                if (commandLine.commands.Count != 2) throw new Exception("Invalid VarDef statement.\nRight way of using it:<statemt: var type> <statement: var name>");
                if (commandLine.commands[0].commandType != Command.CommandTypes.Statement || commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new Exception("Invalid VarDef statement.\nRight way of using it:<statemt: var type> <statement: var name>");
                if (!Enum.TryParse<VarDef.EvarType>(commandLine.commands[0].commandText.ToLower(), out VarDef.EvarType varType) || commandLine.commands[0].commandText == "return") throw new Exception($"The variable type \"{commandLine.commands[0].commandText.ToLower()}\" is invalid.");
                result.ForEach(x =>
                {
                    if (x.varName == commandLine.commands[1].commandText.ToLower()) throw new Exception($"A variable with the name {commandLine.commands[1].commandText.ToLower()}. Keep in mind, that variable names are not case sensitive.");
                });

                result.Add(new(varType, commandLine.commands[1].commandText.ToLower()));
            }
            return result;
        }

        public static Tuple<List<Command>?, NamespaceInfo> InerpretHeaders(List<Command> commands) //This method will interpret the headers of the file and return the start code.
        {
            bool statementMode = false;
            CommandLine? commandLine = new(new(), -1);
            List<Command>? startCode = null;
            NamespaceInfo thisNamespace = new(NamespaceInfo.NamespaceIntend.nonedef, null);


            foreach (Command command in commands)
            {

                if (statementMode)
                {
                    if (command.commandType == Command.CommandTypes.EndCommand)
                    {
                        switch (commandLine.commands[0].commandText.ToLower())
                        {
                            case "name":
                                if (commandLine.commands.Count != 2) throw new Exception("Invalid usage of name statement.\nCorrect usage: name <statement: name>;");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new Exception("Invalid usage of name statement.\nCorrect usage: name <statement: name>;");
                                if (thisNamespace.Name != null) throw new Exception("Name can't be defined twice.");
                                thisNamespace.Name = commandLine.commands[1].commandText;
                                break;

                            case "type":
                                if (commandLine.commands.Count != 2) throw new Exception("Invalid usage of type statement.\nCorrect usage: type <statement: type>;\nPossible types are: Supervisor, Generic, Internal, Library.");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new Exception("Invalid usage of type statement.\nCorrect usage: type <statement: type>;\nPossible types are: Supervisor, Generic, Internal, Library.");
                                if (thisNamespace.namespaceIntend != NamespaceInfo.NamespaceIntend.nonedef) throw new Exception("Type can't be defined twice.");
                                if (!Enum.TryParse<NamespaceInfo.NamespaceIntend>(commandLine.commands[1].commandText.ToLower(), out NamespaceInfo.NamespaceIntend result)) throw new Exception("Invalid usage of type statement.\nCorrect usage: type <statement: type>;\nPossible types are: Supervisor, Generic, Internal, Library.");
                                thisNamespace.namespaceIntend = result;
                                break;
                            case "start":
                                if (thisNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.nonedef || thisNamespace.Name == null) throw new Exception("You can't start while not having defined namespace name and type.\nYou can use the name and type statement to do this.");
                                if (thisNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.library) throw new Exception("Library type namespaces can't have a start.");

                                if (commandLine.commands.Count != 2) throw new Exception("Invalid usage of start statement.\nCorrect usage: start <code container: start code>;");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.CodeContainer) throw new Exception("Invalid usage of start statement.\nCorrect usage: start <code container: start code>;");
                                if (startCode != null) throw new Exception("Start can't be defined twice.");
                                startCode = commandLine.commands[1].codeContainerCommands;
                                break;
                            case "method":
                                if (commandLine.commands.Count != 5) throw new Exception("Invalid usage of method statement.\nCorrect usage: method <statement: return type> <statement: method name> <code container: semicolon seperated input values> <code container: method code>;\nExample:\nmethod num ReturnRandomChosenNumber {num randomness; num randomnessSeed;}\r\n{\r\nreturn (5984 + ($randomness) / ($randomnessSeed) * ($randomness) / 454);\r\n};");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.Statement || commandLine.commands[2].commandType != Command.CommandTypes.Statement || commandLine.commands[3].commandType != Command.CommandTypes.CodeContainer || commandLine.commands[4].commandType != Command.CommandTypes.CodeContainer) throw new Exception("Invalid usage of method statement.\nCorrect usage: method <statement: return type> <statement: method name> <code container: semicolon seperated input values> <code container: method code>;\nExample:\nmethod num ReturnRandomChosenNumber {num randomness; num randomnessSeed;}\r\n{\r\nreturn (5984 + ($randomness) / ($randomnessSeed) * ($randomness) / 454);\r\n};");


                                if (!Enum.TryParse<VarDef.EvarType>(commandLine.commands[1].commandText.ToLower(), out VarDef.EvarType methodReturnType)) throw new Exception("Method return type is invalid.");
                                Method thisMethod = null;
                                foreach (Method method in thisNamespace.namespaceMethods) //Check if method with name already exist
                                {
                                    if (method.funcName == commandLine.commands[2].commandText.ToLower()) thisMethod = method;
                                }
                                string methodName = commandLine.commands[2].commandText.ToLower();
                                List<VarDef> methodInputVars = InterpretVarDef(commandLine.commands[3].codeContainerCommands ?? throw new Exception("Internal: Code container tokens were not generated."));

                                if (thisMethod != null) //If method with name already exist, check if input combination already exist.
                                {
                                    foreach (List<VarDef> varDefs in thisMethod.methodArguments)
                                    {
                                        bool isEqu = true;
                                        if (methodInputVars.Count == varDefs.Count) continue;
                                        for (int i = 0; i < varDefs.Count; i++)
                                        {
                                            VarDef varDef = varDefs[i];
                                            if (varDef.varType != methodInputVars[i].varType)
                                            {
                                                isEqu = false;
                                                continue;
                                            }
                                        }
                                        if (isEqu) throw new Exception($"The method \"{thisMethod.methodLocation}\" with the exact same input-types has already been defined.");
                                    }
                                    thisMethod.methodArguments.Add(methodInputVars);
                                }
                                else
                                    new Method(methodName, methodReturnType, thisNamespace, new() { methodInputVars }, commandLine.commands[4].codeContainerCommands ?? throw new Exception("Internal: Code container tokens were not generated."));
                                break;
                            case "import":
                                if (commandLine.commands.Count != 2) throw new Exception("Invalid usage of type statement.\nCorrect usage: type <statement: type>;\nPossible types are: Supervisor, Generic, Internal, Library.");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.String) throw new Exception("Invalid usage of import statement.\nCorrect usage: type <statement: type>;\nPossible types are: Supervisor, Generic, Internal, Library.");




                                break;

                            default: throw new Exception($"\"{commandLine.commands[0].commandText}\" isn't a recognized statement in header interpret mode.");

                        }

                        statementMode = false;
                        continue;
                    }
                    commandLine.commands.Add(command);
                    continue;

                }


                switch (command.commandType)
                {

                    case Command.CommandTypes.Statement:
                        Global.currentLine = command.commandLine;
                        statementMode = true;
                        commandLine = new(new List<Command> { command }, 1);
                        break;
                    default:
                        throw new NotImplementedException($"You can only use statements in header-mode.");
                }
            }

            if (thisNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.nonedef || thisNamespace.Name == null) throw new Exception("You need to enter name and type for this namespace. You can do that using the name and type statements.");
            bool found = false;
            foreach (NamespaceInfo searchNamespace in Global.Namespaces)
            {
                if (searchNamespace.Name == thisNamespace.Name)
                {
                    found = true; break;
                }
            }
            if (!found)
                Global.Namespaces.Add(thisNamespace);


            return new(startCode, thisNamespace);
        }








        public static Var InterpretNormalMode(List<Command> commands, AccessableObjects accessableObjects)
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

                        returnValue = Statement.StaticStatement(commandLine, accessableObjects);
                        if (returnValue.varDef.varType == VarDef.EvarType.@return)
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
                        Global.currentLine = command.commandLine;
                        if (command.methodCall == null)
                            throw new Exception("Internal: Method call was not converted to a method call.");

                        returnValue = command.methodCall.DoMethodCall(accessableObjects);
                        if (returnValue.varDef.varType == VarDef.EvarType.@return)
                            return returnValue;
                        break;
                    case Command.CommandTypes.Statement:
                        Global.currentLine = command.commandLine;
                        statementMode = true;
                        commandLine = new(new List<Command> { command }, 1);
                        break;
                    default:
                        throw new NotImplementedException($"You can't use a {command.commandType}-type directly.");
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
