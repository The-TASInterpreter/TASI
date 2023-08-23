using TASI.Types.Definition.Visibility;

namespace TASI.Types.Definition.Things
{
    public class AccessorDef : ThingDef
    {
        public enum Access
        {
            get, set
        }

        public VisibilityModifier? getterModifyer;
        public VisibilityModifier? setterModifyer;
        public Access access;
        public AccessorDef(string name, bool isUnimplemented, TypeDef parentType, Access access, VisibilityModifier? getterModifyer, VisibilityModifier? setterModifyer) : base(name, isUnimplemented, parentType)
        {
            this.access = access;

            if (access == Access.get && getterModifyer == null)
                throw new InternalInterpreterException("Access is get but getter modifyer is not defined");
            if (access == Access.set && setterModifyer == null)
                throw new InternalInterpreterException("Access is set but setter modifyer is not defined");
            this.getterModifyer = getterModifyer;
            this.setterModifyer = setterModifyer;
        }

        public AccessorDef(string name, bool isUnimplemented, TypeDef parentType, VisibilityModifier getterModifyer)
            : this(name, isUnimplemented, parentType, Access.get, getterModifyer, null)
        { }

        public AccessorDef(string name, bool isUnimplemented, TypeDef parentType, VisibilityModifier getterModifyer, VisibilityModifier setterModifyer)
            : this(name, isUnimplemented, parentType, Access.get | Access.set, getterModifyer, setterModifyer)
        { }

    }
}
