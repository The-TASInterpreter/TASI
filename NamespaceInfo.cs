namespace Text_adventure_Script_Interpreter
{
    public class NamespaceInfo
    {
        public enum NamespaceIntend
        {
            Main, Supervisor, Generic, Internal
        }
        public string name;
        public List<Method> namespaceMethods = new List<Method>();
        public List<VarDef.evarType> namespaceVars = new List<VarDef.evarType>();
        public List<Var> publicNamespaceVars = new List<Var>();
        public NamespaceIntend namespaceIntend;


        public NamespaceInfo(NamespaceIntend namespaceIntend, string name)
        {
            Text_adventure_Script_Interpreter_Main.interpretInitLog.Log($"Creating new Namespace. Intend: {namespaceIntend}; Name: {name}");
            this.namespaceIntend = namespaceIntend;
            this.name = name;
        }

    }
    

    public class Var
    {
        public VarDef varDef;

        public bool tempVar;
        public double numValue;
        public string stringValue;
        public bool isLinkedVar;
        public string linkedMethod;
        public Var(VarDef.evarType varType, bool tempVar, object value, string varName)
        {
            varDef.varName = varName;
            this.varDef.varType = varType;
            this.tempVar = tempVar;
            switch (varType)
            {
                case VarDef.evarType.Num:
                    numValue = (double)value;
                    break;
                case VarDef.evarType.Bool: //Bool values are just num values *Shock*
                    if ((bool)value)
                        numValue = 1;
                    else
                        numValue = 0;
                    break;
                case VarDef.evarType.String:
                    stringValue = (string)value;
                    break;
                case VarDef.evarType.Void:
                    throw new Exception("Can't create a variable with the \"Void\" type. E.U.0008");
                default:
                    throw new Exception("Unknown variable type at NamespaceInfo.Var(Switch(vartype). E.Internal.0001");
            }

            isLinkedVar = false;
            linkedMethod = string.Empty;
        }
        public Var(VarDef.evarType varType, bool tempVar, object value, string varName, string linkedMethod)
        {
            varDef.varName = varName;
            this.varDef.varType = varType;
            this.tempVar = tempVar;
            switch (varType)
            {
                case VarDef.evarType.@Num:

                    break;
            }

            isLinkedVar = true;
            this.linkedMethod = linkedMethod;
        }

    }

    public class VarDef
    {
        public VarDef (evarType evarType, string varName)
        {
            varType = evarType;
            this.varName = varName;
            this.isArray = false;
        }
        public VarDef(evarType evarType, string varName, bool isArray)
        {
            varType = evarType;
            this.varName = varName;
            if (evarType == evarType.Void)
                throw new Exception("Can't create an array with type void. E.U 0009");
            this.isArray = isArray;
        }
        public enum evarType
        {
             @Num, @String, @Bool, @Void
        }
        public evarType varType;
        public string varName;
        public bool isArray;
    }


}
