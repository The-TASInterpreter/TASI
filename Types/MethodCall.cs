using TASI.InternalLangCoreHandle;
using TASI.Types.Definition.Field;
using TASI.Types.Definition.Things;
using TASI.Types.Instance;

namespace TASI.Types
{
    public class MethodCall
    {
        public string callMethodName;

        public MethodCall(string callMethodName)
        {
            this.callMethodName = callMethodName.ToLower();
        }

        public TypeInstance DoCall(TypeInstance baseInstance, List<TypeInstance> arguments, AccessableObjects accessableObjects)
        {
            MethodImplementation callMethod = GetCallMethod(baseInstance);
            Overload callOverload = callMethod.GetCorrectOverload(TypeInstance.ConverToTypeDef(arguments)) ?? throw new CodeSyntaxException($"The provided input types\n{TypeInstance.GetDescriberOfInput(arguments)}\ndidn't match any of the possible input types of the thing \"{callMethod.GetFullName}\" ");

            if (callMethod.ParentType.parentNamespace.namespaceIntend == RuntimeObjects.FunctionClasses.NamespaceInfo.NamespaceIntend.@internal)
            {
                TypeInstance internalReturn = (callOverload.methodHandler ?? throw new InternalInterpreterException("thing handler for internal thing was null")).Invoke(arguments, accessableObjects, baseInstance);
                if (internalReturn.typeDef != callMethod.returnType) throw new InternalInterpreterException("Internal thing didn't return expected value");
                return internalReturn;
            }

            AccessableObjects inMethod = new(new()
            {
                {"self", new(baseInstance.typeDef, baseInstance) }
            }, baseInstance.typeDef.parentNamespace, accessableObjects.global);
            for (int i = 0; i < arguments.Count; i++)
            {
                TypeInstance? arg = arguments[i];
                inMethod.accessableVars.Add(callOverload.inputTypes[i].Item2, new(callOverload.inputTypes[i].Item1, arguments[i]));
            }


            TypeInstance returnValue = InterpretMain.InterpretNormalMode(callOverload.commands ?? throw new InternalInterpreterException("Commands for overload was null"), inMethod) ?? throw new CodeSyntaxException($"The thing \"{callMethod.GetFullName}\" didn't return anything.");
            if (returnValue.typeDef != callMethod.returnType) throw new CodeSyntaxException($"Overload for thing \"{callMethod.GetFullName}\" didn't return the expected type of \"{callMethod.returnType.GetFullName}\" but rather \"{returnValue.typeDef.GetFullName}\".");
            return returnValue;
        }

        private MethodImplementation GetCallMethod(TypeInstance baseInstance)
        {
            Thing thing = baseInstance.typeDef.things.FirstOrDefault(x => callMethodName == x.Name) ?? throw new CodeSyntaxException($"\"{baseInstance.typeDef.GetFullName}\" doesn't have a definition for \"{callMethodName}\"");

            if (thing is MethodImplementation method)
                return method;
            else
                throw new CodeSyntaxException($"\"{callMethodName}\" can't be called as a thing because it is of type {thing.actualType}");



        }
    }
}
