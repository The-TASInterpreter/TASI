namespace TASI
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
                        if (c == '\\')
                            lastBackslash = true;
                        else
                            lastBackslash = false;
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

            if (currentArgument.Replace(" ", "") == "" && methodArguments.Count != 0) // If argument minus Space is nothing
                throw new Exception("Cant have an empty argument (Check for double commas like \"[Example.Method:test,,]\")");
            if (methodArguments.Count != 0 || (currentArgument.Replace(" ", "") != ""))
                methodArguments.Add(currentArgument);
            argumentCommands = new(methodArguments.Count);
            foreach (string argument in methodArguments) //Convert string arguments to commands
                argumentCommands.Add(new(StringProcess.ConvertLineToCommand(argument), TASI_Main.line));


            this.callMethod = FindMethodByPath(methodName, Global.Namespaces, true); 




            return;
        }

        public bool CheckIfMethodCallHasValidArgTypes(List<Var> inputVars)
        {
            bool matching;
            foreach (List<VarDef> methodInputType in callMethod.methodArguments)
            {
                if (methodInputType.Count == inputVars.Count)
                {
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


        public static NamespaceInfo? FindNamespaceByName(string name, List<NamespaceInfo> namespaces, bool exceptionAtNotFound)
        {

            foreach (NamespaceInfo namespaceInfo in namespaces)
                if (namespaceInfo.name == name)
                    return namespaceInfo;

            if (exceptionAtNotFound)
                  throw new Exception($"The namespace \"{name}\" was not found.");
            else
                return null;
        }

        public static Method? FindMethodByPath(string name, List<NamespaceInfo> parentNamespaces, bool exceptionAtNotFound)
        {
            string[] nameSplit = name.Split('.');
            if (nameSplit.Length < 2)
                throw new Exception($"Can't find method \"{name}\" because there is no method in the location.");
            NamespaceInfo? parentNamespace = FindNamespaceByName(nameSplit[0], parentNamespaces, exceptionAtNotFound);
            if (parentNamespace == null)
                return null;
            List<Method> methods = parentNamespace.namespaceMethods;
            Method? currentMethod = null;
            for (int i = 0; i < nameSplit.Length - 1; i++)
            {
                currentMethod = null;
                foreach (Method method in methods)
                {
                    if (method.funcName == nameSplit[i + 1])
                    {
                        methods = method.subMethods;
                        currentMethod = method;
                        break;
                    }
                }
                if (currentMethod == null)
                {
                    if (exceptionAtNotFound)
                        throw new Exception($"Could not find method \"{name}\".");
                    else
                        return null;
                }

            }
            return currentMethod;

        }

        public Var DoMethodCall()
        {
            inputVars = new();
            foreach (CommandLine commandLine in argumentCommands) // Exicute arguments
            {
                /*
                switch (commandLine.commands[0].lastLetterType)
                {
                    case Command.LastLetterType.String:
                        if (commandLine.commands.Count != 1)
                            throw new Exception($"Not expected {commandLine.commands[1].lastLetterType} after string (\"{commandLine.commands[1].commandText}\")");
                        inputVars.Add(new(new(VarDef.evarType.String, ""), true, commandLine.commands[0].commandText));
                        break;

                    case Command.LastLetterType.UnknownMethod:
                        MethodCall tempMethodCall = new(commandLine.commands[0]);
                        inputVars.Add(tempMethodCall.DoMethodCall());
                        break;

                    case Command.LastLetterType.Statement:
                        inputVars.Add(Statement.ReturnStatement(commandLine.commands));
                        break;
                    case Command.LastLetterType.NumCalculation:
                        if (commandLine.commands.Count != 1)
                            throw new Exception($"Not expected {commandLine.commands[1].lastLetterType} after num calc (\"{commandLine.commands[1].commandText}\")");
                        inputVars.Add(NumCalculation.DoNumCalculation(commandLine.commands[0]));
                        break;

                    default:
                        throw new Exception($"Internal error: Unimplemented lastLetterType ({commandLine.commands[0].lastLetterType})");

                }
                */
                inputVars.Add(Statement.GetVarOfCommandLine(commandLine));
            }

            if (!CheckIfMethodCallHasValidArgTypes(inputVars))
                throw new Exception($"The method \"{callMethod.methodLocation}\" doesent support the provided input types. Use the syntax \"helpm <method call>;\"\nFor this method it would be: \"helpm [{callMethod.methodLocation}];\"");


            if (callMethod.parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.Internal)
            {
                Var returnValue = FuncHandle.HandleInternalFunc(callMethod.methodLocation, inputVars);
                if (Global.debugMode)
                {
                    Console.WriteLine($"Did method call to {callMethod.parentNamespace.namespaceIntend}-intend {callMethod.methodLocation}.\nIt returns a {callMethod.returnType}.\nIt returned a {returnValue.varDef.varType}-type with a value of \"{returnValue.ObjectValue}\".");
                    Console.ReadKey();
                }
                return returnValue;
                
            }
            throw new Exception("Internal: Only internal functions are implemented");
        }

    }
}

