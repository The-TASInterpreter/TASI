

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
        public List<CustomStatement> customStatements = new();


        public Function(string funcName, Function parentFunction, VarConstruct.VarType returnType, NamespaceInfo parentNamespace, List<List<VarConstruct>> functionArguments, List<Command> functionCode) // Has a variable return type and is not a void, but is a sub function
        {
            this.funcName = funcName;
            this.parentFunction = parentFunction;
            this.isSubfunction = true;
            this.returnType = returnType;
            this.subFunctions = new List<Function>();
            this.parentNamespace = parentNamespace;
            parentNamespace.namespaceFuncitons.Add(this);
            functionLocation = GetFunctionLocationString();
            this.functionArguments = new List<List<VarConstruct>>(functionArguments);
            Global.AllFunctions.Add(this);
            this.functionCode.Add(functionCode);

        }

        public Function(string funcName, VarConstruct.VarType returnType, NamespaceInfo parentNamespace, List<List<VarConstruct>> functionArguments, List<Command> functionCode) // Is a Main function and is not a void
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
            Global.AllFunctions.Add(this);
            this.functionCode.Add(functionCode);

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
