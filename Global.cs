namespace Text_adventure_Script_Interpreter
{
    internal class Global
    {
        public static List<NamespaceInfo> Namespaces = new List<NamespaceInfo>();
        public static List<Method> AllMethods = new List<Method>();
        public static NamespaceInfo CurrentNamespace = new(NamespaceInfo.NamespaceIntend.Generic, "Test");
        public static List<Var> CurrentlyAccessableVars = new();
        public static void InitInternalNamespaces()
        {
            /*
            Namespaces = new List<NamespaceInfo>();
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "INF"));
            List<VarDef> varDefs = new List<VarDef> { new VarDef(VarDef.evarType.String, "VerLine") };
            Namespaces[0].namespaceMethods.Add(new Method("Ver", VarDef.evarType.Void, Namespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "VerLine0") })); //INF.Ver String
            Namespaces[0].namespaceMethods.Add(new Method("Intend", VarDef.evarType.Void, Namespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "IntendLine0") })); //INF.Intend String
            Namespaces[0].namespaceMethods.Add(new Method("DefFunc", VarDef.evarType.Void, Namespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "MethodName"), new VarDef(VarDef.evarType.String, "ReturnType"), new VarDef(VarDef.evarType.String, "ArgumentArray", true) })); //INF.DefFunc string, string, string array
            Namespaces[0].namespaceMethods.Add(new Method("Invoke", VarDef.evarType.Void, Namespaces[0], new List<VarDef> { new VarDef(VarDef.evarType.String, "PermitLine0") })); //INF.Invoke String(Permit)
            */
            Namespaces = new();
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Test"));
            new Method("HelloWorld", VarDef.evarType.Void, Namespaces[0], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.evarType.Bool, "display"), new(VarDef.evarType.String, "text")}
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Console"));
            new Method("WriteLine", VarDef.evarType.Void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.evarType.String, "text")},
                new List<VarDef> { new(VarDef.evarType.Num, "num")},
                new List<VarDef> { new(VarDef.evarType.Bool, "bool")}
            });
            new Method("Write", VarDef.evarType.Void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.evarType.String, "text")}
            });
            new Method("ReadLine", VarDef.evarType.String, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.evarType.Bool, "showTextWhenTyping")},
                new List<VarDef> {}
            });
            new Method("Clear", VarDef.evarType.Void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> {}
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Programm"));
            new Method("Pause", VarDef.evarType.Void, Namespaces[2], new List<List<VarDef>> {
                new List<VarDef> {},
                new List<VarDef> {new(VarDef.evarType.Bool, "showPausedMessage")}
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Inf"));
            new Method("DefVar", VarDef.evarType.Void, Namespaces[3], new List<List<VarDef>> {
                new List<VarDef> {new(VarDef.evarType.String, "VarType"), new(VarDef.evarType.String, "VarName")}
            });
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Convert"));
            new Method("ToNum", VarDef.evarType.Num, Namespaces[4], new List<List<VarDef>> {
                new List<VarDef> {new(VarDef.evarType.String, "ConvertFrom"), new(VarDef.evarType.Bool, "errorOnParseFail")}
            });

        }
    }
}
