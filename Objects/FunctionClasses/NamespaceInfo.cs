using System.Collections;

namespace TASI
{
    public class NamespaceInfo
    {
        /// <summary>
        /// This will search this namespace for the input name.
        /// </summary>
        /// <param name="name">includes : and . of the syntax</param>
        /// <returns></returns>
        public TASIObjectDefinition SearchCustomType(string name)
        {
            name = name[1..];
            string[] split = name.Split('.');
            if (split.Length != 2)
            {
                if (split.Length > 2)
                {
                    throw new CodeSyntaxException("A custom type reference can only include Namespace and Type. EG:\n:TypeNamespace.Type");
                }
                else
                {
                    throw new CodeSyntaxException("A custom type reference must include Namespace and Type. EG:\n:TypeNamespace.Type");
                }
            }
            NamespaceInfo foundNamespace = accessableNamespaces.FirstOrDefault(x => x.Name == split[0]) ?? throw new CodeSyntaxException($"The namespace \"{split[0]}\" doesn't exist or wasn't imported");
            if (foundNamespace.objects == null)
            {
                throw new CodeSyntaxException($"The namespace \"{split[0]}\" doesn't support objects");
            }

            return foundNamespace.objects.FirstOrDefault(x => x.objectType == split[1]) ?? throw new CodeSyntaxException($"The namespace \"{split[0]}\" doesn't have a type definition for \"{split[1]}\"");

        }

        public enum NamespaceIntend
        {
            nonedef, // Not defined intend. Should only occur internally.
            supervisor, // A special namespace, used for handling permissions, preimporting Librarys and starting a project.
            generic, // A normal program, with a start, that will have all permissions when started alone.
            @internal, // An internal namespace hard-coded in.
            library, // An also normal program, which doesn't have a start and will throw an error if tried to execute normally.
            @object // A namespace just for defining objects/ custom types
        }
        private string? name;
        public List<Function> namespaceFuncitons = new();
        public List<VarConstruct.VarType> namespaceVars = new();
        public Hashtable publicNamespaceVars = new();
        public List<NamespaceInfo> accessableNamespaces = new();
        public NamespaceIntend namespaceIntend;
        public bool autoImport;
        public List<TASIObjectDefinition>? objects = new();

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
