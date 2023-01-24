namespace Text_adventure_Script_Interpreter
{
    public class StringProcess
    {
        public static string GetConcatInside(char inChar, char outChar, string line)
        {
            int deph = 0;
            int i = 0;
            char currentChar;
            string result = string.Empty;
            do
            {
                currentChar = Convert.ToChar(line.Substring(i, 1));


                if (currentChar == outChar)
                    deph--;

                if (deph != 0)
                    result = result + currentChar;

                if (currentChar == inChar)
                    deph++;

                if (deph < 0)
                    throw new Exception("Invalid Concat amount. Try with some more " + inChar);

                if (i > line.Length)
                    throw new Exception("Invalid Concat amount. Try with some more " + outChar);


                i++;


            } while (deph > 0);
            return result;
        }
        internal static readonly char[] specialCommandChars = { '\"', '[', ']', '(', ')', ';', '/'}; //A statement or syntax will end if it contains any of these chars and the correct type will follow
        public static List<Command> ConvertLineToCommand(string line)
        {
            Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Finding syntax of line text:\n{line}");
            List<Command> commands = new List<Command>();
            bool stringMode = false;
            bool methodMode = false;
            bool skipBecauseString = false;
            bool statementMode = false;
            bool syntaxMode = false;
            bool NumCalculationMode = false;
            bool commentMode = false;
            int methodModeDeph = 0;
            bool stringModeBackslash = false;
            string commandText = string.Empty;
            char lastChar = ' ';

            foreach (char c in line)
            {
                if (stringMode)
                {
                    if (c == '\\')
                    {
                        if (stringModeBackslash)
                        {
                            stringModeBackslash = false;
                            commandText = commandText + c;
                            continue;
                        }
                        stringModeBackslash = true;
                        continue;
                    }
                    if (c != '\"' || stringModeBackslash)
                    {
                        stringModeBackslash = false;
                        commandText = commandText + c;
                        continue;
                    }
                    else
                    {
                        Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"String found:\n{commandText}");

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
                            commandText = commandText + c;
                            continue;
                        }
                        else
                        {
                            commandText = commandText + c;
                            lastChar = c;
                            continue;
                        }
                    }
                    if (c == '\"')
                    {
                        commandText = commandText + c;
                        lastChar = c;
                        skipBecauseString = true;
                        continue;
                    }
                    if (c == ']')
                    {
                        methodModeDeph--;

                        if (methodModeDeph == 0)
                        {
                            Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Unknown method found:\n{commandText}");
                            methodMode = false;
                            commands.Add(new Command(Command.CommandTypes.UnknownMethod, commandText));
                            commandText = string.Empty;
                            continue;
                        }
                        commandText = commandText + c;
                    }
                    else if (c == '[')
                    {
                        commandText = commandText + c;
                        methodModeDeph++;
                    }
                    else
                    {
                        commandText = commandText + c;
                    }
                    continue;
                }

                if (NumCalculationMode)
                {
                    if (c == ')')
                    {
                        methodModeDeph--;

                        if (methodModeDeph == 0)
                        {
                            Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Num calc found:\n{commandText}");
                            NumCalculationMode = false;
                            commands.Add(new Command(Command.CommandTypes.NumCalculation, commandText));
                            commandText = string.Empty;
                            continue;
                        }
                        commandText = commandText + c;
                    }
                    else if (c == '(')
                    {
                        commandText = commandText + c;
                        methodModeDeph++;
                    }
                    else
                    {
                        commandText = commandText + c;
                    }
                    continue;
                }

                if (syntaxMode)
                {
                    if (c == ' ' || specialCommandChars.Contains(c))
                    {
                        Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Statement found:\n{commandText}");
                        commands.Add(new Command(Command.CommandTypes.Statement, commandText));
                        syntaxMode = false;
                        commandText = string.Empty;
                        if (specialCommandChars.Contains(c))
                        {
                            goto checkChars;
                        }
                        continue;
                    }
                    commandText = commandText + c;
                    continue;
                }

            checkChars:
                switch (c)
                {
                    case '/':
                        Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Comment found; Skiping line");
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
                    case '{' or '}':
                        Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Brace found \"{c}\"");
                        commands.Add(new Command(Command.CommandTypes.Brace, Convert.ToString(c)));
                        break;
                    case ';':
                        Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"EndCommand found (;)");
                        commands.Add(new Command(Command.CommandTypes.EndCommand, Convert.ToString(';')));
                        break;

                    default:
                        if (c == ' ')
                            continue;
                        else
                        {
                            syntaxMode = true;
                            commandText = commandText + c;
                            continue;
                        }

                }
                if (commentMode) break;





                stringModeBackslash = false;
                lastChar = c;
            }
            if (syntaxMode && commandText != String.Empty) // if syntax mode has not ended yet
            {

                commands.Add(new Command(Command.CommandTypes.Statement, commandText));
                statementMode = false;

            }


            if (stringMode)
                throw new Exception("Invalid string formating. E.U.0001");
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
