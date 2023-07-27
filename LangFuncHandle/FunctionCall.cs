
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using static TASI.Command;

namespace TASI
{
    public class FunctionCall : Call
    {
        private Function? callFunction;


        public Function CallFunction
        {
            get
            {
                if (callFunction == null)
                {
                    throw new InternalInterpreterException($"The function call call function \"{callName}\" was not activated.");
                }
                return callFunction;

            }
            set { callFunction = value; }
        }


        public FunctionCall(Function callFunction, List<Value> inputVars) : base(inputVars)
        {
            this.CallFunction = callFunction;        }

        public FunctionCall(Command command, Global global) : base(command, global)
        {
            
        }

        public override void SearchCallNameObject(NamespaceInfo currentNamespace, Global global) //This is there, so header analysis can be done, without any errors.
        {

            CallFunction = FindFunctionByPath(callName.ToLower(), global.Namespaces, true, currentNamespace);
            foreach (CommandLine commandLine in argumentCommands)
                foreach (Command command in commandLine.commands)
                {
                    if (command.commandType == Command.CommandTypes.FunctionCall) command.functionCall.SearchCallNameObject(currentNamespace, global);
                    if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(currentNamespace, global);
                    if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(currentNamespace, global);

                }
        }

        public CallInputHelp? CheckIfFunctionCallHasValidArgTypesAndReturnCode(List<Value> inputVars)
        {
            bool matching;
            for (int j = 0; j < CallFunction.functionArguments.Count; j++)
            {
                List<VarConstruct> functionInputType = CallFunction.functionArguments[j];
                if (functionInputType.Count == inputVars.Count)
                {
                    matching = true;
                    for (int i = 0; i < inputVars.Count; i++)
                    {
                        if ((functionInputType[i].type != Value.ConvertValueTypeToVarType(inputVars[i].valueType ?? throw new InternalInterpreterException("Value type of value was null")) && functionInputType[i].type != VarConstruct.VarType.all) || (inputVars[i].comesFromVarValue == null && functionInputType[i].isLink))
                        {
                            matching = false;
                            break;
                        }
                    }
                    if (matching)
                    {
                        if (CallFunction.parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.@internal)
                            return new(new(), new());
                        return new(CallFunction.functionCode[j], functionInputType);
                    }
                }
            }
            return null;
        }


        public static NamespaceInfo? FindNamespaceByName(string name, List<NamespaceInfo> namespaces, bool exceptionAtNotFound)
        {

            foreach (NamespaceInfo namespaceInfo in namespaces)
                if (namespaceInfo.Name == name)
                    return namespaceInfo;

            if (exceptionAtNotFound)
                throw new CodeSyntaxException($"The namespace \"{name}\" was not found.");
            else
                return null;
        }

        public static Function? FindFunctionByPath(string name, List<NamespaceInfo> parentNamespaces, bool exceptionAtNotFound, NamespaceInfo? currentNamespace)
        {

            string[] nameSplit = name.Split('.');
            if (nameSplit.Length < 2)
                throw new CodeSyntaxException($"Can't find Function \"{name}\" because there is no Function in the location.");
            NamespaceInfo? parentNamespace = FindNamespaceByName(nameSplit[0], parentNamespaces, exceptionAtNotFound);
            if (parentNamespace == null)
                return null;
            List<Function> functions = parentNamespace.namespaceFuncitons;
            Function? currentFunction = null;
            for (int i = 0; i < nameSplit.Length - 1; i++)
            {
                currentFunction = null;
                foreach (Function function in functions)
                {
                    if (function.funcName == nameSplit[i + 1])
                    {
                        functions = function.subFunctions;
                        currentFunction = function;
                        break;
                    }
                }
                if (currentFunction == null)
                {
                    if (exceptionAtNotFound)
                        throw new CodeSyntaxException($"Could not find function \"{name}\".");
                    else
                        return null;
                }

            }

            if (currentNamespace != null)
                if (!currentNamespace.accessableNamespaces.Contains(currentFunction.parentNamespace)) throw new CodeSyntaxException($"The function \"{currentFunction.functionLocation}\" can't be accessed in the \"{currentNamespace.Name}\" namespace, because the \"{currentFunction.parentNamespace.Name}\" namespace wasn't imported. ");

            return currentFunction;

        }

        public override Value DoCall(AccessibleObjects accessableObjects)
        {
            GetInputValues(accessableObjects);

            CallInputHelp? functionCallInputHelp = CheckIfFunctionCallHasValidArgTypesAndReturnCode(argumentValues) ?? throw new CodeSyntaxException($"The function \"{CallFunction.functionLocation}\" doesent support the provided input types. Use the syntax \"helpm <function call>;\"\nFor this function it would be: \"helpm [{CallFunction.functionLocation}];\"");
            if (CallFunction.parentNamespace.namespaceIntend == NamespaceInfo.NamespaceIntend.@internal)
            {
                Value? returnValue = InternalFunctionHandle.HandleInternalFunc(CallFunction.functionLocation, argumentValues, accessableObjects);
                if (accessableObjects.global.DebugMode)
                {
                    Console.WriteLine($"Did function call to {CallFunction.parentNamespace.namespaceIntend}-intend {CallFunction.functionLocation}.\nIt returns a {CallFunction.returnType}.\nIt returned a {returnValue.valueType}-type with a value of \"{returnValue.ObjectValue}\".");
                    Console.ReadKey();
                }
                return returnValue;

            }
            
            //return 
            Hashtable functionCallInput = new();

            for (int i = 0; i < argumentValues.Count; i++)
            {
                if (functionCallInputHelp.inputVarType[i].type == VarConstruct.VarType.all)
                    //new VarDef(inputValues[i].varDef.varType, functionCallInputHelp.inputVarType[i].varName), this.inputValues[i].ObjectValue
                    functionCallInput.Add(functionCallInputHelp.inputVarType[i].name, new Var(new VarConstruct(VarConstruct.VarType.all, functionCallInputHelp.inputVarType[i].name), new(argumentValues[i])));
                else
                    functionCallInput.Add(functionCallInputHelp.inputVarType[i].name, new Var(new VarConstruct(Value.ConvertValueTypeToVarType(argumentValues[i].valueType ?? throw new InternalInterpreterException("Value type of value was null")), functionCallInputHelp.inputVarType[i].name), new(argumentValues[i])));
                if (argumentValues[i].comesFromVarValue != null)
                {
                    ((Var?)functionCallInput[functionCallInputHelp.inputVarType[i].name] ?? throw new InternalInterpreterException("Internal: Something, that wasn't supposed to be null ever ended up to be null")).varValueHolder = argumentValues[i].comesFromVarValue.varValueHolder;
                }
            }

            Value? functionReturnValue = InterpretMain.InterpretNormalMode(functionCallInputHelp.inputCode, new(functionCallInput, CallFunction.parentNamespace, accessableObjects.global));



            if (functionReturnValue == null || (Value.ConvertValueTypeToVarType(functionReturnValue.valueType ?? throw new InternalInterpreterException("Value type of value was null")) != CallFunction.returnType && CallFunction.returnType != VarConstruct.VarType.all))
                throw new CodeSyntaxException($"The function \"{CallFunction.functionLocation}\" didn't return the expected {CallFunction.returnType}-type.");
            return functionReturnValue;

            //throw new InternalInterpreterException("Internal: Only internal functions are implemented");
        }

    }

    public class CallInputHelp
    {
        public List<Command> inputCode;
        public List<VarConstruct> inputVarType;
        public CallInputHelp(List<Command> inputCode, List<VarConstruct> inputVarType)
        {
            this.inputCode = inputCode;
            this.inputVarType = inputVarType;
        }
    }
}

