using TASI.InitialisationObjects;
using TASI.InternalLangCoreHandle;
using TASI.LangCoreHandleInterface;
using TASI.PluginManager;
using TASI.RuntimeObjects.FunctionClasses;

namespace TASI
{


    public class GlobalProjectShared
    {
        public bool debugErrorSkip = true;
        public List<NamespaceInfo> namespaces = new List<NamespaceInfo>();
        public List<string> allLoadedFiles = new(); //It is important, that allLoadedFiles and namespaces corrospond
        public List<Function> allFunctions = new List<Function>();
        public bool debugMode = false;
        public List<FunctionCall> allFunctionCalls = new();
        public string mainFilePath;
        public List<Task> processFiles = new();
        public object processFileLock = new();
        public object iportFileLock = new();
        public List<FileStream> allFileStreams = new();
        public Random randomGenerator = new();
        public List<ITASIPlugin> plugins = new();
        public Dictionary<string, Statement> allStatements = new();
        public Dictionary<string, ReturnStatement> allReturnStatements = new();
    }

    public class GlobalContext
    {
        public int currentLine;
        public string currentFile;
    }

    public class Global
    {
        public Dictionary<string, Statement> AllNormalStatements
        {
            get
            {
                return globalProjectShared.allStatements;
            }
            set
            {
                globalProjectShared.allStatements = value;
            }
        }
        public Dictionary<string, ReturnStatement> AllNormalReturnStatements
        {
            get
            {
                return globalProjectShared.allReturnStatements;
            }
            set
            {
                globalProjectShared.allReturnStatements = value;
            }
        }
        public List<ITASIPlugin> Plugins
        {
            get
            {
                return globalProjectShared.plugins;

            }
            set
            {
                globalProjectShared.plugins = value;
            }
        }

        public string CurrentFile
        {
            get
            {
                return globalContext.currentFile;
            }
            set
            {
                globalContext.currentFile = value;
            }
        }

        public bool DebugErrorSkip
        {
            get
            {
                return globalProjectShared.debugErrorSkip;
            }
            set
            {
                globalProjectShared.debugErrorSkip = value;
            }
        }
        public List<NamespaceInfo> Namespaces
        {
            get
            {
                return globalProjectShared.namespaces;
            }
            set
            {
                globalProjectShared.namespaces = value;
            }
        }
        public List<string> AllLoadedFiles
        {
            get
            {
                return globalProjectShared.allLoadedFiles;
            }
            set
            {
                globalProjectShared.allLoadedFiles = value;
            }
        }
        public List<FileStream> AllFileStreams
        {
            get
            {
                return globalProjectShared.allFileStreams;
            }
            set
            {
                globalProjectShared.allFileStreams = value;
            }
        }
        public List<Function> AllFunctions
        {
            get
            {
                return globalProjectShared.allFunctions;
            }
            set
            {
                globalProjectShared.allFunctions = value;
            }
        }
        public Random RandomGenerator
        {
            get
            {
                return globalProjectShared.randomGenerator;
            }
            set
            {
                globalProjectShared.randomGenerator = value;
            }
        }



        public bool DebugMode
        {
            get
            {
                return globalProjectShared.debugMode;
            }
            set
            {
                globalProjectShared.debugMode = value;
            }
        }
        public int CurrentLine
        {
            get
            {
                return globalContext.currentLine;
            }
            set
            {
                globalContext.currentLine = value;
            }
        }
        public List<FunctionCall> AllFunctionCalls
        {
            get
            {
                return globalProjectShared.allFunctionCalls;
            }
            set
            {
                globalProjectShared.allFunctionCalls = value;
            }
        }
        public string MainFilePath
        {
            get
            {
                return globalProjectShared.mainFilePath;
            }
            set
            {
                globalProjectShared.mainFilePath = value;
            }
        }
        public List<Task> ProcessFiles
        {
            get
            {
                return globalProjectShared.processFiles;
            }
            set
            {
                globalProjectShared.processFiles = value;
            }
        }
        public object ProcessFileLock
        {
            get
            {
                return globalProjectShared.processFileLock;
            }
            set
            {
                globalProjectShared.processFileLock = value;
            }
        }
        public object IportFileLock
        {
            get
            {
                return globalProjectShared.iportFileLock;
            }
            set
            {
                globalProjectShared.iportFileLock = value;
            }
        }

        public GlobalContext globalContext;
        public GlobalProjectShared globalProjectShared;


        public Global CreateNewContext(string file)
        {
            GlobalContext globalContext = new()
            {
                currentFile = file,
                currentLine = -1
            };
            return new(globalContext, globalProjectShared);
        }


        public Global(GlobalContext globalContext, GlobalProjectShared globalProjectShared)
        {
            this.globalContext = globalContext;
            this.globalProjectShared = globalProjectShared;
        }
        public Global(string currentFile = "")
        {
            InternalFunctionHandler mainInternalFunctionHandle = new();
            globalContext = new GlobalContext();
            globalContext.currentFile = currentFile;
            globalProjectShared = new GlobalProjectShared();


            Namespaces = new();
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Test", true));
            AllLoadedFiles.Add("*internal");
            new Function("HelloWorld", VarConstruct.VarType.@void, Namespaces[0], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@bool, "display"), new(VarConstruct.VarType.@string, "text")}
            }, new(), this, mainInternalFunctionHandle);

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Console", true));
            AllLoadedFiles.Add("*internal");
            new Function("WriteLine", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@string, "text")},
                new List<VarConstruct> { new(VarConstruct.VarType.num, "num")},
                new List<VarConstruct> { new(VarConstruct.VarType.@bool, "bool")},
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "int")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("Write", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@string, "text")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("ReadLine", VarConstruct.VarType.@string, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@bool, "showTextWhenTyping")},
                new List<VarConstruct> {}
            }, new(), this, mainInternalFunctionHandle);
            new Function("Clear", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> {}
            }, new(), this, mainInternalFunctionHandle);

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Program", true));
            AllLoadedFiles.Add("*internal");
            new Function("Pause", VarConstruct.VarType.@void, Namespaces[2], new List<List<VarConstruct>> {
                new List<VarConstruct> {},
                new List<VarConstruct> {new(VarConstruct.VarType.@bool, "showPausedMessage")}
            }, new(), this, mainInternalFunctionHandle);

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Inf", true));
            AllLoadedFiles.Add("*internal");
            new Function("DefVar", VarConstruct.VarType.@void, Namespaces[3], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "VarType"), new(VarConstruct.VarType.@string, "VarName")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("MakeConst", VarConstruct.VarType.@void, Namespaces[3], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "VarName")}
            }, new(), this, mainInternalFunctionHandle);
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Convert", true));
            AllLoadedFiles.Add("*internal");
            new Function("ToNum", VarConstruct.VarType.num, Namespaces[4], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "ConvertFrom"), new(VarConstruct.VarType.@bool, "errorOnParseFail")}
            }, new(), this, mainInternalFunctionHandle);

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Filesystem", true));
            AllLoadedFiles.Add("*internal");
            new Function("Open", VarConstruct.VarType.@int, Namespaces[5], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "FilePath"), new(VarConstruct.VarType.@string, "Mode") }
            }, new(), this, mainInternalFunctionHandle);
            new Function("Close", VarConstruct.VarType.@void, Namespaces[5], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("Delete", VarConstruct.VarType.@void, Namespaces[5], new List<List<VarConstruct>>
            {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "FilePath")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("Create", VarConstruct.VarType.@int, Namespaces[5], new List<List<VarConstruct>>
            {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "FilePath")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("Exists", VarConstruct.VarType.@bool, Namespaces[5], new List<List<VarConstruct>>
            {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "FilePath")}
            }, new(), this, mainInternalFunctionHandle);
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Filestream", true));
            AllLoadedFiles.Add("*internal");
            new Function("ReadLine", VarConstruct.VarType.@string, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("Read", VarConstruct.VarType.@int, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("Flush", VarConstruct.VarType.@void, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("Write", VarConstruct.VarType.@void, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.@int, "Char")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("WriteLine", VarConstruct.VarType.@void, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.@string, "text")},
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.num, "num")},
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.@bool, "bool")},
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.@int, "int")}
            }, new(), this, mainInternalFunctionHandle);
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Random", true));
            AllLoadedFiles.Add("*internal");
            new Function("Next", VarConstruct.VarType.@int, Namespaces[7], new List<List<VarConstruct>> {
                new List<VarConstruct> {},
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "min")},
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "min"), new(VarConstruct.VarType.@int, "max")}
            }, new(), this, mainInternalFunctionHandle);
            new Function("NextNum", VarConstruct.VarType.num, Namespaces[7], new List<List<VarConstruct>> {
                new List<VarConstruct> {},
            }, new(), this, mainInternalFunctionHandle);
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Shell", false));
            AllLoadedFiles.Add("unsafe.shell");
            new Function("Execute", VarConstruct.VarType.@string, Namespaces[8], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "cmd")},
            }, new(), this, mainInternalFunctionHandle);
            new Function("Run", VarConstruct.VarType.@void, Namespaces[8], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "cmd")},
            }, new(), this, mainInternalFunctionHandle);
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "String", true));
            AllLoadedFiles.Add("*internal");
            new Function("Replace", VarConstruct.VarType.@string, Namespaces[9], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "str"), new(VarConstruct.VarType.@string, "org"), new(VarConstruct.VarType.@string, "new")},
            }, new(), this, mainInternalFunctionHandle);

            //Normal statements
            InternalStatementHandler internalStatementHandler = new();
            AllNormalStatements.Add("loop", new("loop", new()
            {
                new() {}
            }, internalStatementHandler));
            AllNormalStatements.Add("return", new("return", new()
            {
                new() {new(StatementInputType.StatementInput.ValueReturner, "valueToReturn") },
                new()
            }, internalStatementHandler));
            AllNormalStatements.Add("set", new("set", new()
            {
                new() {new(StatementInputType.StatementInput.Statement, "variableName"), new(StatementInputType.StatementInput.ValueReturner, "newVariableValue") },
            }, internalStatementHandler));
            AllNormalStatements.Add("while", new("while", new()
            {
                new() {new(StatementInputType.StatementInput.ValueReturner, "loopWhileTrue"), new(StatementInputType.StatementInput.CodeContainer, "codeToLoop") },
            }, internalStatementHandler));
            AllNormalStatements.Add("if", new("if", new()
            {
                new() {new(StatementInputType.StatementInput.ValueReturner, "ifCheck"), new(StatementInputType.StatementInput.CodeContainer, "doIfTrue")},
                new() {new(StatementInputType.StatementInput.ValueReturner, "ifCheck"), new(StatementInputType.StatementInput.CodeContainer, "doIfTrue"), new("else", "else"), new(StatementInputType.StatementInput.CodeContainer, "doIfFalse") }
            }, internalStatementHandler));
            AllNormalStatements.Add("helpm", new("helpm", new()
            {
                new() {new(StatementInputType.StatementInput.FunctionCall, "functionToCheck (The function call doesn't have to be a valid call)") },
            }, internalStatementHandler));
            AllNormalStatements.Add("listm", new("helpm", new()
            {
                new() {new(StatementInputType.StatementInput.String, "location") },
            }, internalStatementHandler));
            AllNormalStatements.Add("rootm", new("rootm", new()
            {
                new() {},
            }, internalStatementHandler));


            AllNormalStatements.Add("link", new("link", new()
            {
                new() {new(StatementInputType.StatementInput.Statement, "variableToLink"), new(StatementInputType.StatementInput.Statement, "variableToLinkTo") },
            }, internalStatementHandler));
            AllNormalStatements.Add("unlink", new("unlink", new()
            {
                new() {new(StatementInputType.StatementInput.Statement, "variableToReset (After an unlink the variable won't return to its value before the link)") },
            }, internalStatementHandler));
            AllNormalStatements.Add("promise", new("promise", new()
            {
                new() {new(StatementInputType.StatementInput.Statement, "variableName"), new(StatementInputType.StatementInput.CodeContainer, "initPromiseInPromiseContext"), new(StatementInputType.StatementInput.CodeContainer, "promiseCode") },
                new() {new(StatementInputType.StatementInput.Statement, "variableName"), new(StatementInputType.StatementInput.CodeContainer, "promiseCode") },
            }, internalStatementHandler));
            AllNormalStatements.Add("unpromise", new("unpromise", new()
            {
                new() {new(StatementInputType.StatementInput.Statement, "variableName")},
            }, internalStatementHandler));
            AllNormalStatements.Add("makevar", new("makeVar", new()
            {
                new() {new(StatementInputType.StatementInput.Statement, "variableType"), new(StatementInputType.StatementInput.Statement, "variableName")},
                new() {new(StatementInputType.StatementInput.Statement, "variableType"), new(StatementInputType.StatementInput.Statement, "variableName"), new(StatementInputType.StatementInput.ValueReturner, "initialisationValue")},
            }, internalStatementHandler));
            AllNormalStatements.Add("makeconst", new("makeConst", new()
            {
                new() {new(StatementInputType.StatementInput.Statement, "constType"), new(StatementInputType.StatementInput.Statement, "constName"), new(StatementInputType.StatementInput.ValueReturner, "initialisationValue")},
            }, internalStatementHandler));

            //Return statements
            InternalReturnStatementHandler internalReturnStatementHandler = new InternalReturnStatementHandler();
            AllNormalReturnStatements.Add("true", new("true", new()
            {
                new() {},
            }, internalReturnStatementHandler));
            AllNormalReturnStatements.Add("false", new("false", new()
            {
                new() {},
            }, internalReturnStatementHandler));
            AllNormalReturnStatements.Add("void", new("void", new()
            {
                new() {},
            }, internalReturnStatementHandler));
            AllNormalReturnStatements.Add("if", new("if", new()
            {
                new() {new(StatementInputType.StatementInput.ValueReturner, "ifCheck"), new(StatementInputType.StatementInput.CodeContainer, "doIfTrue"), new("else", "else"), new(StatementInputType.StatementInput.CodeContainer, "doIfFalse") }

            }, internalReturnStatementHandler));
            AllNormalReturnStatements.Add("do", new("do", new()
            {
                new() {new(StatementInputType.StatementInput.CodeContainer, "doCode")},
            }, internalReturnStatementHandler));
            AllNormalReturnStatements.Add("linkable", new("linkable", new()
            {
                new() {new(StatementInputType.StatementInput.Statement, "variableName")},
            }, internalReturnStatementHandler));

        }
    }
}
