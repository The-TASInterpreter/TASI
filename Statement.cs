using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_adventure_Script_Interpreter
{
    internal class Statement
    {
        public static Var ReturnStatement(List<Command> commands)
        {
            if (commands[0].commandType != Command.CommandTypes.Statement)           
                throw new Exception("Internal: ReturnStatements must start with a Statement");

            switch (commands[0].commandText)
            {
                case "true":
                    return new Var(new(VarDef.evarType.Bool, ""), true, true);
                case "false":
                    return new Var(new(VarDef.evarType.Bool, ""), true, false);
                case "new":
                    if (commands[1].commandType != Command.CommandTypes.Statement)
                        throw new Exception($"Unexpected {commands[1].commandType} at argument 1 of new statement");
                    throw new NotImplementedException("Internal: New statement is not fully implemented yet");
                    

                default:
                    // Is probably var
                    foreach (Var var in Global.CurrentlyAccessableVars)
                        if (var.varDef.varName == commands[0].commandText)
                            return var;



                    throw new Exception($"Unknown return statement \"{commands[0].commandText}\"");
            }

        }

    }
}
