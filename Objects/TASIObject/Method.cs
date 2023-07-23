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
        NamespaceInfo parentNamespace;
        string methodName;
        
        public VarConstruct.VarType returnType;
        public List<List<VarConstruct>> methodArguments;
        public List<List<Command>> methodCode;

        public Method(TASIObjectDefinition parentType, NamespaceInfo parentNamespace, string methodName, VarConstruct.VarType returnType, List<List<VarConstruct>> methodArguments, List<List<Command>> methodCode)
        {
            this.parentType = parentType ?? throw new ArgumentNullException(nameof(parentType));
            this.parentNamespace = parentNamespace ?? throw new ArgumentNullException(nameof(parentNamespace));
            this.methodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            this.returnType = returnType;
            this.methodArguments = methodArguments ?? throw new ArgumentNullException(nameof(methodArguments));
            this.methodCode = methodCode ?? throw new ArgumentNullException(nameof(methodCode));
        }
    }
}
