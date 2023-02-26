 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_adventure_Script_Interpreter
{
    internal class StatementDef
    {
        private List<List<Command.CommandTypes>> commandTypeSyntax; //All allowed syntax for position
        private List<string> forceSytax; //Force a cirten syntax at position
        public StatementDef(List<List<Command.CommandTypes>> commandTypeSyntax, List<string> forceSytax)
        {
            this.commandTypeSyntax = commandTypeSyntax;
            this.forceSytax = forceSytax;
        }
        public bool IsValidSyntax(CommandLine syntax)
        {
            for (int i = 0; i < syntax.commands.Count; i++)
            {
                if (!commandTypeSyntax[i].Contains(syntax.commands[i].commandType))
                {
                    return false;
                } else if ((forceSytax[i] != null) && (syntax.commands[i].commandText != forceSytax[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
