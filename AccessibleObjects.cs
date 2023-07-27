using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    public class AccessibleObjects
    {
        public Hashtable accessibleVars = new();   //List<Var> accessibleVars = new();
        public NamespaceInfo currentNamespace;
        public bool inLoop = false;
        public CancellationTokenSource? cancellationTokenSource;
        public Global global;
        public List<TASIObjectDefinition> accessibleObjectDefinitions = new();
        public AccessibleObjects(Hashtable accessibleVars, NamespaceInfo importedNamespaces, Global global)
        {
            this.accessibleVars = accessibleVars;
            this.currentNamespace = importedNamespaces;
            this.global = global;
        }
    }
}
