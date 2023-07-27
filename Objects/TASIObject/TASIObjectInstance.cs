using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    internal class TASIObjectInstance
    {
        public static TASIObjectDefinition type;
        public Dictionary<string, Value> simpleValues;
        public Dictionary<string, TASIObjectInstance> pointers;

        public TASIObjectInstance()
        {

        }
        

    }
}
