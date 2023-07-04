namespace TASI
{
    internal class InterpretMain
    {
        public static List<NamespaceInfo> allNamespaces = new();
        public static List<Var> allPublicVars = new();



        /*
        public static List<VarConstruct> InterpretCopyVar(List<Command> commands)
        {
            bool statementMode = false;
            CommandLine? commandLine = new(new(), -1);
            List<VarConstruct> result = new();

            if (commands.Last().commandType != Command.CommandTypes.EndCommand) commands.Add(new(Command.CommandTypes.EndCommand, ";", commands.Last().commandLine));

            foreach (Command command in commands)
            {

                if (statementMode)
                {
                    if (command.commandType != Command.CommandTypes.EndCommand)
                    {
                        commandLine.commands.Add(command);
                        continue;
                    }

                    if (commandLine.commands.Count != 2 && commandLine.commands[0].commandType == Command.CommandTypes.Statement && commandLine.commands[1].commandType == Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid VarCopy statement.\nRight way of using it:<statemt: var one> <statement: var two>;");
                    if (commandLine.commands.Count == 3) // Is link
                    {
                        if (commandLine.commands[1].commandType != Command.CommandTypes.Statement || commandLine.commands[2].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid VarConstruct link statement.\nRight way of using it:link <statemt: var type> <statement: var name>;");
                        if (!Enum.TryParse<VarConstruct.VarType>(commandLine.commands[1].commandText.ToLower(), out VarConstruct.VarType varType)) throw new CodeSyntaxException($"The variable type \"{commandLine.commands[0].commandText.ToLower()}\" is invalid.");
                        result.ForEach(x =>
                        {
                            if (x.name == commandLine.commands[1].commandText.ToLower()) throw new CodeSyntaxException($"A variable with the name {commandLine.commands[1].commandText.ToLower()} already exists. Keep in mind, that variable names are not case sensitive.");
                        });
                        result.Add(new(varType, commandLine.commands[2].commandText.ToLower(), true));
                    }
                    else
                    {
                        if (commandLine.commands[0].commandType != Command.CommandTypes.Statement || commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid VarConstruct statement.\nRight way of using it:<statemt: var type> <statement: var name>;");
                        if (!Enum.TryParse<VarConstruct.VarType>(commandLine.commands[0].commandText.ToLower(), out VarConstruct.VarType varType)) throw new CodeSyntaxException($"The variable type \"{commandLine.commands[0].commandText.ToLower()}\" is invalid.");
                        result.ForEach(x =>
                        {
                            if (x.name == commandLine.commands[1].commandText.ToLower()) throw new CodeSyntaxException($"A variable with the name {commandLine.commands[1].commandText.ToLower()} already exists. Keep in mind, that variable names are not case sensitive.");
                        });

                        result.Add(new(varType, commandLine.commands[1].commandText.ToLower()));

                    }
                    statementMode = false;
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
                        throw new NotImplementedException($"You can only use statements in VarConstruct-mode.");
                }
            }
            return result;
        }

        */


        public static List<VarConstruct> InterpretVarDef(List<Command> commands)
        {
            bool statementMode = false;
            CommandLine? commandLine = new(new(), -1);
            List<VarConstruct> result = new();
            if (commands.Last().commandType != Command.CommandTypes.EndCommand) commands.Add(new(Command.CommandTypes.EndCommand, ";", commands.Last().commandLine));

            foreach (Command command in commands)
            {

                if (statementMode)
                {
                    if (command.commandType != Command.CommandTypes.EndCommand)
                    {
                        commandLine.commands.Add(command);
                        continue;
                    }
                    //Statenent is complete
                    if (commandLine.commands.Count != 2 && commandLine.commands.Count != 3 && commandLine.commands[0].commandType == Command.CommandTypes.Statement && commandLine.commands[0].commandText == "link") throw new CodeSyntaxException("Invalid VarConstruct statement.\nRight way of using it:<statemt: var type> <statement: var name>;");
                    if (commandLine.commands.Count == 3) // Is link
                    {
                        if (commandLine.commands[1].commandType != Command.CommandTypes.Statement || commandLine.commands[2].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid VarConstruct link statement.\nRight way of using it:link <statemt: var type> <statement: var name>;");
                        if (!Enum.TryParse<VarConstruct.VarType>(commandLine.commands[1].commandText.ToLower(), out VarConstruct.VarType varType)) throw new CodeSyntaxException($"The variable type \"{commandLine.commands[0].commandText.ToLower()}\" is invalid.");
                        result.ForEach(x =>
                        {
                            if (x.name == commandLine.commands[1].commandText.ToLower()) throw new CodeSyntaxException($"A variable with the name {commandLine.commands[1].commandText.ToLower()} already exists. Keep in mind, that variable names are not case sensitive.");
                        });
                        result.Add(new(varType, commandLine.commands[2].commandText.ToLower(), true));
                    }
                    else
                    {
                        if (commandLine.commands[0].commandType != Command.CommandTypes.Statement || commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid VarConstruct statement.\nRight way of using it:<statemt: var type> <statement: var name>;");
                        if (!Enum.TryParse<VarConstruct.VarType>(commandLine.commands[0].commandText.ToLower(), out VarConstruct.VarType varType)) throw new CodeSyntaxException($"The variable type \"{commandLine.commands[0].commandText.ToLower()}\" is invalid.");
                        result.ForEach(x =>
                        {
                            if (x.name == commandLine.commands[1].commandText.ToLower()) throw new CodeSyntaxException($"A variable with the name {commandLine.commands[1].commandText.ToLower()} already exists. Keep in mind, that variable names are not case sensitive.");
                        });

                        result.Add(new(varType, commandLine.commands[1].commandText.ToLower()));

                    }
                    statementMode = false;
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
                        throw new NotImplementedException($"You can only use statements in VarConstruct-mode.");
                }
            }
            return result;
        }

        public static Tuple<List<Command>?, NamespaceInfo> InterpretHeaders(List<Command> commands, string currentFile) //This function will interpret the headers of the file and return the start code.
        {
            bool statementMode = false;
            CommandLine? commandLine = new(new(), -1);
            List<Command>? startCode = null;
            NamespaceInfo thisNamespace = new(NamespaceInfo.NamespaceIntend.nonedef, null);
            List<string> alreadyImportedNamespaces = new();
            Global.Namespaces.Add(thisNamespace);

            foreach (Command command in commands)
            {

                if (statementMode)
                {
                    if (command.commandType == Command.CommandTypes.EndCommand)
                    {
                        switch (commandLine.commands[0].commandText.ToLower())
                        {
                            case "name":
                                if (commandLine.commands.Count != 2) throw new CodeSyntaxException("Invalid usage of name statement.\nCorrect usage: name <statement: name>;");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid usage of name statement.\nCorrect usage: name <statement: name>;");
                                if (thisNamespace.Name != null) throw new CodeSyntaxException("Name can't be defined twice.");
                                thisNamespace.Name = commandLine.commands[1].commandText;
                                break;

                            case "type":
                                if (commandLine.commands.Count != 2) throw new CodeSyntaxException("Invalid usage of type statement.\nCorrect usage: type <statement: type>;\nPossible types are: Supervisor, Generic, Internal, Library.");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid usage of type statement.\nCorrect usage: type <statement: type>;\nPossible types are: Supervisor, Generic, Internal, Library.");
                                if (thisNamespace.namespaceIntend != NamespaceInfo.NamespaceIntend.nonedef) throw new CodeSyntaxException("Type can't be defined twice.");
                                if (commandLine.commands[1].commandText.ToLower() == "tutorial0")
                                    Tutorial.TutorialPhase0();
                                if (commandLine.commands[1].commandText.ToLower() == "tutorial1")
                                    Tutorial.TutorialPhase1(commands);
                                if (!Enum.TryParse<NamespaceInfo.NamespaceIntend>(commandLine.commands[1].commandText.ToLower(), out NamespaceInfo.NamespaceIntend result)) throw new CodeSyntaxException("Invalid usage of type statement.\nCorrect usage: type <statement: type>;\nPossible types are: Supervisor, Generic, Internal, Library.");
                                thisNamespace.namespaceIntend = result;
                                break;
                            case "start":
                                if (thisNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.nonedef || thisNamespace.Name == null)
                                {
                                    Tutorial.TutorialPhaseMinusOne();
                                    throw new CodeSyntaxException("You can't start while not having defined namespace name and type.\nYou can use the name and type statement to do this.");
                                }
                                if (thisNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.library) throw new CodeSyntaxException("Library type namespaces can't have a start.");

                                if (commandLine.commands.Count != 2) throw new CodeSyntaxException("Invalid usage of start statement.\nCorrect usage: start <code container: start code>;");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.CodeContainer) throw new CodeSyntaxException("Invalid usage of start statement.\nCorrect usage: start <code container: start code>;");
                                if (startCode != null) throw new CodeSyntaxException("Start can't be defined twice.");
                                startCode = commandLine.commands[1].codeContainerCommands;
                                break;
                            case "function":
                                if (commandLine.commands.Count != 5) throw new CodeSyntaxException("Invalid usage of function statement.\nCorrect usage: function <statement: return type> <statement: function name> <code container: semicolon seperated input values> <code container: function code>;\nExample:\nfunction num ReturnRandomChosenNumber {num randomness; num randomnessSeed;}\r\n{\r\nreturn (5984 + ($randomness) / ($randomnessSeed) * ($randomness) / 454);\r\n};");
                                if (commandLine.commands[1].commandType != Command.CommandTypes.Statement || commandLine.commands[2].commandType != Command.CommandTypes.Statement || commandLine.commands[3].commandType != Command.CommandTypes.CodeContainer || commandLine.commands[4].commandType != Command.CommandTypes.CodeContainer) throw new CodeSyntaxException("Invalid usage of function statement.\nCorrect usage: function <statement: return type> <statement: function name> <code container: semicolon seperated input values> <code container: function code>;\nExample:\nfunction num ReturnRandomChosenNumber {num randomness; num randomnessSeed;}\r\n{\r\nreturn (5984 + ($randomness) / ($randomnessSeed) * ($randomness) / 454);\r\n};");


                                if (!Enum.TryParse<VarConstruct.VarType>(commandLine.commands[1].commandText.ToLower(), out VarConstruct.VarType functionReturnType)) throw new CodeSyntaxException("function return type is invalid.");
                                Function? thisFunction = null;
                                foreach (Function function in thisNamespace.namespaceFuncitons) //Check if function with name already exist
                                {
                                    if (function.funcName == commandLine.commands[2].commandText.ToLower()) thisFunction = function;
                                }
                                string functionName = commandLine.commands[2].commandText.ToLower();
                                List<VarConstruct> functionInputVars = InterpretVarDef(commandLine.commands[3].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container tokens were not generated."));

                                if (thisFunction != null) //If function with name already exist, check if input combination already exist.
                                {
                                    foreach (List<VarConstruct> varDefs in thisFunction.functionArguments)
                                    {
                                        bool isEqu = true;
                                        if (functionInputVars.Count != varDefs.Count)
                                            continue;
                                        for (int i = 0; i < varDefs.Count; i++)
                                        {
                                            VarConstruct varDef = varDefs[i];
                                            if (varDef.type != functionInputVars[i].type)
                                            {
                                                isEqu = false;
                                                continue;
                                            }
                                        }
                                        if (isEqu) throw new CodeSyntaxException($"The function \"{thisFunction.functionLocation}\" with the exact same input-types has already been defined.");
                                    }
                                    thisFunction.functionArguments.Add(functionInputVars);
                                }
                                else
                                    new Function(functionName, functionReturnType, thisNamespace, new() { functionInputVars }, commandLine.commands[4].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container tokens were not generated."));
                                break;
                            case "import":
                                string pathLocation;
                                switch (commandLine.commands.Count)
                                {
                                    case 2:
                                        if (commandLine.commands[1].commandType != Command.CommandTypes.String) throw new CodeSyntaxException("Invalid usage of import statement.\nCorrect usage: import <string: path>;\nor\nimport base <string: path>;");
                                        pathLocation = commandLine.commands[1].commandText.ToLower();
                                        break;
                                    case 3:
                                        if (commandLine.commands[1].commandType != Command.CommandTypes.Statement || commandLine.commands[1].commandText.ToLower() != "base" || commandLine.commands[2].commandType != Command.CommandTypes.String) throw new CodeSyntaxException("Invalid usage of import statement.\nCorrect usage: import <string: path>;\nor\nimport base <string: path>;");
                                        pathLocation = Path.Combine(Global.mainFilePath, commandLine.commands[2].commandText.ToLower());
                                        break;
                                    default:
                                        throw new CodeSyntaxException("Invalid usage of import statement.\nCorrect usage: import < string: path >;\nor\nimport base < string: path >;");
                                }
                                lock (Global.importFileLock)
                                {
                                    if (!Global.allLoadedFiles.Any(x => ComparePaths(x, pathLocation)))
                                    {
                                        alreadyImportedNamespaces.Add(pathLocation);
                                        string pathLocationCopy = pathLocation;
                                        Global.allLoadedFiles.Add(pathLocationCopy);
                                        Global.Namespaces.Add(new(NamespaceInfo.NamespaceIntend.nonedef, ""));
                                        Global.processFiles.Add(Task.Run(() =>
                                        {
                                            var importNamespace = InterpretHeaders(LoadFile.ByPath(pathLocationCopy, false), pathLocationCopy);
                                            Global.Namespaces[Global.allLoadedFiles.FindIndex(x => ComparePaths(x, pathLocation))] = importNamespace.Item2;

                                            thisNamespace.accessableNamespaces.Add(importNamespace.Item2);

                                        }));
                                    }
                                    else
                                    {

                                        if (alreadyImportedNamespaces.Any(a => ComparePaths(a, pathLocation))) throw new CodeSyntaxException($"The namespace \"{pathLocation} has already been imported.");

                                        thisNamespace.accessableNamespaces.Add(Global.Namespaces[Global.allLoadedFiles.FindIndex(a => ComparePaths(a, pathLocation))]);
                                        alreadyImportedNamespaces.Add(pathLocation);
                                    }
                                }
                                break;
                            case "makeglobalvar":
                                if ((commandLine.commands.Count != 3 && commandLine.commands.Count != 4) || commandLine.commands[1].commandType != Command.CommandTypes.Statement || commandLine.commands[2].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid usage of makevar. Correct usage:\nmakevar <statement: var type> <statement: var name>;");



                                if (!Enum.TryParse<Value.ValueType>(commandLine.commands[1].commandText, true, out Value.ValueType varType) && commandLine.commands[1].commandText != "all") throw new CodeSyntaxException($"The vartype \"{commandLine.commands[1].commandText}\" doesn't exist.");
                                if (Statement.FindVar(commandLine.commands[2].commandText, new AccessableObjects(thisNamespace.publicNamespaceVars, new(NamespaceInfo.NamespaceIntend.@internal, "")), false) != null) throw new CodeSyntaxException($"A variable with the name \"{commandLine.commands[2].commandText}\" already exists in this context.");
                                Value? setToValue = null;
                                if (commandLine.commands.Count == 4)
                                {
                                    setToValue = Statement.GetValueOfCommandLine(new CommandLine(new() { commandLine.commands[3] }, -1), new AccessableObjects(new(), new(NamespaceInfo.NamespaceIntend.nonedef, "")));
                                }

                                if (commandLine.commands[1].commandText == "all")
                                    thisNamespace.publicNamespaceVars.Add(commandLine.commands[2].commandText, new Var(new(VarConstruct.VarType.all, commandLine.commands[2].commandText), setToValue ?? new(varType)));
                                else
                                    thisNamespace.publicNamespaceVars.Add(commandLine.commands[2].commandText, new Var(new(Value.ConvertValueTypeToVarType(varType), commandLine.commands[2].commandText), setToValue ?? new(varType)));
                                break;

                            default: throw new CodeSyntaxException($"\"{commandLine.commands[0].commandText}\" isn't a recognized statement in header interpret mode.");

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
                        throw new CodeSyntaxException($"You can only use statements in header-mode.");
                }
            }

            if (thisNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.nonedef || thisNamespace.Name == null)
            {
                Tutorial.TutorialPhaseMinusOne();
                throw new CodeSyntaxException("You need to enter name and type for this namespace. You can do that using the name and type statements.");
            }
            if (Global.Namespaces.Any(x => x != thisNamespace && x.Name == thisNamespace.Name)) throw new CodeSyntaxException($"A namespace with the name \"{thisNamespace.Name}\" has already been defined.");




            return new(startCode, thisNamespace);
        }




        private static bool ComparePaths(string path1, string path2)
        {
            return Path.GetFullPath(path1).Replace('\\', '/').ToLower() == Path.GetFullPath(path2).Replace('\\', '/').ToLower();
        }



        public static Value? InterpretNormalMode(List<Command> commands, AccessableObjects accessableObjects)
        {
            foreach (NamespaceInfo namespaceInfo in accessableObjects.currentNamespace.accessableNamespaces)
            {
                foreach (Var var in namespaceInfo.publicNamespaceVars)
                {
                    if (accessableObjects.accessableVars.Contains(var)) continue;
                    if (accessableObjects.accessableVars.ContainsKey(var.varConstruct.name)) throw new CodeSyntaxException($"A global variable with the name \"{var.varConstruct.name}\" already exists. So you can't use this name again.");
                    accessableObjects.accessableVars.Add(var.varConstruct.name, var);
                }
            }
            //More or less the core of the language. It uses a Command-List and loops over every command, it then checks the command type and calls the corrosponding internal functions to the code.
            bool statementMode = false;
            Value? returnValue;
            CommandLine? commandLine = new(new(), -1);
            foreach (Command command in commands)
            {
                accessableObjects.cancellationTokenSource?.Token.ThrowIfCancellationRequested();
                if (statementMode)
                {
                    if (command.commandType == Command.CommandTypes.EndCommand)
                    {
                        if (Global.debugMode)
                        {

                        }

                        returnValue = Statement.StaticStatement(commandLine, accessableObjects);
                        if (returnValue != null)
                            return returnValue;

                        statementMode = false;
                        continue;
                    }
                    commandLine.commands.Add(command);
                    continue;
                }


                switch (command.commandType)
                {
                    case Command.CommandTypes.FunctionCall:
                        Global.currentLine = command.commandLine;
                        if (command.functionCall == null)
                            throw new InternalInterpreterException("Internal: function call was not converted to a function call.");

                        command.functionCall.DoFunctionCall(accessableObjects);
                        break;
                    case Command.CommandTypes.EndCommand:
                        //Just ignore it
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
            if (statementMode) throw new CodeSyntaxException("Seems like you forgot a semicolon (;)");

            return null;
        }
        public static Function? FindFunctionUsingFunctionPath(string functionPath)
        {
            foreach (Function function in Global.AllFunctions)
                if (function.functionLocation == functionPath)
                    return function;
            return null;
        }
    }
}
