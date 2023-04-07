using DataTypeStore;

namespace TASI
{
    public class MethodCall
    {
        public Method? callMethod;
        public List<Var> inputVars;
        public List<CommandLine> argumentCommands;
        private string methodName;

        public MethodCall(Region region)
        {
            callMethod = FindMethodByPath(region.FindDirectValue("mL").value, Global.Namespaces, true, null);
            argumentCommands = new List<CommandLine>();
            foreach (Region region1 in region.FindSubregionWithNameArray("CmdL"))
            {
                argumentCommands.Add(new(region));
            }
        }
        public Region Region
        {
            get
            {
                Region result = new("MC", new List<Region>(), new());
                result.directValues.Add(new("mL", callMethod.methodLocation, false));
                foreach (var arg in argumentCommands)
                    result.SubRegions.Add(arg.Region);
                return result;

            }
        }
        public MethodCall(Method callMethod, List<Var> inputVars)
        {
            this.callMethod = callMethod;
            this.inputVars = new List<Var>(inputVars);
        }

        public MethodCall(Command command)
        {
            methodName = "";
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
                argumentCommands.Add(new(StringProcess.ConvertLineToCommand(argument), -1));
            Global.allMethodCalls.Add(this);






            return;
        }

        public void SearchCallMethod(NamespaceInfo currentNamespace) //This is there, so header analysis can be done, without any errors.
        {

            callMethod = FindMethodByPath(methodName.ToLower(), Global.Namespaces, true, currentNamespace);
            foreach (CommandLine commandLine in argumentCommands)
                foreach (Command command in commandLine.commands)
                    if (command.commandType == Command.CommandTypes.MethodCall) command.methodCall.SearchCallMethod(currentNamespace);
        }

        public MethodCallInputHelp? CheckIfMethodCallHasValidArgTypesAndReturnCode(List<Var> inputVars)
        {
            bool matching;
            for (int j = 0; j < callMethod.methodArguments.Count; j++)
            {
                List<VarDef> methodInputType = callMethod.methodArguments[j];
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
                    {
                        if (callMethod.parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.@internal)
                            return new(new(), new());
                        return new(callMethod.methodCode[j], methodInputType);
                    }
                }
            }
            return null;
        }


        public static NamespaceInfo? FindNamespaceByName(string name, List<NamespaceInfo> namespaces, bool exceptionAtNotFound)
        {

            foreach (NamespaceInfo namespaceInfo in namespaces)
                if (namespaceInfo.Name == name)
                    return namespaceInfo;

            if (exceptionAtNotFound)
                throw new Exception($"The namespace \"{name}\" was not found.");
            else
                return null;
        }

        public static Method? FindMethodByPath(string name, List<NamespaceInfo> parentNamespaces, bool exceptionAtNotFound, NamespaceInfo? currentNamespace)
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

            if (currentNamespace != null)
                if (!currentNamespace.accessableNamespaces.Contains(currentMethod.parentNamespace)) throw new Exception($"The method \"{currentMethod.methodLocation}\" can't be accessed in the \"{currentNamespace.Name}\" namespace, because the \"{currentMethod.parentNamespace.Name}\" namespace wasn't imported. ");

            return currentMethod;

        }

        public Var DoMethodCall(AccessableObjects accessableObjects)
        {
            inputVars = new();
            foreach (CommandLine commandLine in argumentCommands) // Exicute arguments
            {
                inputVars.Add(Statement.GetVarOfCommandLine(commandLine, accessableObjects));
            }

            MethodCallInputHelp? methodCallInputHelp = CheckIfMethodCallHasValidArgTypesAndReturnCode(inputVars);

            if (methodCallInputHelp == null)
                throw new Exception($"The method \"{callMethod.methodLocation}\" doesent support the provided input types. Use the syntax \"helpm <method call>;\"\nFor this method it would be: \"helpm [{callMethod.methodLocation}];\"");


            if (callMethod.parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.@internal)
            {
                Var returnValue = InternalMethodHandle.HandleInternalFunc(callMethod.methodLocation, inputVars, accessableObjects);
                if (Global.debugMode)
                {
                    Console.WriteLine($"Did method call to {callMethod.parentNamespace.namespaceIntend}-intend {callMethod.methodLocation}.\nIt returns a {callMethod.returnType}.\nIt returned a {returnValue.varDef.varType}-type with a value of \"{returnValue.ObjectValue}\".");
                    Console.ReadKey();
                }
                return returnValue;

            }
            //return 
            List<Var> methodCallInput = new();

            for (int i = 0; i < inputVars.Count; i++)
            {
                methodCallInput.Add(new(new VarDef(methodCallInputHelp.inputVarType[i].varType, methodCallInputHelp.inputVarType[i].varName), false, this.inputVars[i].ObjectValue));
            }

            Var methodReturnValue = InterpretMain.InterpretNormalMode(methodCallInputHelp.inputCode, new(methodCallInput, callMethod.parentNamespace));



            if (methodReturnValue.varDef.varType != VarDef.EvarType.@return || methodReturnValue.returnStatementValue.varDef.varType != callMethod.returnType)
                throw new Exception($"The method \"{callMethod.methodLocation}\" didn't return the expected {callMethod.returnType}-type.");
            return methodReturnValue.returnStatementValue;

            //throw new Exception("Internal: Only internal functions are implemented");
        }

    }

    public class MethodCallInputHelp
    {
        public List<Command> inputCode;
        public List<VarDef> inputVarType;
        public MethodCallInputHelp(List<Command> inputCode, List<VarDef> inputVarType)
        {
            this.inputCode = inputCode;
            this.inputVarType = inputVarType;
        }
    }
}

