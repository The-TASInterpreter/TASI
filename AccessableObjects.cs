namespace TASI
{
    public class AccessableObjects
    {
        public List<Var> accessableVars = new();
        public NamespaceInfo currentNamespace;
        public List<CustomStatement> customStatements;
        public bool inLoop = false;
        public AccessableObjects(List<Var> accessableVars, NamespaceInfo importedNamespaces, List<CustomStatement> customStatements)
        {
            this.accessableVars = accessableVars;
            this.currentNamespace = importedNamespaces;
            this.customStatements = customStatements;
        }
    }
}
