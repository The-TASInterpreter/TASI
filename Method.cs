namespace TASI
{
    // [TASI.DecFunc "Main","void",[SArray.DecArray "array cum, string cool"]];
    public class Method
    {
        public string funcName;
        public string methodLocation;
        public bool isSubmethod;
        public Method? parentMethod;
        public bool isVoid;
        public VarDef.EvarType? returnType;
        public List<VarDef.EvarType> methodPrivateVars;
        public List<Method> subMethods;
        public NamespaceInfo parentNamespace;
        public List<List<VarDef>> methodArguments;
        public List<List<Command>> methodCode = new();


        public Method(string funcName, Method parentMethod, VarDef.EvarType returnType, NamespaceInfo parentNamespace, List<List<VarDef>> methodArguments, List<Command> methodCode) // Has a variable return type and is not a void, but is a sub method
        {
            this.funcName = funcName;
            this.parentMethod = parentMethod;
            this.isSubmethod = true;
            this.isVoid = false;
            this.returnType = returnType;
            this.subMethods = new List<Method>();
            this.methodPrivateVars = new List<VarDef.EvarType>();
            this.parentNamespace = parentNamespace;
            parentNamespace.namespaceMethods.Add(this);
            methodLocation = GetMethodLocationString();
            this.methodArguments = new List<List<VarDef>>(methodArguments);
            Global.AllMethods.Add(this);
            this.methodCode.Add(methodCode);

        }

        public Method(string funcName, VarDef.EvarType returnType, NamespaceInfo parentNamespace, List<List<VarDef>> methodArguments, List<Command> methodCode) // Is a Main method and is not a void
        {
            this.funcName = funcName.ToLower();
            this.parentMethod = null;
            this.isSubmethod = false;
            this.isVoid = false;
            this.returnType = returnType;
            this.subMethods = new List<Method>();
            this.methodPrivateVars = new List<VarDef.EvarType>();
            this.parentNamespace = parentNamespace;
            parentNamespace.namespaceMethods.Add(this);
            methodLocation = GetMethodLocationString();
            this.methodArguments = new List<List<VarDef>>(methodArguments);
            Global.AllMethods.Add(this);
            this.methodCode.Add(methodCode);

        }

        private string GetMethodLocationString()
        {
            Method checkSubPath = this;
            string? result = null;
            do
            {
                if (result == null)
                    result = checkSubPath.funcName;
                else
                    result = checkSubPath.funcName + "." + result;
                if (!checkSubPath.isSubmethod)
                    return parentNamespace.Name + "." + result;

                checkSubPath = checkSubPath.parentMethod ?? throw new Exception("Internal: Parent method of current method is null for some reason.");
            } while (result.Length < 1024);
            throw new Exception("To large submethod-method name lenght. I could easily make the max lenght bigger, but I woun't. Haha!");
        }


    }

    class UnspecifiedMethod
    {
        public string methodName;
        public List<Command> methodCode;
        public UnspecifiedMethod(string methodName, List<Command> methodCode)
        {
            this.methodName = methodName;
            this.methodCode = new List<Command>(methodCode);
        }
    }


}
