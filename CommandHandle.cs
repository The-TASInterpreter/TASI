using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_adventure_Script_Interpreter
{
    internal class CommandHandle
    {
        public static void HandleCommand(List<Command> commands)
        {
            if (commands[0].commandType != Command.CommandTypes.Statement)
                throw new Exception($"Invalid command type: {commands[0].commandType}\nIt is unclear weather this is an internal or user error.");

            switch (commands[0].commandText)
            {
                case "new":
                    NewArray(commands);
                    break;
            }
        }

        private static Var[] NewArray(List<Command> commands)
        {
            return null;
        }
    }
}
