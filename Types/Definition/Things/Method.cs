using TASI.Types.Definition.Things;
using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Field
{
    public class Method : Thing
    {
        public TypeDef ParentType { get; }

        public TypeDef returnType;
        public List<Overload> overloads;


        public string GetFullName
        {
            get
            {
                return $"{ParentType.GetFullName}.{name}";
            }
        }

        public override bool isStatic => true;

        public override string actualType => "Method";

        public Method(TypeDef parentType, string name, TypeDef returnValue, List<Overload> overloads) : base(name, false)
        {
            this.ParentType = parentType;
            this.returnType = returnValue;
            this.overloads = overloads;
        }
        public Method(TypeDef parentType, string name, TypeDef returnValue) : base(name, true)
        {
            this.ParentType = parentType;
            this.returnType = returnValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>The fitting overload for given input types.
        /// If there was no matching overload found it'll return null</returns>
        public Overload? GetCorrectOverload(List<TypeDef> input)
        {
            return overloads.FirstOrDefault(x => x.MatchesInput(input));
        }
    }
}
