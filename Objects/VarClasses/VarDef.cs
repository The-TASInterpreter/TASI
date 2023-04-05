namespace TASI
{
    public class VarDef
    {
        public VarDef(EvarType evarType, string varName)
        {
            varType = evarType;
            this.varName = varName.ToLower();
            this.isArray = false;
        }
        public VarDef(EvarType evarType, string varName, bool isArray)
        {
            varType = evarType;
            this.varName = varName;
            if (evarType == EvarType.@void)
                throw new Exception("Can't create an array with type void. I mean what you wanna put in there lol?. E.U 0009");
            this.isArray = isArray;
        }
        public enum EvarType
        {
            @num, @string, @bool, @void, @return
        }
        public EvarType varType;
        public string varName;
        public bool isArray;
    }


}
