using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    public class AccessableObjects
    {
        public List<Var> accessableVars = new();
        public NamespaceInfo currentNamespace;
        public bool inLoop = false;
        public AccessableObjects(List<Var> accessableVars, NamespaceInfo importedNamespaces)
        {
            this.accessableVars = accessableVars;
            this.currentNamespace = importedNamespaces;
        }
    }
}
