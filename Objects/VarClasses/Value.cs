namespace TASI
{
    public class Value
    {
        public double? numValue;
        public string? stringValue;
        public bool? boolValue;
        public ValueType valueType;

        public string StringValue
        {
            get
            {
                return stringValue ?? throw new Exception("Internal: string value is null");
            }
        }

        public double NumValue
        {
            get
            {
                switch (valueType)
                {
                    case ValueType.num:
                        return numValue ?? throw new Exception("Internal: num value is null.");
                    case ValueType.@bool:
                        return Convert.ToDouble(boolValue ?? throw new Exception("Internal: bool value is null."));
                    default: throw new Exception($"Internal: Can't convert a {valueType} to a numeric type.");

                }
            }
        }

        public Value(ValueType valueType, Object objectValue)
        {
            this.valueType = valueType;
            ObjectValue = objectValue;
        }

        public enum ValueType
        {
            @num, @string, @bool, @void
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

        public bool GetBoolValue
        {
            get
            {
                switch (valueType)
                {
                    case ValueType.num or ValueType.@bool:
                        if (numValue == null)
                            throw new Exception($"The value \"{valueType}\" can't be used, because it is not defined.");
                        if (numValue == 1) return true;
                        if (numValue == 0) return false;
                        if (valueType == ValueType.@bool)
                            throw new Exception("Internal: Bool is neither 0 or 1");

                        throw new Exception($"The num value \"{valueType}\" can't be converted to a bool, because it is neither 1 or 0.");

                    case ValueType.@string:
                        if (stringValue == null)
                            throw new Exception($"The value \"{valueType}\" can't be used, because it is not defined.");
                        if (stringValue == "1" || stringValue == "true") return true;
                        if (stringValue == "0" || stringValue == "false") return false;
                        throw new Exception($"The string value \"{valueType}\" can't be converted to a bool, because it is neither 1, 0, true or false.");

                    default:
                        throw new Exception($"The value type {valueType} can't be converted to a bool.");


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
                        return numValue ?? throw new Exception("Internal: num value of num-type value was null.");
                    case ValueType.@string:
                        return stringValue ?? throw new Exception("Internal: string value of string-type value was null.");
                    case ValueType.@bool:
                        return boolValue ?? throw new Exception("Internal: bool value of bool-type value was null.");
                    case ValueType.@void:
                        return "void";
                    default:
                        throw new Exception("Internal: Unimplemented value type.");

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
                    default:
                        throw new Exception("Internal: Unimplemented value type.");
                }
            }
        }


    }
}
