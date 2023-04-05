using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    internal class AccessableObjects
    {
        List<Var> accessableVars = new();
        List<NamespaceInfo> importedNamespaces = new();

        public AccessableObjects(List<Var> accessableVars, List<NamespaceInfo> importedNamespaces)
        {
            this.accessableVars = accessableVars;
            this.importedNamespaces = importedNamespaces;
        }
    }
}
