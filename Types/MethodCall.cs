using TASI.InternalLangCoreHandle;
using TASI.Types.Definition.Field;
using TASI.Types.Instance;

namespace TASI.Types
{
    public class MethodCall
    {
        public string callMethodName;


        public TypeInstance DoCall(TypeInstance baseInstance, List<TypeInstance> arguments, AccessableObjects accessableObjects)
        {
            Method callMethod = GetCallMethod(baseInstance);
            Overload? callOverload = callMethod.GetCorrectOverload(TypeInstance.ConverToTypeDef(arguments));
            AccessableObjects inMethod = new(new()
            {
                {"this", new() }
            }, baseInstance.typeDef.parentNamespace, accessableObjects.global);
            

            TypeInstance returnValue = InterpretMain.InterpretNormalMode()
        }

        private Method GetCallMethod(TypeInstance baseInstance)
        {
            throw new NotImplementedException();
        }
    }
}
