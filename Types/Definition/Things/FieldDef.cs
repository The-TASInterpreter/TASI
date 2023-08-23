using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition.Things;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Field
{
    public class FieldDef : Thing
    {
        public TypeDef valueType;
        public readonly VisibilityModifier modifyer;

        public FieldDef(VisibilityModifier modifyer, TypeDef valueType, string fieldName) : base(fieldName, false)
        {
            this.valueType = valueType;
            this.modifyer = modifyer;
        }

        public override bool isStatic => false;

        public override ThingType actualType => ThingType.Field;
    }
}
