namespace TASI.Types.Definition.Field
{
    public class Method
    {
        private TypeDef parentType;

        public TypeDef returnValue;
        public List<Overload> overloads;
        public string name;

        public Method(TypeDef parentType, string name, TypeDef returnValue, List<Overload> overloads)
        {
            this.parentType = parentType;
            this.returnValue = returnValue;
            this.overloads = overloads;
            this.name = name;
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
