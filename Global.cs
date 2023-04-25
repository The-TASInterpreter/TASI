namespace TASI
{
    internal class Global
    {
        public static List<NamespaceInfo> Namespaces = new List<NamespaceInfo>();
        public static List<string> allLoadedFiles = new(); //It is important, that allLoadedFiles and Namespaces corrospond
        public static List<Function> AllFunctions = new List<Function>();

        public static List<Var> globalVars = new List<Var>();

        public static bool debugMode = false;
        public static int currentLine;
        public static List<FunctionCall> allFunctionCalls = new();
        public static string mainFilePath;



        public static void InitInternalNamespaces()
        {
            Namespaces = new();
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Test"));
            allLoadedFiles.Add("*internal");
            new Function("HelloWorld", VarConstruct.VarType.@void, Namespaces[0], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@bool, "display"), new(VarConstruct.VarType.@string, "text")}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Console"));
            allLoadedFiles.Add("*internal");
            new Function("WriteLine", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@string, "text")},
                new List<VarConstruct> { new(VarConstruct.VarType.num, "num")},
                new List<VarConstruct> { new(VarConstruct.VarType.@bool, "bool")}
            }, new());
            new Function("Write", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@string, "text")}
            }, new());
            new Function("ReadLine", VarConstruct.VarType.@string, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@bool, "showTextWhenTyping")},
                new List<VarConstruct> {}
            }, new());
            new Function("Clear", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> {}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Program"));
            allLoadedFiles.Add("*internal");
            new Function("Pause", VarConstruct.VarType.@void, Namespaces[2], new List<List<VarConstruct>> {
                new List<VarConstruct> {},
                new List<VarConstruct> {new(VarConstruct.VarType.@bool, "showPausedMessage")}
            }, new());
            new Function("Exit", VarConstruct.VarType.@void, Namespaces[2], new List<List<VarConstruct>> {
                new List<VarConstruct> {}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Inf"));
            allLoadedFiles.Add("*internal");
            new Function("DefVar", VarConstruct.VarType.@void, Namespaces[3], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "VarType"), new(VarConstruct.VarType.@string, "VarName")}
            }, new());
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Convert"));
            allLoadedFiles.Add("*internal");
            new Function("ToNum", VarConstruct.VarType.num, Namespaces[4], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "ConvertFrom"), new(VarConstruct.VarType.@bool, "errorOnParseFail")}
            }, new());


            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Story"));
            allLoadedFiles.Add("*internal");
            new Function("AskQuestion", VarConstruct.VarType.@string, Namespaces[5], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "Prompt")}
            }, new());

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "String"));
            allLoadedFiles.Add("*internal");
            new Function("ToLower", VarConstruct.VarType.@string, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "Input")}
            }, new());
            new Function("GetLetter", VarConstruct.VarType.@string, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "InputString"), new(VarConstruct.VarType.num, "Number") }
            }, new());

        }
    }
}
