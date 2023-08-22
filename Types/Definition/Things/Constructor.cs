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
        public readonly List<Overload> overloads;

        public Constructor(List<Overload> overloads)
        {
            this.overloads = overloads;
        }

        

    }
}
