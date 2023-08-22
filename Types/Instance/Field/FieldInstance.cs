using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition;
using TASI.Types.Definition.Field;

namespace TASI.Types.Instance.Field
{
    public class FieldInstance
    {
        public FieldDef fieldDef;
        public TypeInstance? value;

        public FieldInstance(FieldDef fieldDef, TypeInstance? value)
        {
            this.fieldDef = fieldDef;
            this.value = value;
        }
    }
}
