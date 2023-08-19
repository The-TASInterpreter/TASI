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
    public class Constructor : Thing
    {
        public readonly List<Overload> overloads;

        public Constructor(List<Overload> overloads) : base(CONSTRUCTOR, true)
        {
            this.overloads = overloads;
        }

        public override bool isStatic => true;

        public override string actualType => "Constructor";
    }
}
