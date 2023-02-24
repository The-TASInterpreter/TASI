namespace Text_adventure_Script_Interpreter
{
    public class MethodCall
    {
        public Method callMethod;
        public List<Var> inputVars;
        public List<CommandLine> argumentCommands;
        public MethodCall(Method callMethod, List<Var> inputVars)
        {
            this.callMethod = callMethod;
            this.inputVars = new List<Var>(inputVars);
        }

        public MethodCall(Command command)
        {
            string methodName = "";
            List<string> methodArguments = new();
            string currentArgument = "";
            bool doingName = true;
            int squareDeph = 0;
            int braceDeph = 0;
            bool inString = false;
            bool lastBackslash = false;

            //split method name from arguments and arguments with comma
            foreach (char c in command.commandText)
            {
                if (doingName)
                {
                    if (c == ':')
                    {
                        if (methodName == "")
                            throw new Exception("Method call can't have an empty method path.");
                        doingName = false;
                        continue;
                    }
                    methodName += c;
                }
                else
                {
                    if (inString)
                    {
                        if (!lastBackslash && c == '\"')
                            inString = false;
                        currentArgument += c;
                        continue;
                    }


                    switch (c)
                    {
                        case '\"':
                            inString = true;
                            lastBackslash = false;
                            break;

                        case '[':
                            squareDeph += 1;
                            break;
                        case ']':
                            squareDeph -= 1;
                            if (squareDeph < 0)
                                throw new Exception("Unexpected ']'");
                            break;
                        case '{':
                            braceDeph += 1;
                            break;
                        case '}':
                            braceDeph -= 1;
                            if (braceDeph < 0)
                                throw new Exception("Unexpected '}'");
                            break;
                        case ',':
                            if (braceDeph == 0 && squareDeph == 0 && !inString) // Only check for base layer commas
                            {
                                if (currentArgument.Replace(" ", "") == "") // If argument minus Space is nothing
                                    throw new Exception("Cant have an empty argument (Check for double commas like \"[Example.Method:test,,]\")");
                                methodArguments.Add(currentArgument);
                                currentArgument = "";
                                continue;
                            }
                            break;
                    }
                    currentArgument += c;

                }
            }
            //Check if syntax are valid
            if (inString)
                throw new Exception("Expected \"");
            if (braceDeph != 0)
                throw new Exception("Expected }");
            if (squareDeph != 0)
                throw new Exception("Expected ]");

            if (currentArgument.Replace(" ", "") == "") // If argument minus Space is nothing
                throw new Exception("Cant have an empty argument (Check for double commas like \"[Example.Method:test,,]\")");
            methodArguments.Add(currentArgument);
            argumentCommands = new(methodArguments.Count);
            foreach (string argument in methodArguments) //Convert string arguments to commands
                argumentCommands.Add(new(StringProcess.ConvertLineToCommand(argument), Text_adventure_Script_Interpreter_Main.line));

            // search for method
            string[] methodLocationSplit = methodName.Split('.');

            if (methodLocationSplit.Length < 2)
            {
                if (methodLocationSplit.Length == 0) throw new Exception("The methodname cant be empty.");
                if (methodLocationSplit.Length == 1) throw new Exception("A namespace cant be used as a method directly.");
            }

            NamespaceInfo methodParentNamespace = new(NamespaceInfo.NamespaceIntend.Internal, "Invalid");

            foreach (NamespaceInfo namespaceInfo in Global.Namespaces)
            {
                if (namespaceInfo.name == methodLocationSplit[0])
                {
                    methodParentNamespace = namespaceInfo;
                    break;
                }
            }
            if (methodParentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.Internal && methodParentNamespace.name == "Invalid")
                throw new Exception($"The namespace \"{methodLocationSplit[0]}\" cant be found."); // Hella good code

            Method directNamespaceParentMethod = new("", VarDef.evarType.Void, new(NamespaceInfo.NamespaceIntend.Internal, "Invalid"), new());
            foreach (Method method in methodParentNamespace.namespaceMethods)
            {
                if (method.funcName == methodLocationSplit[1])
                {
                    directNamespaceParentMethod = method;
                    break;
                }
            }

            if (directNamespaceParentMethod.parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.Internal && directNamespaceParentMethod.parentNamespace.name == "Invalid")
                throw new Exception($"The method \"{methodLocationSplit[1]}\" cant be found."); // Hella good code
            List<Method> submethods = new List<Method>();
            submethods.Add(directNamespaceParentMethod);
            for (int i = 2; i < methodLocationSplit.Length; i++)
            {
                foreach (Method method in submethods.Last().subMethods)
                {
                    if (method.funcName == methodLocationSplit[i])
                    {
                        submethods.Add(method);
                        continue;
                    }

                }
                throw new Exception($"The submethod \"{methodLocationSplit[i]}\" does not exist");
            }
            this.callMethod = submethods.Last();




            return;
        }

        public bool CheckIfMethodCallHasValidArgTypes(List<Var> inputVars)
        {
            bool matching;
            foreach (List<VarDef> methodInputType in callMethod.methodArguments)
            {
                if (methodInputType.Count == inputVars.Count) {
                    matching = true;
                    for (int i = 0; i < inputVars.Count; i++)
                    {
                        if (methodInputType[i].varType != inputVars[i].varDef.varType)
                        {
                            matching = false;
                            break;
                        }
                    }
                    if (matching)
                        return true;
                }
            }
            return false;
        }



        public Var DoMethodCall()
        {
            inputVars = new();
            foreach (CommandLine commandLine in argumentCommands) // Exicute arguments
            {
                switch (commandLine.commands[0].commandType)
                {
                    case Command.CommandTypes.StringMethod or Command.CommandTypes.String:
                        if (commandLine.commands.Count != 1)
                            throw new Exception($"Not expected {commandLine.commands[1].commandType} after string (\"{commandLine.commands[1].commandText}\")");
                        inputVars.Add(new(new(VarDef.evarType.String, ""), true, commandLine.commands[0].commandText));
                        break;
                    case Command.CommandTypes.Statement:
                        inputVars.Add(Statement.ReturnStatement(commandLine.commands));
                        break;

                    default:


                        throw new Exception($"Internal error: Unimplemented commandType ({commandLine.commands[0].commandType})");
                }
            }

            if (!CheckIfMethodCallHasValidArgTypes(inputVars))
                throw new Exception($"The method \"{callMethod.methodLocation}\" doesent support the provided input types.");


            if (callMethod.parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.Internal)
            {
                return FuncHandle.HandleInternalFunc(callMethod.methodLocation, inputVars);
            }
            throw new Exception("Internal: Only internal functions are implemented");
        }

    }
}

