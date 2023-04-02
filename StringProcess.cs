namespace TASI
{
    public class StringProcess
    {
        internal static readonly char[] specialCommandChars = { '\"', '[', ']', '(', ')', ';', '{', '}' }; //A statement or syntax will end if it contains any of these chars and the correct type will follow
        public static List<Command> ConvertLineToCommand(string line)
        {
            TASI_Main.interpretInitLog.Log($"Finding syntax of line text:\n{line}");
            List<Command> commands = new List<Command>();
            bool stringMode = false;
            bool methodMode = false;
            bool skipBecauseString = false;

            bool syntaxMode = false;
            bool NumCalculationMode = false;
            bool commentMode = false;
            int methodModeDeph = 0;
            int codeContainerDeph = 0;
            bool codeContainerMode = false;
            bool stringModeBackslash = false;
            string commandText = string.Empty;
            char lastChar = ' ';

            foreach (char c in line) //Thats some shit code and imma have to fix it some time, but it basically is the main syntax analysis method.
            {
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
                            commands.Add(new Command(Command.CommandTypes.CodeContainer, commandText));
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

                        commands.Add(new Command(Command.CommandTypes.String, commandText));
                        commandText = string.Empty;
                        stringMode = false;
                        continue;
                    }
                }

                if (methodMode)
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
                        methodModeDeph--;

                        if (methodModeDeph == 0)
                        {
                            TASI_Main.interpretInitLog.Log($"Unknown method found:\n{commandText}");
                            methodMode = false;
                            commands.Add(new Command(Command.CommandTypes.MethodCall, commandText));
                            commandText = string.Empty;
                            continue;
                        }
                        commandText += c;
                    }
                    else if (c == '[')
                    {
                        commandText += c;
                        methodModeDeph++;
                    }
                    else
                    {
                        commandText += c;
                    }
                    continue;
                }

                if (NumCalculationMode)
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
                        methodModeDeph--;

                        if (methodModeDeph == 0)
                        {
                            TASI_Main.interpretInitLog.Log($"Num calc found:\n{commandText}");
                            NumCalculationMode = false;
                            commands.Add(new Command(Command.CommandTypes.NumCalculation, commandText));
                            commandText = string.Empty;
                            continue;
                        }
                        commandText += c;
                    }
                    else if (c == '(')
                    {
                        commandText += c;
                        methodModeDeph++;
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
                        commands.Add(new Command(Command.CommandTypes.Statement, commandText));
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
                    case '^':
                        TASI_Main.interpretInitLog.Log($"Comment found; Skiping line");
                        commentMode = true;
                        break;
                    case '\"':
                        stringMode = true;
                        break;
                    case '[':
                        methodModeDeph = 1;
                        methodMode = true;
                        break;
                    case '(':
                        methodModeDeph = 1;
                        NumCalculationMode = true;
                        break;
                    case '{':
                        TASI_Main.interpretInitLog.Log($"CodeContainer found \"{c}\"");
                        codeContainerDeph = 1;
                        codeContainerMode = true;
                        break;
                    case ';':
                        TASI_Main.interpretInitLog.Log($"EndCommand found (;)");
                        commands.Add(new Command(Command.CommandTypes.EndCommand, Convert.ToString(';')));
                        break;

                    default:
                        if (c == ' ')
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
                commands.Add(new Command(Command.CommandTypes.Statement, commandText));



            if (stringMode)
                throw new Exception("Invalid string formating. E.U.0001");
            if (codeContainerMode)
                throw new Exception("Invalid code container formating.");
            if (methodMode)
                if (skipBecauseString)
                    throw new Exception("Invalid method formating.\nYou probably just forgot to close an argument string. E.U.0002");
                else
                    throw new Exception("Invalid method formating. E.U.0002");
            if (NumCalculationMode)
                throw new Exception("Invalid calculation formating. E.U.0003");
            return commands;
        }

    }
}
