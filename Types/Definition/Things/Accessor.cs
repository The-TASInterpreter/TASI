using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Things
{
    public class Accessor : Thing
    {

        public VisibilityModifier? getterModifyer;
        public VisibilityModifier? setterModifyer;

        
        public Accessor(string name, VisibilityModifier? getterModifyer, List<Command>? getter, VisibilityModifier? setterModifyer, List<Command>? setter) : base(name)
        {
            this.getterModifyer = getterModifyer;
            this.setterModifyer = setterModifyer;
            this.getter = getter;
            this.setter = setter;
        }

        public List<Command>? getter;
        public List<Command>? setter;

        public override bool isStatic => true;


        public override string actualType => "Accessor";
    }
}
