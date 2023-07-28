using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    public class MethodCall : Call
    {
        private Method? callMethod;

        public MethodCall(Command command, Global global) : base(command, global) { }

        public override void SearchCallNameObject(NamespaceInfo currentNamespace, Global global)
        {
            throw new NotImplementedException();
        }

        public Value DoCall(AccessibleObjects accessibleObjects, Value objectValue)
        {
            GetInputValues(accessibleObjects);
            accessibleObjects.accessibleVars.Add("this", new Var(new VarConstruct(objectValue.objectType), objectValue));
            return null;
        }


    }
}
