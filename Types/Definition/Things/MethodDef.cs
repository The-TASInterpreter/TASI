using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Types.Definition.Things
{
    public class MethodDef : ThingDef
    {
        List<OverloadDef> overloads;
        public MethodDef(string name, bool isUnimplemented, TypeDef parentType, List<OverloadDef> overloads) : base(name, isUnimplemented, parentType)
        {
            this.overloads = overloads;
        }
    }
}
