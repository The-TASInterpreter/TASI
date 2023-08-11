using TASI.LangFuncHandle;

namespace TASI
{
    // [TASI.DecFunc "Main","void",[SArray.DecArray "array cum, string cool"]];
    public class Function
    {
        public string funcName;
        public string functionLocation;
        public bool isSubfunction;
        public Function? parentFunction;
        public VarConstruct.VarType? returnType;
        public List<Function> subFunctions;
        public NamespaceInfo parentNamespace;
        public List<List<VarConstruct>> functionArguments;
        public List<List<Command>> functionCode = new();
        public event EventHandler<Function> functionCreated;
        private IInternalFunctionHandle? functionHandle;

        public IInternalFunctionHandle FunctionHandler
        {
            get
            {
                if (functionHandle == null) 
                {
                    if (parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.@internal)
                        throw new InternalInterpreterException("function handler for internal function was not defined.");
                    else
                        throw new InternalInterpreterException("tried to access function handler for non-internal function.");
                }
                return functionHandle;
            }
        }



        public Function(string funcName, VarConstruct.VarType returnType, NamespaceInfo parentNamespace, List<List<VarConstruct>> functionArguments, List<Command> functionCode, Global global, IInternalFunctionHandle? functionHandle = null) // Is a Main function and is not a void
        {
            this.funcName = funcName.ToLower();
            this.parentFunction = null;
            this.isSubfunction = false;
            this.returnType = returnType;
            this.subFunctions = new List<Function>();
            this.parentNamespace = parentNamespace;
            parentNamespace.namespaceFuncitons.Add(this);
            functionLocation = GetFunctionLocationString();
            this.functionArguments = new List<List<VarConstruct>>(functionArguments);
            global.AllFunctions.Add(this);
            this.functionCode.Add(functionCode);
            this.functionHandle = functionHandle;
            if (parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.@internal && functionHandle == null)
                throw new InternalInterpreterException("function handler for internal function was null.");
        }

        private string GetFunctionLocationString()
        {
            Function checkSubPath = this;
            string? result = null;
            do
            {
                if (result == null)
                    result = checkSubPath.funcName;
                else
                    result = checkSubPath.funcName + "." + result;
                if (!checkSubPath.isSubfunction)
                    return parentNamespace.Name + "." + result;

                checkSubPath = checkSubPath.parentFunction ?? throw new InternalInterpreterException("Internal: Parent function of current function is null for some reason.");
            } while (result.Length < 1024);
            throw new CodeSyntaxException("To large subfunction-function name lenght. I could easily make the max lenght bigger, but I won't. Haha!");
        }


    }



}
