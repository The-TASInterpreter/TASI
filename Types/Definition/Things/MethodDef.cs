using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Types.Definition.Things
{
    public class MethodDef : ThingDef
    {
        public MethodDef(string name, bool isUnimplemented, TypeDef parentType) : base(name, isUnimplemented, parentType)
        {
        }
    }
}
