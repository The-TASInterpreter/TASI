using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Things
{
    public class AccessorImplementation : Thing
    {


        
        public AccessorImplementation(AccessorDef accessorDef, List<Command>? getter,  List<Command>? setter) : base(accessorDef)
        {
            if (accessorDef.access == AccessorDef.Access.get && getter == null)
                throw new CodeSyntaxException("")


            this.getter = getter;
            this.setter = setter;
        }

        public List<Command>? getter;
        public List<Command>? setter;

        public override bool isStatic => true;


        public override ThingType actualType => ThingType.Accessor;
    }
}
