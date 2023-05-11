
namespace TASI
{
    public class Value
    {
        public enum SpecialReturns
        {
            loop
        }


        public SpecialReturns? specialReturn = null;

        public double? numValue;
        public string? stringValue;
        public bool? boolValue;
        public ValueType? valueType;
        public bool isReturnValue;
        public List<Value>? listValue;
        public Var? comesFromVarValue = null;

        public Value(SpecialReturns specialReturn)
        {
            valueType = null;
            this.specialReturn = specialReturn;
        }

        public List<Value> ListValue
        {
            get
            {
                return listValue ?? throw new InternalInterpreterException("Internal: list value is null");
            }
        }

        public string StringValue
        {
            get
            {
                return stringValue ?? throw new InternalInterpreterException("Internal: string value is null");
            }
        }

        public double NumValue
        {
            get
            {
                switch (valueType)
                {
                    case ValueType.num:
                        return numValue ?? throw new InternalInterpreterException("Internal: num value is null.");
                    case ValueType.@bool:
                        switch (boolValue ?? throw new InternalInterpreterException("Internal: bool value is null."))
                        {
                            case true:
                                return 1;
                            case false:
                                return 0;
                        }
                    default: throw new InternalInterpreterException($"Internal: Can't convert a {valueType} to a numeric type.");

                }
            }
        }

        public static VarConstruct.VarType ConvertValueTypeToVarType(ValueType valueType)
        {
            switch (valueType)
            {
                case ValueType.num:
                    return VarConstruct.VarType.num;
                case ValueType.@bool:
                    return VarConstruct.VarType.@bool;
                case ValueType.@string:
                    return VarConstruct.VarType.@string;
                case ValueType.@void:
                    return VarConstruct.VarType.@void;
                case ValueType.list:
                    return VarConstruct.VarType.list;
                default: throw new InternalInterpreterException("Internal: Unimplemented Value Type.");
            }
        }

        public Value(Value value)
        {
            valueType = value.valueType;
            ObjectValue = value.ObjectValue;
        }

        public Value(ValueType valueType)
        {
            this.valueType = valueType;
            switch (valueType)
            {
                case ValueType.num:
                    ObjectValue = (double)0;
                    break;
                case ValueType.@bool:
                    ObjectValue = false;
                    break;
                case ValueType.@string:
                    ObjectValue = String.Empty;
                    break;
                case ValueType.list:
                    ObjectValue = new List<Value>();
                    break;
                
            }
        }
        public Value()
        {
            valueType = ValueType.@void;
        }
        public Value(ValueType valueType, Object objectValue)
        {
            this.valueType = valueType;
            ObjectValue = objectValue;
        }



        public enum ValueType
        {
            @num, @string, @bool, @void, @list
        }

        public bool IsNumeric
        { 
            get
            {
                return valueType switch
                {
                    ValueType.num or ValueType.@bool => true,
                    _ => false,
                };
            }
        }

        public bool BoolValue
        {
            get
            {
                switch (valueType)
                {
                    case ValueType.num:
                        if (numValue == null)
                            throw new CodeSyntaxException($"The value \"{valueType}\" can't be used, because it is not defined.");
                        if (numValue == 1) return true;
                        if (numValue == 0) return false;
                        throw new CodeSyntaxException($"The num value \"{valueType}\" can't be converted to a bool, because it is neither 1 or 0.");
                    case ValueType.@bool:
                        return boolValue ?? throw new InternalInterpreterException("Internal: bool value is null.");



                    case ValueType.@string:
                        if (stringValue == null)
                            throw new CodeSyntaxException($"The value \"{valueType}\" can't be used, because it is not defined.");
                        if (stringValue == "1" || stringValue == "true") return true;
                        if (stringValue == "0" || stringValue == "false") return false;
                        throw new CodeSyntaxException($"The string value \"{valueType}\" can't be converted to a bool, because it is neither 1, 0, true or false.");

                    default:
                        throw new CodeSyntaxException($"The value type {valueType} can't be converted to a bool.");


                }
            }
        }


        public Object ObjectValue
        {
            get
            {
                switch (valueType)
                {
                    case ValueType.num:
                        return numValue ?? throw new InternalInterpreterException("Internal: num value of num-type value was null.");
                    case ValueType.@string:
                        return stringValue ?? throw new InternalInterpreterException("Internal: string value of string-type value was null.");
                    case ValueType.@bool:
                        return boolValue ?? throw new InternalInterpreterException("Internal: bool value of bool-type value was null.");
                    case ValueType.@list:
                        return listValue ?? throw new InternalInterpreterException("Internal: list value of list-type was null");
                    case ValueType.@void:
                        return "void";
                    default:
                        throw new InternalInterpreterException("Internal: Unimplemented value type.");

                }
            }
            set
            {
                switch (valueType)
                {
                    case ValueType.num:
                        numValue = (double)value;
                        break;
                    case ValueType.@string:
                        stringValue = (string)value;
                        break;
                    case ValueType.@bool:
                        boolValue = (bool)value;
                        break;
                    case ValueType.@list:
                        listValue = (List<Value>)value;
                        break;
                    default:
                        throw new InternalInterpreterException("Internal: Unimplemented value type.");
                }
            }
        }


    }
}
