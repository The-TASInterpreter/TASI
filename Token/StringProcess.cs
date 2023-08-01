
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
                        if (endChar + 1! < input.Length || input[endChar + 1] != ':') goto default; // to not confuse is smaller than with accessers

                        break;

                    case '(':
                        //Would be nice to do that smoother some day

                        sb.Clear();
                        endChar++;
                        startLine = line;
                        for (int calcDepth = 1; calcDepth != 0; endChar++)
                        {
                            if (endChar >= input.Length) throw new CodeSyntaxException("Expected ']'");

                            switch (input[endChar])
                            {
                                case ')':
                                    calcDepth--;
                                    if (calcDepth != 0)
                                        sb.Append(')');
                                    break;
                                case '(':
                                    calcDepth++;
                                    sb.Append('(');
                                    break;
                                case '\"':
                                    sb.Append($"\"{HandleString(input, endChar, out endChar, out currentLine, global, -1, false).commandText}\"");
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
                        for (int methodDepth = 1; methodDepth != 0; endChar++)
                        {
                            if (endChar >= input.Length) throw new CodeSyntaxException("Expected ']'");
                            switch (input[endChar])
                            {
                                case ']':
                                    methodDepth--;
                                    if (methodDepth != 0)
                                        sb.Append(']');
                                    break;
                                case '[':
                                    methodDepth++;
                                    sb.Append('[');
                                    break;
                                case '\"':

                                    sb.Append($"\"{HandleString(input, endChar, out endChar, out currentLine, global, -1, false).commandText}\"");

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
                throw new CodeSyntaxException("Invalid code container formatting (You probably forgot a '}')");
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
            {'h', '#' },
            {'\\', '\\' }


        };

        private static StringBuilder handleStringSB = new();

        public static List<string> SplitAtBaselevel(string input, char splitChar)
        {
            List<string> result = new List<string>();
            StringBuilder currentItem = new();
            for(int i = 0; i < input.Length; i++)
            {

                switch(input[i])
                {
                    case '\"':
                        throw new NotImplementedException();

                }
            }
            throw new NotImplementedException();

        }
        public static Command HandleObjectAccessorChain(string input, int start, out int endChar, out int endLine, Global global, int startLine = -1)
        {
            endChar = start;
            endLine = startLine;
            if (input[endChar] == '<')
            {
                endChar++;
            }
            int methodDepth = 0;
            List<Accessor> accessors = new();
            StringBuilder sb = new();
            string offset;
            bool baseAccessor = true;
            for (; methodDepth != 0 || input[endChar] == '>'; endChar++)
            {
                if (endChar >= input.Length) throw new CodeSyntaxException("Expected '>'");

                switch (input[endChar])
                {
                    case '.':
                        offset = sb.ToString();
                        if (offset == string.Empty)
                            throw new CodeSyntaxException("Object accessor chain can't have an empty accessor. (Check for double dots: <:base..accessor>");
                        accessors.Add(new(offset));
                        
                        break;
                    case '[':
                        sb.Clear();
                        endChar++;
                        for (methodDepth = 1; methodDepth != 0; endChar++)
                        {
                            if (endChar >= input.Length) throw new CodeSyntaxException("Expected ']'");

                            if (input[endChar] == '[')
                                methodDepth++;
                            if (input[endChar] == ']')
                            {
                                methodDepth--;
                                if (methodDepth == 0)
                                    break;
                            }
                            if (input[endChar] == '\"')
                            {
                                sb.Append($"\"{HandleString(input, endChar, out endChar, out endLine, global, startLine, false).commandText}\"");
                                continue;
                            }
                            
                            sb.Append(input[endChar]);
                        }
                       // accessors.Add(new(new MethodCall()))
                        break;
                        
                    default:
                        if (ignoreChars.Contains(input[endChar]))
                            break;
                        while (input[endChar] != '.')
                        {
                            if (ignoreChars.Contains(input[endChar]))
                            {
                                continue;
                            }
                            sb.Append(input[endChar]);
                        }
                        break;
                }
                
            }
            return null;
        }
        
        
        public static Command HandleString(string input, int start, out int endCharIDX, out int endLine, Global global, int startLine = -1, bool replaceEscape = true)
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
                    if (replaceEscape)
                    {
                        resultString.Append(replace);
                    }
                    else
                    {
                        resultString.Append('\\');

                        resultString.Append(replace);
                    }
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
        

    }
}
