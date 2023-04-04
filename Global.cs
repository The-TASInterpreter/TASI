namespace TASI
{
    internal class Global
    {
        public static List<NamespaceInfo> Namespaces = new List<NamespaceInfo>();
        public static List<Method> AllMethods = new List<Method>();
        public static NamespaceInfo CurrentNamespace = new(NamespaceInfo.NamespaceIntend.Generic, "Test");
        public static List<Var> CurrentlyAccessableVars = new();
        public static bool debugMode = false;
        public static int currentLine;
        
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
            new Method("HelloWorld", VarDef.EvarType.Void, Namespaces[0], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.Bool, "display"), new(VarDef.EvarType.String, "text")}
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Console"));
            new Method("WriteLine", VarDef.EvarType.Void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.String, "text")},
                new List<VarDef> { new(VarDef.EvarType.Num, "num")},
                new List<VarDef> { new(VarDef.EvarType.Bool, "bool")}
            });
            new Method("Write", VarDef.EvarType.Void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.String, "text")}
            });
            new Method("ReadLine", VarDef.EvarType.String, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.Bool, "showTextWhenTyping")},
                new List<VarDef> {}
            });
            new Method("Clear", VarDef.EvarType.Void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> {}
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Program"));
            new Method("Pause", VarDef.EvarType.Void, Namespaces[2], new List<List<VarDef>> {
                new List<VarDef> {},
                new List<VarDef> {new(VarDef.EvarType.Bool, "showPausedMessage")}
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Inf"));
            new Method("DefVar", VarDef.EvarType.Void, Namespaces[3], new List<List<VarDef>> {
                new List<VarDef> {new(VarDef.EvarType.String, "VarType"), new(VarDef.EvarType.String, "VarName")}
            });
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.Internal, "Convert"));
            new Method("ToNum", VarDef.EvarType.Num, Namespaces[4], new List<List<VarDef>> {
                new List<VarDef> {new(VarDef.EvarType.String, "ConvertFrom"), new(VarDef.EvarType.Bool, "errorOnParseFail")}
            });

        }
    }
}
