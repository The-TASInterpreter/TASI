using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.Types.Definition;
using TASI.Types.Definition.Visibility;
using TASI.Types.Instance.Field;

namespace TASI.Types.Instance
{
    public class TypeInstance
    {
        public readonly TypeDef typeDef;
        
        public List<FieldInstance> fieldInstances = new List<FieldInstance>();
        public Dictionary<string, object> typeThings = new();
        private object simpleTypeValue;

       

        public static List<TypeDef> ConverToTypeDef(List<TypeInstance> instances)
        {
            List<TypeDef> ret = new List<TypeDef>();
            foreach (var instance in instances)
            {
                ret.Add(instance.typeDef);
            }
            return ret;
        }

        public static string GetDescriberOfInput(List<TypeInstance> instances)
        {
            StringBuilder sb = new StringBuilder("(");
            for (int i = 0; i < instances.Count; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append(instances[i].typeDef.GetFullName);
            }
            return sb.ToString();
        }
    }
}
