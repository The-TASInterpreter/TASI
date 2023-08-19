using System.Collections;
using TASI.InterpretStartup;
using TASI.RuntimeObjects.VarClasses;
using TASI.Types.Definition;

namespace TASI.RuntimeObjects.FunctionClasses
{
    public class NamespaceInfo
    {
        public enum NamespaceIntend
        {
            nonedef, // Not defined intend. Should only occur internaly.
            generic, // A normal program, with a start, that will have all permissions when started alone.
            @internal, // An internal namspace hard-coded in.
            library, // An also normal program, which doesn't have a start and will throw an error if tried to excecute normally.
            typedef
        }
        private string? name;
        public readonly List<Function>? namespaceFuncitons;
        public List<NamespaceInfo> accessableNamespaces;
        public NamespaceIntend namespaceIntend;
        public bool autoImport;
        public readonly List<TypeDef>? namespaceTypes;

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

        public NamespaceInfo(List<TypeDef> types, string? name, bool autoImport = false, Global? global = null)
        {
            namespaceIntend = NamespaceIntend.typedef;
            Name = name;
            accessableNamespaces.Add(this);
            if (global != null)
                accessableNamespaces.AddRange(global.Namespaces.Where(x => x.autoImport)); //Import all internal namespaces that have auto import activated
            this.autoImport = autoImport;
            this.namespaceTypes = types;
            
        }


        public NamespaceInfo(NamespaceIntend namespaceIntend, string? name, bool autoImport = false, Global? global = null)
        {
            TASI_Main.interpretInitLog.Log($"Creating new Namespace. Intend: {namespaceIntend}; Name: {name}");
            this.namespaceIntend = namespaceIntend;
            Name = name;
            accessableNamespaces.Add(this);
            if (global != null)
                accessableNamespaces.AddRange(global.Namespaces.Where(x => x.autoImport)); //Import all internal namespaces that have auto import activated
            this.autoImport = autoImport;
        }

        public NamespaceInfo(string? name, List<NamespaceInfo> accessableNamespaces, NamespaceIntend namespaceIntend)
        {
            this.name = name;
            this.accessableNamespaces = accessableNamespaces;
            this.namespaceIntend = namespaceIntend;
        }
    }


}
