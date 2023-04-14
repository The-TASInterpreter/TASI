namespace TASI
{
    internal class LetterByLetterAnalysis
    {
        public char letterChar;
        public enum LastLetterType
        {
            @string, function, numCalc, statement
        }

        public LastLetterType lastLetterType;

        public int charInFunctionDeph;
        public int letterLine;


        public LetterByLetterAnalysis(char letterChar, LastLetterType commandType, int charInFunctionDeph, int letterLine)
        {
            this.letterChar = letterChar;
            this.lastLetterType = commandType;
            this.charInFunctionDeph = charInFunctionDeph;
            this.letterLine = letterLine;
        }

        public static List<LetterByLetterAnalysis> AnalyseString(string text, int line)
        {
            List<LetterByLetterAnalysis> result = new();
            List<LastLetterType> letterTypeStack = new();
            int squareDeph = 0;
            bool lastBackslash = false;
            foreach (char c in text)
            {
                if (letterTypeStack.Any() && letterTypeStack.Last() == LastLetterType.@string)
                {

                    if (!lastBackslash && c == '\"')
                        letterTypeStack.RemoveAt(letterTypeStack.Count - 1);
                    if (c == '\\')
                        lastBackslash = true;
                    else
                        lastBackslash = false;
                    result.Add(new(c, LastLetterType.@string, -1, line));
                    continue;
                }


                switch (c)
                {
                    case '\"':
                        letterTypeStack.Add(LastLetterType.@string);
                        lastBackslash = false;
                        break;

                    case '[':
                        squareDeph += 1;
                        letterTypeStack.Add(LastLetterType.function);
                        break;
                    case ']':
                        if (!letterTypeStack.Any()) throw new CodeSyntaxException("Can't end function, because no function was started");
                        squareDeph += -1;
                        if (letterTypeStack.Last() != LastLetterType.function) throw new CodeSyntaxException($"Can't exit a function using a square bracket while base {letterTypeStack.Last()} is still active.");
                        letterTypeStack.RemoveAt(letterTypeStack.Count - 1);
                        break;
                    case '(':
                        letterTypeStack.Add(LastLetterType.numCalc);
                        break;
                    case ')':
                        if (!letterTypeStack.Any()) throw new CodeSyntaxException("Can't end num calc, because no num calc was started");
                        if (letterTypeStack.Last() != LastLetterType.numCalc) throw new CodeSyntaxException($"Can't exit a num calc using a bracket while base {letterTypeStack.Last()} is still active.");
                        letterTypeStack.RemoveAt(letterTypeStack.Count - 1);
                        break;
                }

                if (letterTypeStack.Any())
                    if (letterTypeStack.Last() == LastLetterType.function)
                        result.Add(new(c, letterTypeStack.Last(), squareDeph, line));
                    else
                        result.Add(new(c, letterTypeStack.Last(), -1, line));
                else
                    result.Add(new(c, LastLetterType.statement, squareDeph, line));

            }

            return result;
        }
    }
}
