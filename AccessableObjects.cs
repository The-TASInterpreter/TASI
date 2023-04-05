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
        public List<NamespaceInfo> importedNamespaces = new();

        public AccessableObjects(List<Var> accessableVars, List<NamespaceInfo> importedNamespaces)
        {
            this.accessableVars = accessableVars;
            this.importedNamespaces = importedNamespaces;
        }
    }
}
