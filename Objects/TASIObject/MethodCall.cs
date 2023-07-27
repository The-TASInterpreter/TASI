using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    public class MethodCall : FunctionCall
    {
        private Method? callMethod;
        public List<Value> inputValues;
        public List<CommandLine> argumentCommands;

        public MethodCall(Command command, Global global) : base(command, global)
        {
            
        }

    }
}
