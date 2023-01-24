namespace Text_adventure_Script_Interpreter
{
    internal class InternalNamespaces
    {
        public List<NamespaceInfo> internalNamespaces;
        public InternalNamespaces()
        {
            internalNamespaces = new List<NamespaceInfo>();
            internalNamespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "INF"));
            List<VarDef> varDefs = new List<VarDef> { new VarDef(VarDef.evarType.String, "VerLine") };
            internalNamespaces[0].namespaceMethods.Add(new Method("Ver", VarDef.evarType.Void, internalNamespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "VerLine0") })); //INF.Ver String
            internalNamespaces[0].namespaceMethods.Add(new Method("Intend", VarDef.evarType.Void, internalNamespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "IntendLine0") })); //INF.Intend String
            internalNamespaces[0].namespaceMethods.Add(new Method("DefFunc", VarDef.evarType.Void, internalNamespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "MethodName"), new VarDef(VarDef.evarType.String, "ReturnType"), new VarDef(VarDef.evarType.String, "ArgumentArray", true) })); //INF.DefFunc string, string, string array
            internalNamespaces[0].namespaceMethods.Add(new Method("Invoke", VarDef.evarType.Void, internalNamespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "PermitLine0") })); //INF.Invoke String(Permit)
        }
    }
}
