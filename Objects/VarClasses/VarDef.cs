namespace TASI
{
    public class VarConstruct
    {
        public enum VarType
        {
            num, @string, @bool, @void, all
        }
        public string name;
        public VarType type;

        public VarConstruct(VarType type, string name)
        {
            this.name = name.ToLower();
            this.type = type;
        }
    }
}