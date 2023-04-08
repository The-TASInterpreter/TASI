using System.Runtime.CompilerServices;

namespace TASI
{
    public class NamespaceInfo
    {
        public enum NamespaceIntend
        {
            nonedef, // Not defined intend. Should only occur internaly.
            supervisor, // A special namespace, used for handeling permissions, preimporting Librarys and starting a project.
            generic, // A normal program, with a start, that will have all permissions when started alone.
            @internal, // An internal namspace hard-coded in.
            library // An also normal program, which doesn't have a start and will throw an error if tried to excecute normally.
        }
        private string? name; 
        public List<Function> namespaceFuncitons = new();
        public List<VarDef.EvarType> namespaceVars = new();
        public List<Var> publicNamespaceVars = new();
        public List<NamespaceInfo> accessableNamespaces = new();
        public NamespaceIntend namespaceIntend;

        public string? Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value == null)
                    name = null;
                else
                    name = value.ToLower();
            }
        }


        public NamespaceInfo(NamespaceIntend namespaceIntend, string? name)
        {
            TASI_Main.interpretInitLog.Log($"Creating new Namespace. Intend: {namespaceIntend}; Name: {name}");
            this.namespaceIntend = namespaceIntend;
            Name = name;
            accessableNamespaces.Add(this);
            accessableNamespaces.AddRange(Global.Namespaces.Where(x => x.namespaceIntend == NamespaceIntend.@internal)); //Import all internal namespaces
        }

        public NamespaceInfo(string? name, List<NamespaceInfo> accessableNamespaces, NamespaceIntend namespaceIntend)
        {
            this.name = name;
            this.accessableNamespaces = accessableNamespaces;
            this.namespaceIntend = namespaceIntend;
        }
    }


}
