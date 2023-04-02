namespace Text_adventure_Script_Interpreter
{
    internal class LetterByLetterAnalysis
    {
        public char letterChar;
        public enum LastLetterType
        {
            @string, method, numCalc, statement
        }

        public LastLetterType lastLetterType;

        public int charInMethodDeph;
        public int letterLine;


        public LetterByLetterAnalysis(char letterChar, LastLetterType commandType, int charInMethodDeph, int letterLine)
        {
            this.letterChar = letterChar;
            this.lastLetterType = commandType;
            this.charInMethodDeph = charInMethodDeph;
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
                        letterTypeStack.Add(LastLetterType.method);
                        break;
                    case ']':
                        if (!letterTypeStack.Any()) throw new Exception("Can't end method, because no method was started");
                        squareDeph += -1;
                        if (letterTypeStack.Last() != LastLetterType.method) throw new Exception($"Can't exit a method using a square bracket while base {letterTypeStack.Last()} is still active.");
                        letterTypeStack.RemoveAt(letterTypeStack.Count - 1);
                        break;
                    case '(':
                        letterTypeStack.Add(LastLetterType.numCalc);
                        break;
                    case ')':
                        if (!letterTypeStack.Any()) throw new Exception("Can't end num calc, because no num calc was started");
                        if (letterTypeStack.Last() != LastLetterType.numCalc) throw new Exception($"Can't exit a num calc using a bracket while base {letterTypeStack.Last()} is still active.");
                        letterTypeStack.RemoveAt(letterTypeStack.Count - 1);
                        break;
                }

                if (letterTypeStack.Any())
                    if (letterTypeStack.Last() == LastLetterType.method)
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
