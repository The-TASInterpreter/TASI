using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Objects.TASIObject
{
    public class MethodCall
    {
        private Method? callMethod;
        public List<Value> inputValues;
        public List<CommandLine> argumentCommands;
    }
}
