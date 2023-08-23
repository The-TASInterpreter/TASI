using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Types.Definition.Things
{
    public abstract class ThingDef
    {
        public string name;
        public bool isUnimplemented;
        public TypeDef parentType;
        public ThingDef(string name, bool isUnimplemented, TypeDef parentType)
        {
            this.name = name;
            this.isUnimplemented = isUnimplemented;
            this.parentType = parentType;
        }
    }
}
