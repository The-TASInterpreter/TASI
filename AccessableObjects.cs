using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    public class AccessableObjects
    {
        public Hashtable accessableVars = new();   //List<Var> accessableVars = new();
        public NamespaceInfo currentNamespace;
        public bool inLoop = false;
        public CancellationTokenSource? cancellationTokenSource;
        public Global global;
        public List<TASIObjectDefinition> accessableObjectDefinitions = new();
        public AccessableObjects(Hashtable accessableVars, NamespaceInfo importedNamespaces, Global global)
        {
            this.accessableVars = accessableVars;
            this.currentNamespace = importedNamespaces;
            this.global = global;
        }
    }
}
