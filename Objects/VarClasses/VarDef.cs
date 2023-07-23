namespace TASI
{
    public class VarConstruct
    {
        public enum VarType
        {
            num, @string, @bool, @void, @int, list, all, @object
        }
        private TASIObjectDefinition? objectDefinition;

        


        public TASIObjectDefinition ObjectDefinitions
        {
            get
            {
                if (objectDefinition == null)
                {
                    if (type == VarType.@object)
                    {
                        throw new InternalInterpreterException("Internal: Object definition for Var def was null");
                    } 
                    else
                    {
                        throw new InternalInterpreterException("Internal: Tryed to access Object definition of non object var type");
                    }
                }
                return objectDefinition;
            }
        }

        public string name;
        public VarType type;
        public bool isLink;

        public VarConstruct(VarType type, string name, bool isLink = false )
        {
            this.name = name.ToLower();
            this.type = type;
            this.isLink = isLink;
        }
    }
}