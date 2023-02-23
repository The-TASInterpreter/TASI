namespace Text_adventure_Script_Interpreter
{
    public class MethodCall
    {
        public Method callMethod;
        public List<Var> inputVars;
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
            if (inString)
                throw new Exception("Expected \"");
            if (braceDeph != 0)
                throw new Exception("Expected }");
            if (squareDeph != 0)
                throw new Exception("Expected ]");

            if (currentArgument.Replace(" ", "") == "") // If argument minus Space is nothing
                throw new Exception("Cant have an empty argument (Check for double commas like \"[Example.Method:test,,]\")");
            methodArguments.Add(currentArgument);
            List<CommandLine> argumentCommands = new(methodArguments.Count);
            foreach (string argument in methodArguments)
                argumentCommands.Add(new(StringProcess.ConvertLineToCommand(argument), Text_adventure_Script_Interpreter_Main.line));
            inputVars = new();
            foreach (CommandLine commandLine in argumentCommands)
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
            
            return;
        }

    }
}

