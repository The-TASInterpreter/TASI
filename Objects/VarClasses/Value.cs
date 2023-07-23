
namespace TASI
{
    public class Value
    {
        public enum SpecialReturns
        {
            loop
        }


        public SpecialReturns? specialReturn = null;
        public object? value;
        public ValueType? valueType;
        public bool isReturnValue;
        public bool isConstant;
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
                if (value is not List<Value>)
                    if (valueType == ValueType.list)
                        throw new InternalInterpreterException("Internal: value is not a list value");
                    else
                        throw new InternalInterpreterException("Internal: Tried to access list value of non-list type");
                return (List<Value>)value;
            }
        }

        public string StringValue
        {
            get
            {
                if (value is not string)
                    if (valueType == ValueType.@string)
                        throw new InternalInterpreterException("Internal: value is not a string value");
                    else
                        throw new InternalInterpreterException("Internal: Tried to access string value of non-string type");
                return (string)value;
            }
        }

        public double NumValue
        {
            get
            {
                switch (valueType)
                {
                    case ValueType.num:
                        if (value is not double)
                            throw new InternalInterpreterException("Internal: value is not a double value");
                        return (double)value;
                    case ValueType.@int:
                        if (value is not int)
                            throw new InternalInterpreterException("Internal: value is not an int value");
                        return (int)value;
                    case ValueType.@bool:
                        if (value is not bool)
                            throw new InternalInterpreterException("Internal: value is not a bool value");
                        switch ((bool)value)
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
                case ValueType.@int:
                    return VarConstruct.VarType.@int;
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
                case ValueType.@int:
                    ObjectValue = (int)0;
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
            @num, @string, @bool, @void, @int, @list
        }

        public bool IsNumeric
        {
            get
            {
                return valueType switch
                {
                    ValueType.num or ValueType.@bool or ValueType.@int => true,
                    _ => false,
                };
            }
        }

        public bool IsConstant
        {
            get
            {
                return isConstant;
            }
            set
            {
                isConstant = value;
            }
        }

        public bool BoolValue
        {
            get
            {
                switch (valueType)
                {
                    case ValueType.num:
                        if (value is not double) throw new InternalInterpreterException("Internal: value is not a double value");
                        if ((double)value == 1) return true;
                        if ((double)value == 0) return false;
                        throw new CodeSyntaxException($"The num value \"{valueType}\" can't be converted to a bool, because it is neither 1 or 0.");
                    case ValueType.@int:
                        if (value is not int) throw new InternalInterpreterException("Internal: value is not a int value");
                        if ((int)value == 1) return true;
                        if ((int)value == 0) return false;
                        throw new CodeSyntaxException($"The int value \"{valueType}\" can't be converted to a bool, because it is neither 1 or 0.");
                    case ValueType.@bool:
                        if (value is not bool) throw new InternalInterpreterException("Internal: value is not a bool value");
                        return (bool)value;



                    case ValueType.@string:
                        if (value is not string) throw new InternalInterpreterException("Internal: value is not a string value");
                        if ((string)value == "1" || (string)value == "true") return true;
                        if ((string)value == "0" || (string)value == "false") return false;
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
                        if (value is not double)
                            throw new InternalInterpreterException("Internal: value is not a double value");
                        return value;
                    case ValueType.@int:
                        if (value is not int)
                            throw new InternalInterpreterException("Internal: value is not an int value");
                        return value;
                    case ValueType.@string:
                        if (value is not string)
                            throw new InternalInterpreterException("Internal: value is not a string value");
                        return value;
                    case ValueType.@bool:
                        if (value is not bool)
                            throw new InternalInterpreterException("Internal: value is not a bool value");
                        return value;
                    case ValueType.@list:
                        if (value is not List<Value>)
                            throw new InternalInterpreterException("Internal: value is not a list value");
                        return value;
                    case ValueType.@void:
                        return "void";
                    default:
                        throw new InternalInterpreterException("Internal: Unimplemented value type.");

                }
            }
            set
            {
                this.value = value;
            }
        }


    }
}
