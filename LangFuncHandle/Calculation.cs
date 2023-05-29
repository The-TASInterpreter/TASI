using static TASI.Command;

namespace TASI
{
    public class Calculation
    {
        public static Value DoCalculation(Command command, AccessableObjects accessableObjects)
        {
            if (command.commandType != Command.CommandTypes.Calculation) throw new InternalInterpreterException("Internal: Do calculation only works with num calculation tokens");
            Value returnValue = (command.calculation ?? throw new InternalInterpreterException("Internal: Calculation was not parsed.")).GetValue(accessableObjects);
            return returnValue;
        }



    }


    public class CalculationType
    {
        public void InitFunctions(NamespaceInfo currentNamespace)
        {
            switch (type)
            {
                case Type.calculation:
                    foreach (CalculationType calculationType in subValues)
                    {
                        calculationType.InitFunctions(currentNamespace);
                    }
                    break;
                case Type.returnStatement:
                    foreach (Command command in returnStatement)
                    {
                        if (command.commandType == Command.CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(currentNamespace);
                        if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(currentNamespace);
                        if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(currentNamespace);
                    }
                    break;
                case Type.function:
                    functionCall.SearchCallFunction(currentNamespace);
                    break;





            }
            return;
        }

        public Value GetValue(AccessableObjects accessableObjects)
        {
            switch (type)
            {
                case Type.value: return value ?? throw new InternalInterpreterException("Internal: value is null.");
                case Type.returnStatement: return Statement.ReturnStatement(returnStatement ?? throw new InternalInterpreterException("Internal: return statement list is null."), accessableObjects);
                case Type.@operator: throw new InternalInterpreterException("Internal: Can't get the value of an operator.");
                case Type.function: return (functionCall ?? throw new InternalInterpreterException("Internal: function call is null")).DoFunctionCall(accessableObjects);
                case Type.calculation:
                    List<Value> values = new();
                    string currentOperator = "";

                    foreach (CalculationType calculationType in subValues ?? throw new InternalInterpreterException("Internal: sub values is null"))
                    {
                        switch (calculationType.type)
                        {
                            case Type.@operator:
                                if (currentOperator != "")
                                {

                                    values = new() { SimulateOperator(values, currentOperator) };
                                }
                                currentOperator = calculationType.@operator;
                                break;
                            default:
                                values.Add(calculationType.GetValue(accessableObjects));
                                break;
                        }
                    }
                    if (currentOperator != "")
                        values = new() { SimulateOperator(values, currentOperator) };
                    if (values.Count == 1) return values[0];
                    if (values.Count == 0) throw new CodeSyntaxException("You can't end up with 0 values at the end of a num calculation (I don't even know how you managed to do this, but it's 100% your fault).");
                    throw new CodeSyntaxException("You can't end up with more than 1 value at the end of a num calculation.");
                default: throw new InternalInterpreterException($"Internal: Unexpected \"{type}\" CalculationType.Type");
            }
        }

        public static Value SimulateOperator(List<Value> values, string @operator)
        {
            switch (@operator.ToLower())
            {
                case "+":
                    if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for an addition operator.");
                    if (values[0].valueType == Value.ValueType.@string || values[1].valueType == Value.ValueType.@string) return new(Value.ValueType.@string, values[0].ObjectValue.ToString() + values[1].ObjectValue.ToString());
                    return new(Value.ValueType.num, values[0].NumValue + values[1].NumValue);
                case "-":
                    if (values.Count == 1)
                    {
                        if (!values[0].IsNumeric) throw new CodeSyntaxException($"You can't have a negative non-numeric {values[0].valueType}-type.");
                        return new(Value.ValueType.num, -values[0].NumValue);
                    }
                    else if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for a subtraction operator.");
                    if (values.Any(x => !x.IsNumeric)) throw new CodeSyntaxException("You can't substract with a non-number type.");
                    return new(Value.ValueType.num, values[0].NumValue - values[1].NumValue);
                case "*":
                    if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for a multiplication operator.");
                    if (values.Any(x => !x.IsNumeric)) throw new CodeSyntaxException("You can't multiply with a non-number type.");
                    return new(Value.ValueType.num, values[0].NumValue * values[1].NumValue);
                case "/":
                    if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for a division operator.");
                    if (values.Any(x => !x.IsNumeric)) throw new CodeSyntaxException("You can't devide with a non-number type.");
                    if (values[1].NumValue == 0) throw new CodeSyntaxException("Devision by zero.");
                    return new(Value.ValueType.num, values[0].NumValue / values[1].NumValue);
                case "and":
                    if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for an and operator.");
                    return new(Value.ValueType.@bool, values[0].BoolValue && values[1].BoolValue);
                case "or":
                    if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for an or operator.");
                    return new(Value.ValueType.@bool, values[0].BoolValue || values[1].BoolValue);
                case "!" or "not":
                    if (values.Count != 1) throw new CodeSyntaxException("You need 1 value for a not operator.");
                    return new(Value.ValueType.@bool, !values[0].BoolValue);
                case "%":
                    if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for a modulus operator.");
                    if (values.Any(x => !x.IsNumeric)) throw new CodeSyntaxException("You can't mod with a non-number type.");
                    return new(Value.ValueType.num, values[0].NumValue % values[1].NumValue);
                case "=": //Non strict equal
                    if (values.Count < 3 || values[0].valueType != Value.ValueType.@string) throw new CodeSyntaxException("You need at least 3 values for the non strict equal operator and the first value must be the comparison-type in a string form.");

                    switch (values[0].StringValue.ToLower())
                    {
                        case "string":

                            string? firstStringValue = values[1].ObjectValue.ToString();
                            values.RemoveRange(0, 2);
                            return new(Value.ValueType.@bool, !values.Any(x => x.ObjectValue.ToString() != firstStringValue));
                        case "bool":
                            try
                            {
                                bool firstBoolValue = values[1].BoolValue;
                                values.RemoveRange(0, 2);
                                return new(Value.ValueType.@bool, !values.Any(x => x.BoolValue != firstBoolValue));
                            }
                            catch (Exception ex)
                            { //If one value couldn't be converted to a bool
                                return new(Value.ValueType.@bool, false);
                            }
                        case "num":
                            try
                            {
                                double firstNumValue = double.Parse(values[1].ObjectValue.ToString() ?? "");
                                values.RemoveRange(0, 2);
                                return new(Value.ValueType.@bool, !values.Any(x => double.Parse(x.ObjectValue.ToString() ?? "") != firstNumValue));
                            }
                            catch (Exception)
                            { //If one value couldn't be converted to a double
                                return new(Value.ValueType.@bool, false);
                            }
                        default:
                            throw new CodeSyntaxException($"The \"{values[0].StringValue}\"-type either doesn't exist, or is not suitable for the non strinct equal operator. Suitable types are:\nstring, bool and num.");
                    }

                case "==": //Type strict equal
                    if (values.Count < 2) throw new CodeSyntaxException("You need at least 2 values for the type strict equal operator.");

                    Value.ValueType firstType = values[0].valueType ?? throw new InternalInterpreterException("Value type of value was null");
                    if (values.Any(x => x.valueType != firstType)) return new(Value.ValueType.@bool, false);
                    object firstValue = values[0].ObjectValue;
                    values.RemoveAt(0);
                    return new(Value.ValueType.@bool, !values.Any(x => !x.ObjectValue.Equals(firstValue)));
                case "<":
                    if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for a less than operator.");
                    if (values.Any(x => !x.IsNumeric)) throw new CodeSyntaxException("You can't use the less than operator with a non-number type.");
                    return new(Value.ValueType.@bool, values[0].NumValue < values[1].NumValue);
                case ">":
                    if (values.Count != 2) throw new CodeSyntaxException("You need 2 values for a greater than operator.");
                    if (values.Any(x => !x.IsNumeric)) throw new CodeSyntaxException("You can't use the greater than operator with a non-number type.");
                    return new(Value.ValueType.@bool, values[0].NumValue > values[1].NumValue);
                default: throw new InternalInterpreterException($"Internal: \"{@operator.ToLower()}\" is not a valid operator and should have thrown an exeption already.");





            }
        }

        public static readonly string[] operators = { "+", "-", "*", "/", "and", "or", "!", "not", "%", "=", "==", "<", ">" };
        public enum Type
        {
            @operator, returnStatement, value, calculation, function
        }

        public Type type;
        public int line;
        public string? @operator;
        public List<Command>? returnStatement;
        public FunctionCall? functionCall;
        public Value? value;
        public List<CalculationType>? subValues;
        public CalculationType(Command command)
        {
            line = command.commandLine;
            switch (command.commandType)
            {
                case Command.CommandTypes.FunctionCall:
                    this.functionCall = command.functionCall;
                    type = Type.function;
                    return;
                case Command.CommandTypes.Statement:
                    int foundAt = -1;
                    if (operators.Any(x => { foundAt++; return x == command.commandText; }))
                    {
                        @operator = operators[foundAt];
                        type = Type.@operator;
                        return;
                    }
                    if (!double.TryParse(command.commandText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double value))
                    {
                        type = Type.returnStatement;
                        returnStatement = new List<Command>() { command };
                        return;
                    }
                    type = Type.value;
                    this.value = new(Value.ValueType.num, value);
                    return;
                case Command.CommandTypes.String:
                    type = Type.value;
                    this.value = new(Value.ValueType.@string, command.commandText);
                    return;
                case Command.CommandTypes.Calculation:

                    if (command.commandText.StartsWith('$'))
                    {
                        type = Type.returnStatement;
                        returnStatement = StringProcess.ConvertLineToCommand(command.commandText.Substring(1, command.commandText.Count() - 1), command.commandLine);
                        return;
                    }
                    else
                    {
                        type = Type.calculation;
                        List<Command> subCalculationTypes = StringProcess.ConvertLineToCommand(command.commandText, command.commandLine);
                        List<CalculationType> subCalculationTypesTokenSplit = new();
                        foreach (Command subCalculationType in subCalculationTypes)
                        {
                            subCalculationTypesTokenSplit.Add(new(subCalculationType));
                        }


                        subValues = subCalculationTypesTokenSplit;

                    }
                    return;
            }





        }





    }

}
