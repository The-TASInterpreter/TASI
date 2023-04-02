using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_adventure_Script_Interpreter
{
    internal class FileCode
    {
        public string code;
        public int line;
        public string file;
        public FileCode(string code, int line, string file)
        {
            this.code = code;
            this.line = line;
            this.file = file;
        }
    }
}
