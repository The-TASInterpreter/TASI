namespace Text_adventure_Script_Interpreter
{
    internal class Global
    {
        public static List<NamespaceInfo> Namespaces = new List<NamespaceInfo>();
        public static List<Method> AllMethods = new List<Method>();
        public static void InitInternalNamespaces()
        {
            Namespaces = new List<NamespaceInfo>();
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "INF"));
            List<VarDef> varDefs = new List<VarDef> { new VarDef(VarDef.evarType.String, "VerLine") };
            Namespaces[0].namespaceMethods.Add(new Method("Ver", VarDef.evarType.Void, Namespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "VerLine0") })); //INF.Ver String
            Namespaces[0].namespaceMethods.Add(new Method("Intend", VarDef.evarType.Void, Namespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "IntendLine0") })); //INF.Intend String
            Namespaces[0].namespaceMethods.Add(new Method("DefFunc", VarDef.evarType.Void, Namespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "MethodName"), new VarDef(VarDef.evarType.String, "ReturnType"), new VarDef(VarDef.evarType.String, "ArgumentArray", true) })); //INF.DefFunc string, string, string array
            Namespaces[0].namespaceMethods.Add(new Method("Invoke", VarDef.evarType.Void, Namespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "PermitLine0") })); //INF.Invoke String(Permit)
        }
    }
}
