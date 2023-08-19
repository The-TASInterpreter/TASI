using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Types.Definition.Visibility
{
    public abstract class VisibilityModifier
    {

        public TypeDef parentTypeDef;

        protected VisibilityModifier(TypeDef parentTypeDef)
        {
            this.parentTypeDef = parentTypeDef;
        }

        public abstract bool HasAccess(TypeDef typeDef);
    }
}
