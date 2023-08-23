using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition.Field;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Things
{
    public abstract class Thing
    { 
        public enum ThingType
        {
            Field,
            Method,
            Accessor,
            Operator
        }
        public abstract bool isStatic { get; }
        public abstract ThingType actualType { get; }
        public string Name {
            get
            {
                return thingDef.name;
            }
        }


        public static readonly string[] reservedNames = new string[]
        {
            
        };
        ThingDef thingDef;

        public Thing(ThingDef thingDef)
        {
            this.thingDef = thingDef;
            if (reservedNames.Contains(this.Name) ) 
            {
                throw new CodeSyntaxException($"The Name {this.Name} is reserved for internal purposes");
            }
        }
    }
}
