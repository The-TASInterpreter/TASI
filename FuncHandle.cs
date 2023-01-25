using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_adventure_Script_Interpreter
{
    internal class FuncHandle
    {
        
         
        public static string HandleInternalFunc (List<Command> commands)
        {
            return null;
        }

    }

   internal class InternalFuncs
    {
        public static void TASI(string[] restOfFunction, NamespaceInfo namespaceInfo)
        {
            switch(restOfFunction[1])
            {
                case "Ver":
                    if (restOfFunction[2] != Text_adventure_Script_Interpreter_Main.interpreterVer)
                        Console.WriteLine("!WARNING! The current programm has been written on an outdated interpreter version. Some things might not work as expected.");
                    break;
                case "Intend":
                    namespaceInfo.namespaceIntend = Enum.Parse < NamespaceInfo.NamespaceIntend >(restOfFunction[2]);
                    break;

            }
        }
    }
}
