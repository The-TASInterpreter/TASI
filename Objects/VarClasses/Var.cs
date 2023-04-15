

using TASI.Objects.VarClasses;

namespace TASI
{
    public class Var
    {
        public VarConstruct varConstruct;
        public VariableValueHolder varValueHolder;

        public Var(VarConstruct varConstruct, Value varValue)
        {
            this.varConstruct = varConstruct;

            

            this.varValueHolder = new(varValue);

            if (Value.ConvertValueTypeToVarType(VarValue.valueType) != varConstruct.type && varConstruct.type != VarConstruct.VarType.all) throw new CodeSyntaxException($"The variable \"{varConstruct.name}\" can't be initialized with a {varValue.valueType}-type value, because it's a {varConstruct.type}-type variable.");

        }

        public Value VarValue
        {
            get
            {
                return varValueHolder.value;
            }
            set
            {
                if (varConstruct.type == VarConstruct.VarType.all)
                {
                    varValueHolder.value = value;
                    return;
                }
                switch (value.valueType)
                {
                    case Value.ValueType.num:
                        if (varConstruct.type != VarConstruct.VarType.num) throw new CodeSyntaxException($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                        varValueHolder.value = value;
                        break;
                    case Value.ValueType.@bool:
                        if (varConstruct.type != VarConstruct.VarType.@bool) throw new CodeSyntaxException($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                        varValueHolder.value = value;
                        break;
                    case Value.ValueType.@string:
                        if (varConstruct.type != VarConstruct.VarType.@string) throw new CodeSyntaxException($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                        varValueHolder.value = value;
                        break;
                    default:
                        throw new InternalInterpreterException($"Internal: The {value.valueType}-type is not implemented.");
                }
            }


        }


    }
}
