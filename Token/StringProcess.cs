namespace TASI
{
    public class StringProcess
    {
        internal static readonly char[] specialCommandChars = { '\"', '[', ']', '(', ')', ';', '{', '}', 'Ⅼ' }; //A statement or syntax will end if it contains any of these chars and the correct type will follow
        public static List<Command> ConvertLineToCommand(string line, int currentLine = 1)
        {
            TASI_Main.interpretInitLog.Log($"Finding syntax of line text:\n{line}");
            List<Command> commands = new List<Command>();
            bool stringMode = false;
            bool functionMode = false;
            bool skipBecauseString = false;
            bool silentLineCharMode = false;

            bool syntaxMode = false;
            bool CalculationMode = false;
            bool commentMode = false;
            int functionModeDeph = 0;
            int codeContainerLineStart = -1;
            int codeContainerDeph = 0;
            bool codeContainerMode = false;
            bool stringModeBackslash = false;
            string commandText = string.Empty;
            bool lineCharMode = false;
            string lineCharLine = "";
            Global.currentLine = currentLine;
            char lastChar = ' ';

            foreach (char c in line) //Thats some shit code and imma have to fix it some time, but it basically is the main syntax analysis function.
            {
                if (silentLineCharMode)
                {
                    if (c == 'Ⅼ')
                    {
                        silentLineCharMode = false;
                        currentLine = Convert.ToInt32(lineCharLine);
                        lineCharLine = "";
                        Global.currentLine = currentLine;
                    }
                    lineCharLine += c;
                }
                else
                {
                    if (c == 'Ⅼ' && !lineCharMode && !silentLineCharMode)
                    {
                        lineCharLine = "";
                        silentLineCharMode = true;
                    }
                }



                if (lineCharMode)
                {
                    if (c == 'Ⅼ')
                    {

                        lineCharMode = false;
                        currentLine = Convert.ToInt32(lineCharLine);
                        lineCharLine = "";
                        Global.currentLine = currentLine;
                        continue;
                    }
                    lineCharLine += c;
                    continue;
                }



                if (codeContainerMode)
                {
                    if (skipBecauseString)
                    {
                        if (c == '\"' && lastChar != '\\')
                        {
                            skipBecauseString = false;
                            commandText += c;
                            continue;
                        }
                        else
                        {
                            commandText += c;
                            lastChar = c;
                            continue;
                        }
                    }
                    if (c == '\"')
                    {
                        commandText += c;
                        lastChar = c;
                        skipBecauseString = true;
                        continue;
                    }
                    if (c == '}')
                    {
                        codeContainerDeph--;

                        if (codeContainerDeph == 0)
                        {
                            TASI_Main.interpretInitLog.Log($"Code container found:\n{commandText}");
                            codeContainerMode = false;
                            commands.Add(new Command(Command.CommandTypes.CodeContainer, commandText, codeContainerLineStart, currentLine));
                            commandText = string.Empty;
                            continue;
                        }
                        commandText += c;
                    }
                    else if (c == '{')
                    {
                        commandText += c;
                        codeContainerDeph++;
                    }
                    else
                    {
                        commandText += c;
                    }
                    continue;
                }

                if (stringMode)
                {

                    if (c == '\\')
                    {
                        if (stringModeBackslash)
                        {
                            stringModeBackslash = false;
                            commandText += c;
                            continue;
                        }
                        stringModeBackslash = true;
                        continue;
                    }
                    if (c != '\"' || stringModeBackslash)
                    {
                        stringModeBackslash = false;
                        commandText += c;
                        continue;
                    }
                    else
                    {
                        TASI_Main.interpretInitLog.Log($"String found:\n{commandText}");

                        commands.Add(new Command(Command.CommandTypes.String, commandText, currentLine));
                        commandText = string.Empty;
                        stringMode = false;
                        continue;
                    }
                }

                if (functionMode)
                {
                    if (skipBecauseString)
                    {
                        if (c == '\"' && lastChar != '\\')
                        {
                            skipBecauseString = false;
                            commandText += c;
                            continue;
                        }
                        else
                        {
                            commandText += c;
                            lastChar = c;
                            continue;
                        }
                    }
                    if (c == '\"')
                    {
                        commandText += c;
                        lastChar = c;
                        skipBecauseString = true;
                        continue;
                    }
                    if (c == ']')
                    {
                        functionModeDeph--;

                        if (functionModeDeph == 0)
                        {
                            TASI_Main.interpretInitLog.Log($"Unknown function found:\n{commandText}");
                            functionMode = false;
                            commands.Add(new Command(Command.CommandTypes.FunctionCall, commandText, currentLine));
                            commandText = string.Empty;
                            continue;
                        }
                        commandText += c;
                    }
                    else if (c == '[')
                    {
                        commandText += c;
                        functionModeDeph++;
                    }
                    else
                    {
                        commandText += c;
                    }
                    continue;
                }

                if (CalculationMode)
                {
                    if (skipBecauseString)
                    {
                        if (c == '\"' && lastChar != '\\')
                        {
                            skipBecauseString = false;
                            commandText += c;
                            continue;
                        }
                        else
                        {
                            commandText += c;
                            lastChar = c;
                            continue;
                        }
                    }
                    if (c == '\"')
                    {
                        commandText += c;
                        lastChar = c;
                        skipBecauseString = true;
                        continue;
                    }
                    if (c == ')')
                    {
                        functionModeDeph--;

                        if (functionModeDeph == 0)
                        {
                            TASI_Main.interpretInitLog.Log($"Num calc found:\n{commandText}");
                            CalculationMode = false;
                            commands.Add(new Command(Command.CommandTypes.Calculation, commandText, currentLine));
                            commandText = string.Empty;
                            continue;
                        }
                        commandText += c;
                    }
                    else if (c == '(')
                    {
                        commandText += c;
                        functionModeDeph++;
                    }
                    else
                    {
                        commandText += c;
                    }
                    continue;
                }

                if (syntaxMode)
                {
                    if (c == ' ' || specialCommandChars.Contains(c))
                    {
                        TASI_Main.interpretInitLog.Log($"Statement found:\n{commandText}");
                        commands.Add(new Command(Command.CommandTypes.Statement, commandText, currentLine));
                        syntaxMode = false;
                        commandText = string.Empty;
                        if (specialCommandChars.Contains(c))
                        {
                            goto checkChars;
                        }
                        continue;
                    }
                    commandText += c;
                    continue;
                }

            checkChars:
                switch (c)
                {
                    case 'Ⅼ':
                        lineCharMode = true;
                        silentLineCharMode = false;
                        continue;
                    case '^':
                        TASI_Main.interpretInitLog.Log($"Comment found; Skiping line");
                        commentMode = true;
                        break;
                    case '\"':
                        stringMode = true;
                        break;
                    case '[':
                        functionModeDeph = 1;
                        functionMode = true;
                        break;
                    case '(':
                        functionModeDeph = 1;
                        CalculationMode = true;
                        break;
                    case '{':
                        codeContainerLineStart = currentLine;
                        TASI_Main.interpretInitLog.Log($"CodeContainer found \"{c}\"");
                        codeContainerDeph = 1;
                        codeContainerMode = true;
                        break;
                    case ';':
                        TASI_Main.interpretInitLog.Log($"EndCommand found (;)");
                        commands.Add(new Command(Command.CommandTypes.EndCommand, Convert.ToString(';'), currentLine));
                        break;

                    default:
                        if (c == ' ' || c == '\t')
                            continue;
                        else
                        {
                            syntaxMode = true;
                            commandText += c;
                            continue;
                        }

                }
                if (commentMode) break;





                stringModeBackslash = false;
                lastChar = c;
            }
            if (syntaxMode && commandText != String.Empty) // if syntax mode has not ended yet
                commands.Add(new Command(Command.CommandTypes.Statement, commandText, currentLine));



            if (stringMode)
                throw new CodeSyntaxException("Invalid string formating. E.U.0001");
            if (codeContainerMode)
                throw new CodeSyntaxException("Invalid code container formating.");
            if (functionMode)
                if (skipBecauseString)
                    throw new CodeSyntaxException("Invalid function formating.\nYou probably just forgot to close an argument string. E.U.0002");
                else
                    throw new CodeSyntaxException("Invalid function formating. E.U.0002");
            if (CalculationMode)
                throw new CodeSyntaxException("Invalid calculation formating. E.U.0003");
            return commands;
        }

    }
}
