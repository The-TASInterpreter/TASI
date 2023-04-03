namespace DataTypeStore
{
    public class DirectValueClearify
    {
        private static readonly char[] invalidChars = { '%', ';', ':' };
        public static string EncodeInvalidChars(string text)
        {
            string result = string.Empty;
            foreach (char c in text)
            {

                if (invalidChars.Contains(c))
                    result += $"%{Array.IndexOf(invalidChars, c)}%";
                else
                    result += c;
            }
            return result;
        }
        public static string DecodeInvalidCharCode(string text)
        {
            bool inPercent = false;
            string currentInt = string.Empty;
            string result = string.Empty;
            foreach (char c in text)
            {
                if (inPercent) //special chars are inside a percent code like this %0% and the number needs to be extracted.
                {
                    if (c == '%') //Number extracted; return to normal
                    {
                        inPercent = false;
                        result += invalidChars[Convert.ToInt32(currentInt)];
                        currentInt = string.Empty;
                        continue;

                    }
                    currentInt += c;
                }
                else
                {
                    if (c == '%') //Start number extraction
                    {
                        inPercent = true;
                        continue;
                    }
                    result += c;
                }

            }
            return result;
        }
    }

}