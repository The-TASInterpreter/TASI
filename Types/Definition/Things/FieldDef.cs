using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition.Things;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Field
{
    public class FieldDef : ThingDef
    {
        public TypeDef valueType;
        public readonly VisibilityModifier modifyer;

        public FieldDef(VisibilityModifier modifyer, TypeDef valueType, string fieldName, TypeDef parentType) : base(fieldName, false, parentType)
        {
            this.valueType = valueType;
            this.modifyer = modifyer;
        }

    }
}
