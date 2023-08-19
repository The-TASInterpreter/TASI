using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Types.Definition.Visibility
{
    public class Public : VisibilityModifier
    {
        public Public(TypeDef parentTypeDef) : base(parentTypeDef) { }
        public override bool HasAccess(TypeDef typeDef)
        {
            return true;
        }
    }
}
