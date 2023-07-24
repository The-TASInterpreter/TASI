using NUnit.Framework.Constraints;
using System.Text;

namespace TASI
{

    internal class IntHolder
    {
        public int value;
        public IntHolder(int value)
        {
            this.value = value;
        }
        public static IntHolder operator +(IntHolder a, int b)
        {
            return new(a.value + b);
        }
        public static IntHolder operator -(IntHolder a, int b)
        {
            return new(a.value - b);
        }
        public static IntHolder operator ++(IntHolder a)
        {
            return new(a.value + 1);
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    public class StringProcess
    {
        public const char LineChar = 'Ⅼ';
        private static StringBuilder handleLineCharSB = new();
        public static int HandleLineChar(string input, out int endChar, int startChar, Global global)
        {
            endChar = startChar;
            if (input[endChar] == LineChar)
                endChar++;
            StringBuilder nextLine = handleLineCharSB;
            nextLine.Clear();
            while (input[endChar] != LineChar)
            {
                nextLine.Append(input[endChar]);
                endChar++;
            }
            int parsedLine = int.Parse(nextLine.ToString());
            global.CurrentLine = parsedLine;
            return int.Parse(nextLine.ToString());
        }

        public static List<Command> V2(string input, out int endChar, out int line, Global global, int currentLine = 0, int startChar = -1)
        {
            bool canEndAtDataEnd;
            if (startChar == -1)
            {
                canEndAtDataEnd = true;
                startChar = 0;
            }
            else
            {
                canEndAtDataEnd = false;
            }
            List<Command> result = new();
            line = currentLine;
            StringBuilder sb = new();
            int startLine = startChar;
            global.CurrentLine = line;
            for (endChar = startChar; endChar < input.Length; endChar++)
            {
                char c = input[endChar];

                switch (c)
                {
                    case ')':
                    case ']':
                        throw new CodeSyntaxException($"Unexpected '{c}'");


                    case '\"':
                        result.Add(HandleString(input, endChar, out endChar, out line, global, line));
                        break;
                    case ';':
                        result.Add(new(Command.CommandTypes.EndCommand, ";", global, line, line));
                        break;

                    case '<':

                        break;

                    case '(':
                        //Would be nice to do that smoother some day
                        sb.Clear();
                        endChar++;
                        startLine = line;
                        for (int calcDeph = 1; calcDeph != 0; endChar++)
                        {
                            if (input.Length)

                            switch (input[endChar])
                            {
                                case ')':
                                    calcDeph--;
                                    if (calcDeph != 0)
                                        sb.Append(')');
                                    break;
                                case '(':
                                    calcDeph++;
                                    sb.Append('(');
                                    break;
                                case '\"':
                                    sb.Append($"\"{HandleString(input, endChar, out endChar, out currentLine, global).commandText}\"");
                                    break;
                                case LineChar:
                                    line = HandleLineChar(input, out endChar, endChar, global);
                                    sb.Append($"Ⅼ{line}Ⅼ");
                                    break;

                                default:
                                    sb.Append(input[endChar]);
                                    break;
                            }
                        }
                        endChar--;
                        result.Add(new(Command.CommandTypes.Calculation, sb.ToString(),global, startLine, line));
                        break;
                    case '[':
                        //Would be nice to do that smoother some day
                        sb.Clear();
                        endChar++;
                        startLine = line;
                        for (int methodDeph = 1; methodDeph != 0; endChar++)
                        {
                            switch (input[endChar])
                            {
                                case ']':
                                    methodDeph--;
                                    if (methodDeph != 0)
                                        sb.Append(']');
                                    break;
                                case '[':
                                    methodDeph++;
                                    sb.Append('[');
                                    break;
                                case '\"':

                                    sb.Append($"\"{HandleString(input, endChar, out endChar, out currentLine, global).commandText}\"");

                                    break;
                                case LineChar:
                                    line = HandleLineChar(input, out endChar, endChar, global);
                                    sb.Append($"Ⅼ{line}Ⅼ");
                                    break;

                                default:
                                    sb.Append(input[endChar]);
                                    break;
                            }
                        }
                        endChar--;
                        result.Add(new(Command.CommandTypes.FunctionCall, sb.ToString(),global, startLine, line));
                        break;

                    case '{':
                        int lineStart = line;
                        var inCodeContainer = V2(input, out endChar, out line, global, line, endChar + 1);
                        result.Add(new(inCodeContainer, global, lineStart));
                        break;
                    case '}':
                        return result;
                    case LineChar:
                        line = HandleLineChar(input, out endChar, endChar, global);
                        break;
                    default:
                        if (ignoreChars.Contains(c))
                        {
                            break;
                        }
                        sb.Clear();
                        startLine = line;
                        if (specialCommandChars.Contains(input[endChar]))
                        {
                            throw new InternalInterpreterException($"Internal: parsed as statement, but was special command char '{input[endChar]}'");
                        }
                        //Do that cleaner bruh
                        while (!specialCommandChars.Contains(input[endChar]))
                        {


                            if (input[endChar] == LineChar)
                            {
                                line = HandleLineChar(input, out endChar, endChar, global);
                                endChar++;
                                continue;
                            }
                            sb.Append(input[endChar]);
                            endChar++;

                            if (endChar == input.Length)
                                break;
                        }
                        endChar--;
                        result.Add(new(Command.CommandTypes.Statement, sb.ToString(),global, startLine, line));
                        break;
                }
            }
            if (!canEndAtDataEnd)
                throw new CodeSyntaxException("Invalid code container formating (You probably forgot a '}')");
            return result;
        }
        private static readonly HashSet<char> ignoreChars = new()
        {
            ' ', '\t'
        };

        private static readonly Dictionary<char, char> backslashReplace = new Dictionary<char, char>()
        {
            { 'n', '\n' },
            { '\"', '\"' },
            { 't', '\t' },
            { 'l', LineChar },
            {'h', '#' }


        };

        private static StringBuilder handleStringSB = new();

        /*
        public static Command HandleObjectAccessorChain(string input, int start, out int endChar, out int endLine, Global global, int startLine = -1)
        {
            endChar = start;
            if (input[endChar] == '<')
            {
                endChar++;
            }
            int methodDeph = 0;
            List<Accessor> accessors = new();
            StringBuilder sb = new();
            string offset;
            bool workingOnAccessor = true;
            Accessor currentAccessor = null;
            for (; methodDeph != 0 || input[endChar] == '>'; endChar++)
            {
                
                switch (input[endChar])
                {
                    case '.':
                        offset = sb.ToString();
                        
                        if (offset == string.Empty || !workingOnAccessor)
                            throw new CodeSyntaxException("Object accessor chain can't have an empty accessor");
                        workingOnAccessor = false;
                        break;
                    case ' ' or '\t':
                        break;
                    case '[':
                        
                        endChar++;
                        for (methodDeph = 1; methodDeph != 0; endChar++)
                        {
                            if (input[endChar] == '[')
                                methodDeph++;
                            if (input[endChar] == ']')
                            {
                                methodDeph--;
                                if (methodDeph == 0)
                                    break;
                            }
                            if (input[endChar] == '\"')
                            {
                                sb.Append($"\"{HandleString(input, endChar, out endChar, out endLine, global, startLine).commandText}\"");
                                continue;
                            }
                            
                            sb.Append(input[endChar]);
                        }
                        currentAccessor = new();
                        
                }
            }
        }
        */
        public static Command HandleString(string input, int start, out int endCharIDX, out int endLine, Global global, int startLine = -1)
        {


            StringBuilder resultString = handleStringSB;
            resultString.Clear();


            if (input[start] == '\"')
                start++;
            endLine = startLine;
            Command result = new(Command.CommandTypes.String, "",global, startLine, endLine);
            bool lastCharBackslash = false;
            for (endCharIDX = start; endCharIDX < input.Length; endCharIDX++)
            {
                if (lastCharBackslash)
                {
                    lastCharBackslash = false;

                    if (!backslashReplace.TryGetValue(input[endCharIDX], out char replace))
                        throw new CodeSyntaxException($"Invalid string escape char: '{input[endCharIDX]}'");
                    resultString.Append(replace);
                    continue;
                }
                switch (input[endCharIDX])
                {
                    case LineChar:
                        endLine = HandleLineChar(input, out endCharIDX, endCharIDX, global);
                        continue;
                    case '\\':
                        lastCharBackslash = true;
                        continue;
                    case '\"':
                        result.commandEnd = endLine;
                        result.commandText = resultString.ToString();
                        return result;
                }

                resultString.Append(input[endCharIDX]);

            }
            throw new CodeSyntaxException("Didn't end string.");

        }


        internal static readonly HashSet<char> specialCommandChars = new() { '\"', '[', ']', '(', ')', ';', '{', '}', ' ' }; //A sb or syntax will end if it contains any of these chars and the correct type will follow

        public static List<Command> ConvertLineToCommand(string line, Global global, int currentLine = 1)
        {
            return V2(line, out int _, out int _, global);

        }
        /*
        public static List<Command> ConvertLineToCommandOld(string line, int CurrentLine = 1)
        {

            return V2(line, out int _, out int _);
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
            Global.CurrentLine = CurrentLine;
            char lastChar = ' ';

            foreach (char c in line) //Thats some shit code and imma have to fix it some time, but it basically is the main syntax analysis function.
            {
                if (silentLineCharMode)
                {
                    if (c == 'Ⅼ')
                    {
                        silentLineCharMode = false;
                        CurrentLine = Convert.ToInt32(lineCharLine);
                        lineCharLine = "";
                        Global.CurrentLine = CurrentLine;
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
                        CurrentLine = Convert.ToInt32(lineCharLine);
                        lineCharLine = "";
                        Global.CurrentLine = CurrentLine;
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
                            commands.Add(new Command(Command.CommandTypes.CodeContainer, commandText, codeContainerLineStart, CurrentLine));
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

                        commands.Add(new Command(Command.CommandTypes.String, commandText, CurrentLine));
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
                            commands.Add(new Command(Command.CommandTypes.FunctionCall, commandText, CurrentLine));
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
                            commands.Add(new Command(Command.CommandTypes.Calculation, commandText, CurrentLine));
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
                        commands.Add(new Command(Command.CommandTypes.Statement, commandText, CurrentLine));
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
                        codeContainerLineStart = CurrentLine;
                        TASI_Main.interpretInitLog.Log($"CodeContainer found \"{c}\"");
                        codeContainerDeph = 1;
                        codeContainerMode = true;
                        break;
                    case ';':
                        TASI_Main.interpretInitLog.Log($"EndCommand found (;)");
                        commands.Add(new Command(Command.CommandTypes.EndCommand, Convert.ToString(';'), CurrentLine));
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
                commands.Add(new Command(Command.CommandTypes.Statement, commandText, CurrentLine));



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
        */

    }
}
