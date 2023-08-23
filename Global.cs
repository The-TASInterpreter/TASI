using System.Diagnostics;
using TASI.InitialisationObjects;
using TASI.InternalLangCoreHandle;
using TASI.LangCoreHandleInterface;
using TASI.PluginManager;
using TASI.RuntimeObjects;
using TASI.RuntimeObjects.FunctionClasses;
using TASI.RuntimeObjects.VarClasses;
using TASI.Types.Definition;
using TASI.Types.Definition.Field;
using TASI.Types.Instance;

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
        public TypeDef TYPE_DEF_STRING { get; private set; }
        public TypeDef TYPE_DEF_DOUBLE { get; private set; }
        public TypeDef TYPE_DEF_BOOL { get; private set; }
        public TypeDef TYPE_DEF_VOID { get; private set; }
        public TypeDef TYPE_DEF_INT { get; private set; }
        public TypeDef TYPE_DEF_OBJECT { get; private set; }


        public void InitTypes(Global global)
        {
            TYPE_DEF_OBJECT = new("Object", new(NamespaceInfo.NamespaceIntend.@internal, "Object", true, global), new(), false);
            TYPE_DEF_STRING = new("String", new(NamespaceInfo.NamespaceIntend.@internal, "String", true, global), new(), true);
            TYPE_DEF_DOUBLE = new("Double", new(NamespaceInfo.NamespaceIntend.@internal, "Double", true, global), new(), new() { TYPE_DEF_OBJECT }, true, TypeDef.InstantiationType.normal);
            TYPE_DEF_BOOL = new("Bool", new(NamespaceInfo.NamespaceIntend.@internal, "Bool", true, global), new(), new() { TYPE_DEF_OBJECT }, true, TypeDef.InstantiationType.normal);
            TYPE_DEF_VOID = new("Void", new(NamespaceInfo.NamespaceIntend.@internal, "Void", true, global), new(), new() { TYPE_DEF_OBJECT }, true, TypeDef.InstantiationType.normal);
            TYPE_DEF_INT = new("Int", new(NamespaceInfo.NamespaceIntend.@internal, "Int", true, global), new(), new() { TYPE_DEF_OBJECT }, true, TypeDef.InstantiationType.normal);

            TYPE_DEF_OBJECT.fields.Add(new MethodImplementation(TYPE_DEF_OBJECT, "ToString", TYPE_DEF_STRING, new()
            {
                new(new(), (List<Value> input, AccessableObjects obj, TypeInstance self) =>
                {
                    return new()
                } )
            })

        }


    }

    public class GlobalContext
    {
        public int currentLine;
        public string currentFile;
    }

    public class Global
    {
        public TypeDef TYPE_DEF_VOID
        {
            get
            {
                return globalProjectShared.TYPE_DEF_VOID;
            }
        }
        public TypeDef TYPE_DEF_DOUBLE
        {
            get
            {
                return globalProjectShared.TYPE_DEF_DOUBLE;
            }
        }
        public TypeDef TYPE_DEF_STRING
        {
            get
            {
                return globalProjectShared.TYPE_DEF_STRING;
            }
        }
        public TypeDef TYPE_DEF_BOOL
        {
            get
            {
                return globalProjectShared.TYPE_DEF_BOOL;
            }
        }
        public TypeDef TYPE_DEF_INT
        {
            get
            {
                return globalProjectShared.TYPE_DEF_INT;
            }
        }

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
            globalContext = new GlobalContext();
            globalContext.currentFile = currentFile;
            globalProjectShared = new GlobalProjectShared();


            Namespaces = new();



            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Test", true));
            AllLoadedFiles.Add("*internal");
            new Function("HelloWorld", VarConstruct.VarType.@void, Namespaces[0], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(TYPE_DEF_BOOL, "display"), new(TYPE_DEF_STRING, "text")}
            }, new(), this, (input, accessableObjects) =>
            {
                if (input[0].NumValue == 1)
                    Console.WriteLine(input[1].StringValue);
                else
                    Console.WriteLine("No text pritable.");
                return null;
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Console", true));
            AllLoadedFiles.Add("*internal");
            new Function("WriteLine", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@string, "text")},
                new List<VarConstruct> { new(VarConstruct.VarType.num, "num")},
                new List<VarConstruct> { new(VarConstruct.VarType.@bool, "bool")},
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "int")}
            }, new(), this, (input, accessableObjects) =>
            {
                if (input[0].IsNumeric)
                    Console.WriteLine(input[0].NumValue);
                else
                    Console.WriteLine(input[0].StringValue);
                return null;
            });
            new Function("Write", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@string, "text")}
            }, new(), this, (input, accessableObjects) =>
            {
                if (input[0].IsNumeric)
                    Console.Write(input[0].NumValue);
                else
                    Console.Write(input[0].StringValue);
                return null;
            });
            new Function("ReadLine", VarConstruct.VarType.@string, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@bool, "showTextWhenTyping")},
                new List<VarConstruct> {}
            }, new(), this, (input, accessableObjects) =>
            {
                return new(Value.ValueType.@string, Console.ReadLine() ?? throw new RuntimeCodeExecutionFailException("Console.ReadLine returned null", "InternalFuncException"));
            });
            new Function("Clear", VarConstruct.VarType.@void, Namespaces[1], new List<List<VarConstruct>> {
                new List<VarConstruct> {}
            }, new(), this, (input, accessableObjects) =>
            {
                Console.Clear();
                return null;
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Program", true));
            AllLoadedFiles.Add("*internal");
            new Function("Pause", VarConstruct.VarType.@void, Namespaces[2], new List<List<VarConstruct>> {
                new List<VarConstruct> {},
                new List<VarConstruct> {new(VarConstruct.VarType.@bool, "showPausedMessage")}
            }, new(), this, (input, accessableObjects) =>
            {
                if (input.Count == 1 && input[0].NumValue == 1)
                    Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                return null;
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Inf", true));
            AllLoadedFiles.Add("*internal");
            new Function("DefVar", VarConstruct.VarType.@void, Namespaces[3], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "VarType"), new(VarConstruct.VarType.@string, "VarName")}
            }, new(), this, (input, accessableObjects) =>
            {
                if (!Enum.TryParse(input[0].StringValue, true, out Value.ValueType varType) && input[0].StringValue != "all") throw new CodeSyntaxException($"The vartype \"{input[0].StringValue}\" doesn't exist.");
                if (input[0].StringValue == "all")
                {
                    accessableObjects.accessableVars.Add(input[1].StringValue, new Var(new VarConstruct(VarConstruct.VarType.all, input[1].StringValue), new(varType)));
                    return null;
                }
                accessableObjects.accessableVars.Add(input[1].StringValue, new Var(new VarConstruct(Value.ConvertValueTypeToVarType(varType), input[1].StringValue), new(varType)));
                return null;
            });
            new Function("MakeConst", VarConstruct.VarType.@void, Namespaces[3], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "VarName")}
            }, new(), this, (input, accessableObjects) =>
            {
                Var var = (Var)(accessableObjects.accessableVars[input[0].StringValue] ?? throw new CodeSyntaxException($"The variable \"{input[0]}\" cannot be found."));

                var.varType.isConstant = true;
                return null;
            });
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Convert", true));
            AllLoadedFiles.Add("*internal");
            new Function("ToNum", VarConstruct.VarType.num, Namespaces[4], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "ConvertFrom"), new(VarConstruct.VarType.@bool, "errorOnParseFail")}
            }, new(), this, (input, accessableObjects) =>
            {
                if (!double.TryParse(input[0].StringValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double result))
                    if (input[1].BoolValue)
                        throw new CodeSyntaxException("Can't convert string in current format to double.");
                    else
                        return new(Value.ValueType.num, double.NaN);

                return new(Value.ValueType.num, result);
            });

            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Filesystem", true));
            AllLoadedFiles.Add("*internal");
            new Function("Open", VarConstruct.VarType.@int, Namespaces[5], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "FilePath"), new(VarConstruct.VarType.@string, "Mode") }
            }, new(), this, (input, accessableObjects) =>
            {
                FileMode mode = FileMode.Open;
                FileAccess access = FileAccess.ReadWrite;


                if (input[1].StringValue.Contains('w'))
                    access |= FileAccess.Write;

                if (input[1].StringValue.Contains('r'))
                    access |= FileAccess.Read;

                if (input[1].StringValue.Contains('a'))
                    mode = FileMode.Append;

                if (input[1].StringValue.Contains("+!"))
                    mode = FileMode.CreateNew;

                else if (input[1].StringValue.Contains('+'))
                    mode = FileMode.Create;


                if (input[1].StringValue.Contains("~"))
                    mode = FileMode.Truncate;

                if (input[1].StringValue.Contains('?'))
                    mode = FileMode.OpenOrCreate;


                FileStream stream = File.Open(input[0].StringValue, mode, access);
                accessableObjects.global.AllFileStreams.Add(stream);

                int streamIndex = accessableObjects.global.AllFileStreams.IndexOf(stream);

                return new(Value.ValueType.@int, streamIndex);
            });
            new Function("Close", VarConstruct.VarType.@void, Namespaces[5], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex")}
            }, new(), this, (input, accessableObjects) =>
            {

                FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                fileStream.Close();
                accessableObjects.global.AllFileStreams.RemoveAt((int)input[0].NumValue);

                return null;
            });
            new Function("Delete", VarConstruct.VarType.@void, Namespaces[5], new List<List<VarConstruct>>
            {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "FilePath")}
            }, new(), this, (input, accessableObjects) =>
            {
                File.Delete(input[0].StringValue);
                return null;
            });
            new Function("Create", VarConstruct.VarType.@int, Namespaces[5], new List<List<VarConstruct>>
            {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "FilePath")}
            }, new(), this, (input, accessableObjects) =>
            {
                FileStream new_stream = File.Create(input[0].StringValue);
                accessableObjects.global.AllFileStreams.Add(new_stream);

                int new_streamIndex = accessableObjects.global.AllFileStreams.IndexOf(new_stream);

                return new(Value.ValueType.@int, new_streamIndex);
            });
            new Function("Exists", VarConstruct.VarType.@bool, Namespaces[5], new List<List<VarConstruct>>
            {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "FilePath")}
            }, new(), this, (input, accessableObjects) =>
            {
                return new(Value.ValueType.@bool, File.Exists(input[0].StringValue));
            });
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Filestream", true));
            AllLoadedFiles.Add("*internal");
            new Function("ReadLine", VarConstruct.VarType.@string, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex")}
            }, new(), this, (input, accessableObjects) =>
            {
                FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                if (!fileStream.CanRead)

                    throw new RuntimeCodeExecutionFailException("Tried to read from a stream that dosen't allow reading!", "InternalFuncException");

                using StreamReader reader = new(fileStream);
                string line = reader.ReadLine() ?? throw new RuntimeCodeExecutionFailException("Stream.ReadLine returned null", "InternalFuncException");

                return new Value(Value.ValueType.@string, line);
            });
            new Function("Read", VarConstruct.VarType.@int, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex")}
            }, new(), this, (input, accessableObjects) =>
            {
                FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                if (!fileStream.CanRead)
                    throw new RuntimeCodeExecutionFailException("Tried to read from a stream that dosen't allow reading!", "InternalFuncException");

                using StreamReader reader = new(fileStream);
                int character = reader.Read();

                return new Value(Value.ValueType.@int, character);
            });
            new Function("Flush", VarConstruct.VarType.@void, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex")}
            }, new(), this, (input, accessableObjects) =>
            {
                FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                fileStream.Flush();

                return null;
            });
            new Function("Write", VarConstruct.VarType.@void, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.@int, "Char")}
            }, new(), this, (input, accessableObjects) =>
            {
                FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                if (!fileStream.CanWrite)
                    throw new RuntimeCodeExecutionFailException("Tried to read from a stream that doesn't allow writing!", "InternalFuncException");

                using StreamWriter writer = new(fileStream);
                writer.Write((char)(int)input[1].NumValue);

                return null;
            });
            new Function("WriteLine", VarConstruct.VarType.@void, Namespaces[6], new List<List<VarConstruct>> {
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.@string, "text")},
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.num, "num")},
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.@bool, "bool")},
                new List<VarConstruct> { new(VarConstruct.VarType.@int, "StreamIndex"), new(VarConstruct.VarType.@int, "int")}
            }, new(), this, (input, accessableObjects) =>
            {
                FileStream fileStream = accessableObjects.global.AllFileStreams[(int)input[0].NumValue];

                if (!fileStream.CanWrite)
                    throw new RuntimeCodeExecutionFailException("Tried to read from a stream that doesn't allow writing!", "InternalFuncException");

                using StreamWriter writer = new(fileStream);

                if (input[1].IsNumeric)
                    writer.WriteLine(input[1].NumValue);
                else
                    writer.WriteLine(input[1].StringValue);

                return null;
            });
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Random", true));
            AllLoadedFiles.Add("*internal");
            new Function("Next", VarConstruct.VarType.@int, Namespaces[7], new List<List<VarConstruct>> {
                new List<VarConstruct> {},
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "min")},
                new List<VarConstruct> {new(VarConstruct.VarType.@int, "min"), new(VarConstruct.VarType.@int, "max")}
            }, new(), this, (input, accessableObjects) =>
            {
                if (input.Count == 0)
                    return new(Value.ValueType.@int, accessableObjects.global.RandomGenerator.Next());
                else if (input.Count == 1 && input[0].valueType == Value.ValueType.@int)
                    return new(Value.ValueType.@int, accessableObjects.global.RandomGenerator.Next((int)input[0].NumValue));
                else if (input.Count == 2 && input[0].valueType == Value.ValueType.@int && input[1].valueType == Value.ValueType.@int)
                    return new(Value.ValueType.@int, accessableObjects.global.RandomGenerator.Next((int)input[0].NumValue, (int)input[1].NumValue));

                throw new CodeSyntaxException("Invalid usage of the \"Random.Next\" function. Correct usage: Random.Next [<int: min>] [<int: max>];");
            });
            new Function("NextNum", VarConstruct.VarType.num, Namespaces[7], new List<List<VarConstruct>> {
                new List<VarConstruct> {},
            }, new(), this, (input, accessableObjects) =>
            {
                if (input.Count == 0)
                    return new(Value.ValueType.num, accessableObjects.global.RandomGenerator.NextDouble());

                throw new CodeSyntaxException("Invalid usage of the \"Random.Next\" function. It dosn't take any paramters!");
            });
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "Shell", false));
            AllLoadedFiles.Add("unsafe.shell");
            new Function("Execute", VarConstruct.VarType.@string, Namespaces[8], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "cmd")},
            }, new(), this, (input, accessableObjects) =>
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c " + input[0].StringValue;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;


                process.Start();

                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                return new Value(Value.ValueType.@string, output);
            });
            new Function("Run", VarConstruct.VarType.@void, Namespaces[8], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "cmd")},
            }, new(), this, (input, accessableObjects) =>
            {
                Process i_process = new Process();
                i_process.StartInfo.FileName = "cmd.exe";
                i_process.StartInfo.Arguments = "/c " + input[0].StringValue;
                i_process.StartInfo.UseShellExecute = false;
                i_process.StartInfo.RedirectStandardInput = false;
                i_process.StartInfo.RedirectStandardOutput = false;
                i_process.StartInfo.CreateNoWindow = false;

                i_process.Start();

                i_process.WaitForExit();

                return null;
            });
            Namespaces.Add(new NamespaceInfo(NamespaceInfo.NamespaceIntend.@internal, "String", true));
            AllLoadedFiles.Add("*internal");
            new Function("Replace", VarConstruct.VarType.@string, Namespaces[9], new List<List<VarConstruct>> {
                new List<VarConstruct> {new(VarConstruct.VarType.@string, "str"), new(VarConstruct.VarType.@string, "org"), new(VarConstruct.VarType.@string, "new")},
            }, new(), this, (input, accessableObjects) =>
            {
                return new Value(Value.ValueType.@string, input[0].StringValue.Replace(input[1].StringValue, input[2].StringValue));
            });

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
