using TASI.RuntimeObjects;
using TASI.Types.Instance;

namespace TASI.Types.Definition.Field
{
    public class Overload
    {
        public List<TypeDef> inputTypes;
        public readonly List<Command>? commands;
        public readonly MethodHandler? methodHandler;

        public delegate TypeInstance MethodHandler(List<Value> input, AccessableObjects objs, TypeInstance self);

        public Overload(List<TypeDef> inputTypes, MethodHandler? methodHandler)
        {
            this.inputTypes = inputTypes;
            this.methodHandler = methodHandler;
        }

        public Overload(List<TypeDef> inputTypes, List<Command>? commands)
        {
            this.inputTypes = inputTypes;
            this.commands = commands;
        }



        public bool MatchesInput(List<TypeDef> inputTypes)
        {
            if (this.inputTypes.Count != inputTypes.Count)
                return false;
            for (int i = 0; i < inputTypes.Count; i++)
            {
                if (this.inputTypes[i] != inputTypes[i])
                    return false;
            }
            return true;
        }
    }
}
