using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.RuntimeObjects.FunctionClasses;
using TASI.RuntimeObjects.VarClasses;

namespace TASI
{
    public class AccessableObjects
    {
        public Dictionary<string, Var> accessableVars = new();   //List<Var> accessableVars = new();
        public NamespaceInfo currentNamespace;
        public bool inLoop = false;
        public CancellationTokenSource? cancellationTokenSource;
        public Global global;
        public AccessableObjects(Dictionary<string, Var> accessableVars, NamespaceInfo currentNamespace, Global global)
        {
            this.accessableVars = accessableVars;
            this.currentNamespace = currentNamespace;
            this.global = global;
        }
    }
}
