using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition.Field;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Things
{
    public class Constructor
    {
        public readonly List<OverloadImplementation> overloads;

        public Constructor(List<OverloadImplementation> overloads)
        {
            this.overloads = overloads;
        }

        

    }
}
