using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    public abstract class Call
    {

        public abstract void SearchCallNameObject(NamespaceInfo currentNamespace, Global global);

        public List<CommandLine> argumentCommands;
        public string callName;
        public List<Value> argumentValues;

        public abstract Value DoCall(AccessibleObjects accessibleObjects);


        public void GetInputValues(AccessibleObjects accessibleObjects)
        {
            argumentValues = new();
            foreach (CommandLine commandLine in argumentCommands) // Exicute arguments
            {
                argumentValues.Add(Statement.GetValueOfCommandLine(commandLine, accessibleObjects));
                
            }
        }
        public Call(List<Value> argumentValues)
        {
            this.argumentValues = argumentValues;
        }

        public Call(Command command, Global global)
        {
            callName = "";
            List<string> functionArguments = new();
            bool doingName = true;
            int squareDeph = 0;
            int braceDeph = 0;
            bool inString = false;
            string currentArgumentString;
            StringBuilder currentArgument = new();

            //split function name from arguments and arguments with comma
            for (int i = 0; i < command.commandText.Length; i++)
            {
                char c = command.commandText[i];
                if (doingName)
                {
                    if (c == ':')
                    {
                        if (callName == "")
                            throw new CodeSyntaxException("Function call can't have an empty function path.");
                        doingName = false;
                        continue;
                    }
                    callName += c;
                }
                else
                {



                    switch (c)
                    {
                        case '\"':
                            currentArgument.Append('\"');
                            currentArgument.Append(StringProcess.HandleString(command.commandText, i, out i, out _, global, -1, false).commandText);
                            currentArgument.Append('\"');

                            continue;

                        case '[':
                            squareDeph += 1;
                            break;
                        case ']':
                            squareDeph -= 1;
                            if (squareDeph < 0)
                                throw new CodeSyntaxException("Unexpected ']'");
                            break;
                        case '{':
                            braceDeph += 1;
                            break;
                        case '}':
                            braceDeph -= 1;
                            if (braceDeph < 0)
                                throw new CodeSyntaxException("Unexpected '}'");
                            break;
                        case ',':
                            if (braceDeph == 0 && squareDeph == 0 && !inString) // Only check for base layer commas
                            {
                                currentArgumentString = currentArgument.ToString();
                                if (currentArgumentString.Replace(" ", "") == "") // If argument minus Space is nothing
                                    throw new CodeSyntaxException("Cant have an empty argument (Check for double commas like \"[Example.Function:test,,]\")");
                                functionArguments.Add(currentArgumentString);
                                currentArgument.Clear();
                                continue;
                            }
                            break;
                    }
                    currentArgument.Append(c);

                }
            }
            //Check if syntax are valid
            currentArgumentString = currentArgument.ToString();

            if (inString)
                throw new CodeSyntaxException("Expected \"");
            if (braceDeph != 0)
                throw new CodeSyntaxException("Expected }");
            if (squareDeph != 0)
                throw new CodeSyntaxException("Expected ]");

            if (currentArgumentString.Replace(" ", "") == "" && functionArguments.Count != 0) // If argument minus Space is nothing
                throw new CodeSyntaxException("Cant have an empty argument (Check for double commas like \"[Example.Function:test,,]\")");
            if (functionArguments.Count != 0 || (currentArgumentString.Replace(" ", "") != ""))
                functionArguments.Add(currentArgumentString);
            argumentCommands = new(functionArguments.Count);
            foreach (string argument in functionArguments) //Convert string arguments to commands
                argumentCommands.Add(new(StringProcess.ConvertLineToCommand(argument, global, command.commandLine), -1));







            return;
        }
    }
}
