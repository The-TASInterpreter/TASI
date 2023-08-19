namespace TASI.Types.Definition.Visibility
{
    public class Private : VisibilityModifier
    {
        public Private(TypeDef parentTypeDef) : base(parentTypeDef) { }

        public override bool HasAccess(TypeDef accessingTypeDef)
        {
            if (accessingTypeDef == parentTypeDef)
                return true;
            else
                return false;
        }
    }
}
