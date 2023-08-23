using System.Net;
using TASI.LangCoreHandleInterface;
using TASI.RuntimeObjects.VarClasses;

namespace TASI.RuntimeObjects.FunctionClasses
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
        private FunctionHandler? functionHandle;
        public delegate Value? FunctionHandler(List<Value> list, AccessableObjects objs);

        public FunctionHandler FunctionHandle
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



        internal Function(string funcName, VarConstruct.VarType returnType, NamespaceInfo parentNamespace, List<List<VarConstruct>> functionArguments, List<Command> functionCode, Global global, FunctionHandler? functionHandle = null) // Is a Main function and is not a void
        {
            this.funcName = funcName.ToLower();
            parentFunction = null;
            isSubfunction = false;
            this.returnType = returnType;
            subFunctions = new List<Function>();
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
        
        /// <summary>
        /// Defines a default function
        /// </summary>
        /// <param Name="funcName"></param>
        /// <param Name="returnType"></param>
        /// <param Name="parentNamespace"></param>
        /// <param Name="functionCode"></param>
        /// <param Name="global"></param>
        /// <param Name="functionHandle"></param>

        public static void CreateFunctionToParentNamespace(string funcName, VarConstruct.VarType returnType, NamespaceInfo parentNamespace, List<List<VarConstruct>> functionArguments, List<Command> functionCode, Global global, FunctionHandler? functionHandle = null)
        {
            new Function(funcName, returnType, parentNamespace, functionArguments, functionCode, global, functionHandle);
        }


        /// <summary>
        /// Defines a default function which needs no input arguments
        /// </summary>
        /// <param Name="funcName"></param>
        /// <param Name="returnType"></param>
        /// <param Name="parentNamespace"></param>
        /// <param Name="functionCode"></param>
        /// <param Name="global"></param>
        /// <param Name="functionHandle"></param>
        public static void CreateFunctionToParentNamespace(string funcName, VarConstruct.VarType returnType, NamespaceInfo parentNamespace, List<Command> functionCode, Global global, FunctionHandler? handler)
        {
            new Function(funcName, returnType, parentNamespace, new() { new() }, functionCode, global, handler);
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
            throw new CodeSyntaxException("To large subfunction-function Name lenght. I could easily make the max lenght bigger, but I won't. Haha!");
        }


    }



}
