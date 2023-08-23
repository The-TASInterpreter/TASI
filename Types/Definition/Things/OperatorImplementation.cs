using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Types.Definition.Things
{
    public class OperatorImplementation : Thing
    {
        public OperatorImplementation(string name, bool isUnimplemented, bool allowReservedNames = false) : base(name, isUnimplemented, allowReservedNames)
        {
        }

        public override bool isStatic => true;

        public override ThingType actualType => ThingType.Operator;
    }
}
