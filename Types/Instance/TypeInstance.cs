using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition;
using TASI.Types.Instance.Field;

namespace TASI.Types.Instance
{
    public class TypeInstance
    {
        public readonly TypeDef typeDef;
        public List<FieldInstance> fieldInstances = new List<FieldInstance>();
        public Dictionary<string, object> typeThings = new();

        public static List<TypeDef> ConverToTypeDef(List<TypeInstance> instances)
        {
            List<TypeDef> ret = new List<TypeDef>();
            foreach (var instance in instances)
            {
                ret.Add(instance.typeDef);
            }
            return ret;
        }
    }
}
