namespace TASI
{
    internal class Global
    {
        public static List<NamespaceInfo> Namespaces = new List<NamespaceInfo>();
        public static List<string> allLoadedFiles = new(); //It is important, that allLoadedFiles and Namespaces corrospond
        public static List<Method> AllMethods = new List<Method>();
        public static List<Var> CurrentlyAccessableVars = new();
        public static bool debugMode = false;
        public static int currentLine;
        public static List<MethodCall> allMethodCalls = new();

        
        
        
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
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Test"));
            allLoadedFiles.Add("*internal");
            new Method("HelloWorld", VarDef.EvarType.@void, Namespaces[0], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.@bool, "display"), new(VarDef.EvarType.@string, "text")}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Console"));
            allLoadedFiles.Add("*internal");
            new Method("WriteLine", VarDef.EvarType.@void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.@string, "text")},
                new List<VarDef> { new(VarDef.EvarType.num, "num")},
                new List<VarDef> { new(VarDef.EvarType.@bool, "bool")}
            }, new());
            new Method("Write", VarDef.EvarType.@void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.@string, "text")}
            }, new());
            new Method("ReadLine", VarDef.EvarType.@string, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.@bool, "showTextWhenTyping")},
                new List<VarDef> {}
            }, new());
            new Method("Clear", VarDef.EvarType.@void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> {}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Program"));
            allLoadedFiles.Add("*internal");
            new Method("Pause", VarDef.EvarType.@void, Namespaces[2], new List<List<VarDef>> {
                new List<VarDef> {},
                new List<VarDef> {new(VarDef.EvarType.@bool, "showPausedMessage")}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Inf"));
            allLoadedFiles.Add("*internal");
            new Method("DefVar", VarDef.EvarType.@void, Namespaces[3], new List<List<VarDef>> {
                new List<VarDef> {new(VarDef.EvarType.@string, "VarType"), new(VarDef.EvarType.@string, "VarName")}
            }, new());
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Convert"));
            allLoadedFiles.Add("*internal");
            new Method("ToNum", VarDef.EvarType.num, Namespaces[4], new List<List<VarDef>> {
                new List<VarDef> {new(VarDef.EvarType.@string, "ConvertFrom"), new(VarDef.EvarType.@bool, "errorOnParseFail")}
            }, new());

        }
    }
}
