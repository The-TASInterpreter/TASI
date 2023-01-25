using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_adventure_Script_Interpreter
{
    internal class FuncHandle
    {
        
         
        public static void HandleInternalFunc (MethodCall methodCall)
        {
            switch(methodCall.callMethod.parentNamespace.name)
            {
                case "INF":
                    InternalFuncs.INF(methodCall);
                    break;

            }
            return;
        }

    }

   internal class InternalFuncs
    {
        public static void INF(MethodCall methodCall)
        {
            switch(methodCall.callMethod.methodLocation)
            {
                case "INF.DefFunc":
                    if (InterpretMain.FindMethodUsingMethodPath(methodCall.inputVars[0].stringValue) == null)
                        throw new Exception($"Can't define func {methodCall.inputVars[0].stringValue}, because it isn't declared anywhere. E.U 0010\nTry to add something like this:\nmethod {methodCall.inputVars[0].stringValue} {{\n/code here\n}}.");
                    if (!Enum.TryParse<VarDef.evarType>(methodCall.inputVars[1].stringValue, out VarDef.evarType result))
                        throw new Exception($"{methodCall.inputVars[1].stringValue} is an invalid variable type. E.U 0011\nValid types are:\nnum\nvoid\nbool\nstring");
                 
                    break;

            }
        }
    }
}
