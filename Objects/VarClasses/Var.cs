namespace TASI
{
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

        public bool IsNumeric
        {
            get
            {
                return varDef.varType switch
                {
                    VarDef.EvarType.num or VarDef.EvarType.@bool => true, _ => false,
                };
            }
        }

        public bool GetBoolValue
        {
            get
            {
                switch (varDef.varType)
                {
                    case VarDef.EvarType.num or VarDef.EvarType.@bool:
                        if (numValue == null)
                            throw new Exception($"The variable \"{varDef.varName}\" can't be used, because it is not defined.");
                        if (numValue == 1) return true;
                        if (numValue == 0) return false;
                        if (varDef.varType == VarDef.EvarType.@bool)
                            throw new Exception("Internal: Bool is neither 0 or 1");

                        throw new Exception($"The num variable \"{varDef.varName}\" can't be converted to a bool, because it is neither 1 or 0.");

                    case VarDef.EvarType.@string:
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
                if (varDef.varType == VarDef.EvarType.num || varDef.varType == VarDef.EvarType.@bool)
                    return numValue ?? throw new Exception("Internal: correct object value is null");
                else if (varDef.varType == VarDef.EvarType.@void)
                    return "void";
                else
                    return stringValue ?? throw new Exception("Internal: correct object value is null");
            }
            set
            {
                if (varDef.varType == VarDef.EvarType.num || varDef.varType == VarDef.EvarType.@bool)
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
                case VarDef.EvarType.num:

                    value ??= 0.0;
                    if (varDef.isArray == true)
                        numArrayValue = (double[])value;
                    else
                        numValue = (double)value;
                    break;
                case VarDef.EvarType.@bool: //Bool values are just num values *Shock*

                    if (varDef.isArray == true)
                        throw new Exception("Sorry, but there are no bool arrays rn. Gonna add them in later. I promise!");
                    value ??= 0.0;
                    if ((bool)value)
                        numValue = 1;
                    else
                        numValue = 0;
                    break;
                case VarDef.EvarType.@string:

                    value ??= "";
                    if (varDef.isArray == true)
                        stringArrayValue = (string[])value;
                    else
                        stringValue = (string)value;
                    break;
                case VarDef.EvarType.@void:
                    break;
                default:
                    throw new Exception("Internal: Unknown variable type at NamespaceInfo.Var(Switch(vartype).");
            }
        }

        public Var(Var varValue)
        {
            this.varDef = new(VarDef.EvarType.@return, "");
            this.tempVar = true;
            this.returnStatementValue = varValue;
        }

        public Var()
        {
            tempVar = true;
            varDef = new(VarDef.EvarType.@void, "");



        }


    }


}
