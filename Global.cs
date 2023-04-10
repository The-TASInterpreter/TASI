namespace TASI
{
    internal class Global
    {
        public static List<NamespaceInfo> Namespaces = new List<NamespaceInfo>();
        public static List<string> allLoadedFiles = new(); //It is important, that allLoadedFiles and Namespaces corrospond
        public static List<Function> AllFunctions = new List<Function>();
        public static List<Var> CurrentlyAccessableVars = new();
        public static bool debugMode = false;
        public static int currentLine;
        public static List<FunctionCall> allFunctionCalls = new();
        public static string mainFilePath;
        
        
        
        public static void InitInternalNamespaces()
        {
            Namespaces = new();
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Test"));
            allLoadedFiles.Add("*internal");
            new Function("HelloWorld", VarDef.EvarType.@void, Namespaces[0], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.@bool, "display"), new(VarDef.EvarType.@string, "text")}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Console"));
            allLoadedFiles.Add("*internal");
            new Function("WriteLine", VarDef.EvarType.@void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.@string, "text")},
                new List<VarDef> { new(VarDef.EvarType.num, "num")},
                new List<VarDef> { new(VarDef.EvarType.@bool, "bool")}
            }, new());
            new Function("Write", VarDef.EvarType.@void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.@string, "text")}
            }, new());
            new Function("ReadLine", VarDef.EvarType.@string, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> { new(VarDef.EvarType.@bool, "showTextWhenTyping")},
                new List<VarDef> {}
            }, new());
            new Function("Clear", VarDef.EvarType.@void, Namespaces[1], new List<List<VarDef>> {
                new List<VarDef> {}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Program"));
            allLoadedFiles.Add("*internal");
            new Function("Pause", VarDef.EvarType.@void, Namespaces[2], new List<List<VarDef>> {
                new List<VarDef> {},
                new List<VarDef> {new(VarDef.EvarType.@bool, "showPausedMessage")}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Inf"));
            allLoadedFiles.Add("*internal");
            new Function("DefVar", VarDef.EvarType.@void, Namespaces[3], new List<List<VarDef>> {
                new List<VarDef> {new(VarDef.EvarType.@string, "VarType"), new(VarDef.EvarType.@string, "VarName")}
            }, new());
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Convert"));
            allLoadedFiles.Add("*internal");
            new Function("ToNum", VarDef.EvarType.num, Namespaces[4], new List<List<VarDef>> {
                new List<VarDef> {new(VarDef.EvarType.@string, "ConvertFrom"), new(VarDef.EvarType.@bool, "errorOnParseFail")}
            }, new());

        }
    }
}
