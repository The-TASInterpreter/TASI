using System.Text;
using TASI.Types.Definition.Visibility;
using TASI.Types.Instance;

namespace TASI.Types.Definition.Field
{
    public class Overload
    {
        public VisibilityModifier Modifyer { get; }
        public List<(TypeDef, string)> inputTypes;
        public readonly List<Command>? commands;
        public readonly OverloadHandler? methodHandler;

        public string GetCallName
        {
            get
            {
                throw new NotImplementedException();
                StringBuilder sb = new();

                for (int i = 0; i < inputTypes.Count; i++)
                {
                    (TypeDef, string) inputType = inputTypes[i];
                }
            }
        }


        public delegate TypeInstance OverloadHandler(List<TypeInstance> input, AccessableObjects objs, TypeInstance self);

        public Overload(List<(TypeDef, string)> inputTypes, OverloadHandler? methodHandler, VisibilityModifier modifyer)
        {
            this.inputTypes = inputTypes;
            this.methodHandler = methodHandler;
            Modifyer = modifyer;
        }

        public Overload(List<(TypeDef, string)> inputTypes, List<Command>? commands, VisibilityModifier modifyer)
        {
            this.inputTypes = inputTypes;
            this.commands = commands;
            Modifyer = modifyer;
        }



        public bool MatchesInput(List<TypeDef> inputTypes)
        {
            if (this.inputTypes.Count != inputTypes.Count)
                return false;
            for (int i = 0; i < inputTypes.Count; i++)
            {
                if (this.inputTypes[i].Item1 != inputTypes[i])
                    return false;
            }
            return true;
        }

        public static Overload? GetCorrectOverload(List<Overload> allOverloads, List<TypeInstance> findArgs)
        {
            return GetCorrectOverload(allOverloads, TypeInstance.ConverToTypeDef(findArgs));
        }
        public static Overload? GetCorrectOverload(List<Overload> allOverloads, List<TypeDef> findArgs)
        {

            return allOverloads.FirstOrDefault(x => x.MatchesInput(findArgs));
        }
    }
}
