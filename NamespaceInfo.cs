namespace TASI
{
    public class NamespaceInfo
    {
        public enum NamespaceIntend
        {
            Main, Supervisor, Generic, Internal
        }
        public string name;
        public List<Method> namespaceMethods = new();
        public List<VarDef.EvarType> namespaceVars = new();
        public List<Var> publicNamespaceVars = new();
        public NamespaceIntend namespaceIntend;


        public NamespaceInfo(NamespaceIntend namespaceIntend, string name)
        {
            TASI_Main.interpretInitLog.Log($"Creating new Namespace. Intend: {namespaceIntend}; Name: {name}");
            this.namespaceIntend = namespaceIntend;
            this.name = name;
        }

    }


    public class Var
    {
        public VarDef varDef;

        public bool tempVar;
        public double? numValue;
        public string? stringValue;
        public double[]? numArrayValue;
        public string[]? stringArrayValue;
        public Var? returnStatementValue;
        public bool isNumeric;


        public bool GetBoolValue
        {
            get
            {
                switch (varDef.varType)
                {
                    case VarDef.EvarType.Num or VarDef.EvarType.Bool:
                        if (numValue == null)
                            throw new Exception($"The variable \"{varDef.varName}\" can't be used, because it is not defined.");
                        if (numValue == 1) return true;
                        if (numValue == 0) return false;
                        if (varDef.varType == VarDef.EvarType.Bool)
                            throw new Exception("Internal: Bool is neither 0 or 1");

                        throw new Exception($"The num variable \"{varDef.varName}\" can't be converted to a bool, because it is neither 1 or 0.");

                    case VarDef.EvarType.String:
                        if (stringValue == null)
                            throw new Exception($"The variable \"{varDef.varName}\" can't be used, because it is not defined.");
                        if (stringValue == "1" || stringValue == "true") return true;
                        if (stringValue == "0" || stringValue == "false") return false;
                        throw new Exception($"The string variable \"{varDef.varName}\" can't be converted to a bool, because it is neither 1, 0, true or false.");

                    default:
                        throw new Exception("Invalid var type to convert to bool.");


                }
            }
        }

        public object ObjectValue
        {
            get
            {
                if (varDef.varType == VarDef.EvarType.Num || varDef.varType == VarDef.EvarType.Bool)
                    return numValue ?? throw new Exception("Internal: correct object value is null");
                else if (varDef.varType == VarDef.EvarType.Void)
                    return "void";
                else
                    return stringValue ?? throw new Exception("Internal: correct object value is null");
            }
            set
            {
                if (varDef.varType == VarDef.EvarType.Num || varDef.varType == VarDef.EvarType.Bool)
                    numValue = (double)value;
                else
                    stringValue = (string)value;
            }
        }


        public Var(VarDef varDef, bool tempVar, object? value)
        {
            this.varDef = varDef;
            this.tempVar = tempVar;
            switch (varDef.varType)
            {
                case VarDef.EvarType.Num:
                    isNumeric = true;
                    value ??= 0.0;
                    if (varDef.isArray == true)
                        numArrayValue = (double[])value;
                    else
                        numValue = (double)value;
                    break;
                case VarDef.EvarType.Bool: //Bool values are just num values *Shock*
                    isNumeric = true;
                    if (varDef.isArray == true)
                        throw new Exception("Sorry, but there are no bool arrays rn. Gonna add them in later. I promise!");
                    value ??= 0.0;
                    if ((bool)value)
                        numValue = 1;
                    else
                        numValue = 0;
                    break;
                case VarDef.EvarType.String:
                    isNumeric = false;
                    value ??= "";
                    if (varDef.isArray == true)
                        stringArrayValue = (string[])value;
                    else
                        stringValue = (string)value;
                    break;
                case VarDef.EvarType.Void:
                    throw new Exception("Can't create a variable with the \"Void\" type. E.U.0008");
                default:
                    throw new Exception("Unknown variable type at NamespaceInfo.Var(Switch(vartype). E.Internal.0001");
            }
        }

        public Var(Var varValue)
        {
            this.varDef = new(VarDef.EvarType.Return, "");
            this.tempVar = true;
            this.returnStatementValue = varValue;
        }

        public Var()
        {
            tempVar = true;
            varDef = new(VarDef.EvarType.Void, "");
            isNumeric = false;


        }


    }

    public class VarDef
    {
        public VarDef(EvarType evarType, string varName)
        {
            varType = evarType;
            this.varName = varName;
            this.isArray = false;
        }
        public VarDef(EvarType evarType, string varName, bool isArray)
        {
            varType = evarType;
            this.varName = varName;
            if (evarType == EvarType.Void)
                throw new Exception("Can't create an array with type void. I mean what you wanna put in there lol?. E.U 0009");
            this.isArray = isArray;
        }
        public enum EvarType
        {
            @Num, @String, @Bool, @Void, @Return
        }
        public EvarType varType;
        public string varName;
        public bool isArray;
    }


}
