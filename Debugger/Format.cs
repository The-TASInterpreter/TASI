using System.Security.Cryptography.X509Certificates;

namespace TASI.debug
{
    internal class Format
    {


        public static void PrintFormatedString(string input)
        {

            bool metadata = true;
            bool color = false;
            foreach (string line in input.Split('\n'))
            {
                metadata = true;
                if (line.Count() != 0 && line[0] == '1')
                    Console.BackgroundColor = ConsoleColor.Red;
                else
                    Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                foreach (char c in line)
                {
                    if (metadata)
                    {
                        if (c == '%')
                            metadata = false;
                        continue;
                    }


                    if (color)
                    {
                        switch (c)
                        {
                            case 'W':
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 'Y':
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            case 'P':
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            case 'O':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 'B':
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case 'G':
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            default:
                                throw new InternalInterpreterException($"Couldn't find color for escape code {c}");
                        }
                        color = false;
                        continue;
                    }

                    if (c == 'Ⅼ')
                    {
                        color = true;
                        continue;
                    }
                    Console.Write(c);
                    
                }
                Console.ResetColor();
                Console.Write('\n');
            }
        }
        public static Tuple<string, int> FormatCalculation(CalculationType calculationType, int highlightCurrentLine = -1, int deph = 0, int currentCommandLine = -1)
        {
            switch (calculationType.type)
            {
                case CalculationType.Type.value:
                    switch (calculationType.value.valueType)
                    {
                        case Value.ValueType.num:
                            return new($"ⅬB{calculationType.value.ObjectValue} ", calculationType.line);
                        case Value.ValueType.@bool:
                            if (calculationType.value.BoolValue)
                                return new($"ⅬBtrue ", calculationType.line);
                            else
                                return new($"ⅬBfalse ", calculationType.line);
                        case Value.ValueType.@string:
                            return new($"ⅬY\"{calculationType.value.ObjectValue}\" ", calculationType.line);
                        default:
                            return new($"ⅬY<unimplemented> ", calculationType.line);




                    }
                case CalculationType.Type.@operator:
                    return new($"ⅬP{calculationType.@operator} ", calculationType.line);
                case CalculationType.Type.function:
                    return FormatFunctionCall(calculationType.functionCall, highlightCurrentLine, deph, currentCommandLine);
                case CalculationType.Type.returnStatement:
                    var returnStatement = FormatCommands(calculationType.returnStatement, highlightCurrentLine, deph, currentCommandLine, true);
                    return new($"ⅬB(${returnStatement.Item1} ) ", returnStatement.Item2);
                case CalculationType.Type.calculation:
                    string result = "ⅬG (";
                    foreach(CalculationType calculationType1 in calculationType.subValues)
                    {
                        result += FormatCalculation(calculationType1, highlightCurrentLine, deph + 1, currentCommandLine).Item1;
                    }
                    result += "ⅬG) ";
                    return new(result, currentCommandLine);


                default: return new($"ⅬY<Unimplemented>", calculationType.line);

            }

        }



        public static Tuple<string,int> FormatFunctionCall(FunctionCall functionCall, int highlightCurrentLine = -1, int deph = 0, int currentCommandLine = -1)
        {
            string result = $"ⅬY[{functionCall.CallFunction.functionLocation}";
            if (functionCall.argumentCommands.Count == 0)
            {
                result += "]";
                return new(result, currentCommandLine);
            }
            result += ":";

            foreach(CommandLine commandLine in functionCall.argumentCommands)
            {
                var formated = FormatCommands(commandLine.commands, highlightCurrentLine, deph, currentCommandLine, true);
                result += formated.Item1;
                currentCommandLine = formated.Item2;
                if (commandLine != functionCall.argumentCommands.Last())
                {
                    result += "ⅬW,";
                }
            }
            result += "ⅬY]";
            return new(result, currentCommandLine);

        }


        public static Tuple<string, int> FormatCommands(List<Command> commands, int highlightCurrentLine = -1, int deph = 0, int currentCommandLine = -1, bool statementMode = false)
        {
            string result = "";
            bool isNewLine = false;
            string tabs = new('\t', deph);
            foreach (Command command in commands)
            {
                Command executingCommand = command;
            start:
                if (executingCommand.commandLine != currentCommandLine)
                {
                    isNewLine = true;

                    if (Math.Abs(executingCommand.commandLine - currentCommandLine) > 1)
                    {
                        currentCommandLine = executingCommand.commandLine;
                        result += $"\n0{currentCommandLine}";
                    }
                    else
                        currentCommandLine = executingCommand.commandLine;
                    if (executingCommand.commandLine == highlightCurrentLine)
                        result += $"\n1{currentCommandLine}%{executingCommand.commandLine + 1}:\t{tabs}";
                    else
                        result += $"\n0{currentCommandLine}%{executingCommand.commandLine + 1}:\t{tabs}";


                }
                else
                {
                    isNewLine = false;
                }

                switch (executingCommand.commandType)
                {
                    case Command.CommandTypes.CodeContainer:
                        if (statementMode && !isNewLine) result += " ";
                        var insideContainer = FormatCommands(executingCommand.codeContainerCommands, highlightCurrentLine, deph + 1, currentCommandLine);
                        result += $"ⅬW{{{insideContainer.Item1}";
                        currentCommandLine = insideContainer.Item2;

                        if (executingCommand.commandEnd > currentCommandLine)
                        {

                            executingCommand = new(Command.CommandTypes.Statement, "ⅬW}", executingCommand.commandEnd);
                            goto start;
                        }
                        else
                        {
                            result += "ⅬW}";
                        }
                        break;
                    case Command.CommandTypes.Calculation:
                        result += FormatCalculation(executingCommand.calculation, highlightCurrentLine, deph + 1, currentCommandLine).Item1;
                        break;

                    case Command.CommandTypes.Statement or Command.CommandTypes.String:

                        switch (executingCommand.commandType)
                        {
                            case Command.CommandTypes.Statement:
                                if (statementMode)
                                    result += "ⅬB";
                                else
                                    result += "ⅬP";
                                break;
                            case Command.CommandTypes.Calculation:
                                result += "ⅬG";
                                break;
                            case Command.CommandTypes.String:
                                result += "ⅬO";
                                break;


                        }

                        if (statementMode && !isNewLine)
                            result += " ";
                        else
                        {
                            statementMode = true;
                        }

                        
                        
                        result += executingCommand.originalCommandText;
                        break;
                    case Command.CommandTypes.FunctionCall:
                        if (statementMode && !isNewLine)
                            result += " ";
                        insideContainer = FormatFunctionCall(executingCommand.functionCall, highlightCurrentLine, deph, currentCommandLine);
                        result += insideContainer.Item1;
                        currentCommandLine = insideContainer.Item2;
                        break;
                    case Command.CommandTypes.EndCommand:
                        statementMode = false;
                        result += "ⅬW; ";
                        break;

                }
            }
            return new(result, currentCommandLine);
        }
    }
}
