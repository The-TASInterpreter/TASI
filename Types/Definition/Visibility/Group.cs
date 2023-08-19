using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Types.Definition.Visibility
{
    public class Group : Modifyer
    {
        public List<TypeDef> AllowedTypeDefs { get; }

        public Group(TypeDef parentTypeDef, List<TypeDef> allowedTypeDefs) : base(parentTypeDef)
        {
            AllowedTypeDefs = allowedTypeDefs;
        }

        public override bool HasAccess(TypeDef accessingTypeDef)
        {
            if (parentTypeDef == accessingTypeDef) return true;

            foreach (TypeDef allowedTypeDef in AllowedTypeDefs)
            {
                if (allowedTypeDef == accessingTypeDef) return true;
            }
            return false;
        }
    }
}
