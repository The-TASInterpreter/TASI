namespace Text_adventure_Script_Interpreter
{
    internal class CheckMethodName
    {
        private static readonly string[] forbidenNames = { "soccer" };
        private static readonly char[] allowedChars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T','U','V','W','X','Y','Z','_' };

        private static bool IsValidMethodName(string name)
        {
            foreach (string forbidenName in forbidenNames)
                if (name.Contains(forbidenName)) return false;
            foreach (char c in name) 
                if (!allowedChars.Contains(c)) return false;
            return true;
        }
    }
}
