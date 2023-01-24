using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_adventure_Script_Interpreter
{
    internal class GetKeys
    {
        public string PressedKey;
        public void GetPressedKeys()
        {
            char currentKey;
            while (true)
            {
                currentKey = Convert.ToChar(Console.ReadKey(true));
                this.PressedKey = PressedKey + Convert.ToString(currentKey);
            }
        }
    }
}
