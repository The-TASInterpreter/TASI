using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Objects.TASIObject
{
    public class Method
    {
        TASIObjectDefinition parentType;
        string methodName;
        NamespaceInfo parentNamespace;
        public VarConstruct.VarType? returnType;
        public List<List<VarConstruct>> methodArguments;
        public List<CommandLine> methodCode;



    }
}
