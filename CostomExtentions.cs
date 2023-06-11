using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename_TALaT_CS
{
    public static class CostomExtentions
    {
        public static string RemoveWorking(this string str, char removeChar)
        {
            string result = "";
            foreach (char c in str)
            {
                if (c != removeChar)
                    result += c;
            }
            return result;
        }
    }
}
