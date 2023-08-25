using TASI.Types.Definition.Things;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Field
{
    public class MethodImplementation : Thing
    {
        public TypeDef ParentType { get; }

        public TypeDef returnType;
        public List<OverloadImplementation> overloads;


        public string GetFullName
        {
            get
            {
                return $"{ParentType.GetFullName}.{Name}";
            }
        }

        public override bool isStatic => true;

        public override ThingType actualType => ThingType.Method;

        public MethodImplementation(TypeDef parentType, string name, TypeDef returnValue, List<OverloadImplementation> overloads) : base(name, false)
        {
            this.ParentType = parentType;
            this.returnType = returnValue;
            this.overloads = overloads;
        }
        public MethodImplementation(TypeDef parentType, string name, TypeDef returnValue) : base(name, true)
        {
            this.ParentType = parentType;
            this.returnType = returnValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param Name="input"></param>
        /// <returns>The fitting overload for given input types.
        /// If there was no matching overload found it'll return null</returns>
        public OverloadImplementation? GetCorrectOverload(List<TypeDef> input)
        {
            return overloads.FirstOrDefault(x => x.MatchesInput(input));
        }
    }
}
