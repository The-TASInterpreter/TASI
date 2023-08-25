using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Things
{
    public class OverloadDef
    {
        public TypeDef parentType;
        public VisibilityModifier Modifyer { get; }
        public List<TypeDef> inputTypes;



    }
}
