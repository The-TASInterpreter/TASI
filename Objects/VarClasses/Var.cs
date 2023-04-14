

namespace TASI
{
    public class Var
    {
        public VarConstruct varConstruct;
        private Value varValue;

        public Var(VarConstruct varConstruct, Value varValue)
        {
            this.varConstruct = varConstruct;
            VarValue = varValue;
        }

        public Value VarValue
        {
            get
            {
                return varValue;
            }
            set
            {
                if (varConstruct.type == VarConstruct.VarType.all)
                {
                    varValue = value;
                    return;
                }
                switch (value.valueType)
                {
                    case Value.ValueType.num:
                        if (varConstruct.type != VarConstruct.VarType.num) throw new Exception($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                        varValue = value;
                        break;
                    case Value.ValueType.@bool:
                        if (varConstruct.type != VarConstruct.VarType.@bool) throw new Exception($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                        varValue = value;
                        break;
                    case Value.ValueType.@string:
                        if (varConstruct.type != VarConstruct.VarType.@string) throw new Exception($"{value.valueType} is not the expected {varConstruct.type}-type, the \"{varConstruct.name}\" variable expects");
                        varValue = value;
                        break;
                    default:
                        throw new Exception($"Internal: The {value.valueType}-type is not implemented.");
                }
            }


        }


    }
}
