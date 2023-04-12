namespace TASI
{
    public class VarDef
    {
        public VarDef(EvarType evarType, string varName)
        {
            if (evarType == EvarType.all)
            {
                isAllType = true;
                evarType = EvarType.@void;
            }
            varType = evarType;
            this.varName = varName.ToLower();
            this.isArray = false;


        }

        public enum EvarType
        {
            @num, @string, @bool, @void, @return, @all
        }
        public EvarType varType;
        public string varName;
        public bool isArray;
        public bool isAllType;
    }


}
