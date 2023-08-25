using System.Text;
using TASI.Types.Definition.Things;
using TASI.Types.Definition.Visibility;
using TASI.Types.Instance;

namespace TASI.Types.Definition.Field
{
    public class OverloadImplementation
    {
        public VisibilityModifier Modifyer { get; }
        public List<string> inputNames ;
        public readonly List<Command>? commands;
        public readonly OverloadHandler? methodHandler;
        public OverloadDef overloadDef;
        public string GetCallInput
        {
            get
            {
                StringBuilder sb = new("(");

                for (int i = 0; i < inputNames.Count; i++)
                {
                    if (i != 0)
                        sb.Append(", ");
                    sb.Append(overloadDef.inputTypes[i].GetFullName);
                    sb.Append(": ");
                    sb.Append(inputNames[i]);

                }
                return sb.ToString();

            }
        }


        public delegate TypeInstance OverloadHandler(List<TypeInstance> input, AccessableObjects objs, TypeInstance self);

        public OverloadImplementation(List<string> inputNames, OverloadHandler? methodHandler, VisibilityModifier modifyer, OverloadDef overloadDef)
        {
            this.inputNames = inputNames;
            this.methodHandler = methodHandler;
            Modifyer = modifyer;
            this.overloadDef = overloadDef;
            ThrowIfIncorrectBuild();


        }

        public OverloadImplementation(List<string> inputNames, List<Command>? commands, VisibilityModifier modifyer, OverloadDef overloadDef)
        {
            this.inputNames = inputNames;
            this.commands = commands;
            Modifyer = modifyer;
            this.overloadDef = overloadDef;
            ThrowIfIncorrectBuild();
        }

        private void ThrowIfIncorrectBuild()
        {
            if (inputNames.Count != overloadDef.inputTypes.Count)
            {
                
                    throw new InternalInterpreterException("Internal name definition doesn't match type definition");

            }
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

        public static OverloadImplementation? GetCorrectOverload(List<OverloadImplementation> allOverloads, List<TypeInstance> findArgs)
        {
            return GetCorrectOverload(allOverloads, TypeInstance.ConverToTypeDef(findArgs));
        }
        public static OverloadImplementation? GetCorrectOverload(List<OverloadImplementation> allOverloads, List<TypeDef> findArgs)
        {

            return allOverloads.FirstOrDefault(x => x.MatchesInput(findArgs));
        }
    }
}
