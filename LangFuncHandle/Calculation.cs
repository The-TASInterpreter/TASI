using static TASI.Command;

namespace TASI
{
    public class Calculation
    {
        public static Var DoCalculation(Command command, AccessableObjects accessableObjects)
        {
            if (command.commandType != Command.CommandTypes.Calculation) throw new Exception("Internal: Do calculation only works with num calculation tokens");
            return (command.calculation ?? throw new Exception("Internal: Calculation was not parsed.")).GetValue(accessableObjects);
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
                    foreach(Command command in returnStatement)
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

        public Var GetValue(AccessableObjects accessableObjects)
        {
            switch (type)
            {
                case Type.value: return value ?? throw new Exception("Internal: value is null.");
                case Type.returnStatement: return Statement.ReturnStatement(returnStatement ?? throw new Exception("Internal: return statement list is null."), accessableObjects);
                case Type.@operator: throw new Exception("Internal: Can't get the value of an operator.");
                case Type.function: return (functionCall ?? throw new Exception("Internal: function call is null")).DoFunctionCall(accessableObjects);
                case Type.calculation:
                    List<Var> values = new List<Var>();
                    string currentOperator = "";

                    foreach (CalculationType calculationType in subValues ?? throw new Exception("Internal: sub values is null"))
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
                    if (values.Count == 0) throw new Exception("You can't end up with 0 values at the end of a num calculation (I don't even know how you managed to do this, but it's 100% your fault).");
                    throw new Exception("You can't end up with more than 1 value at the end of a num calculation.");
                default: throw new Exception($"Internal: Unexpected \"{type}\" CalculationType.Type");
            }
        }

        public static Var SimulateOperator(List<Var> values, string @operator)
        {
            switch (@operator.ToLower())
            {
                case "+":
                    if (values.Count != 2) throw new Exception("You need 2 values for an addition operator.");
                    if (values[0].varDef.varType == VarDef.EvarType.@string || values[1].varDef.varType == VarDef.EvarType.@string) return new(new(VarDef.EvarType.@string, ""), true, values[0].ToString() + values[1].ToString());
                    return new(new(VarDef.EvarType.num, ""), true, values[0].numValue + values[1].numValue);
                case "-":
                    if (values.Count == 1)
                    {
                        if (!values[0].isNumeric) throw new Exception($"You can't have a negative non-numeric {values[0].varDef.varType}-type.");
                        return new(new(VarDef.EvarType.num, ""), true, -values[0].numValue);
                    }
                    else if (values.Count != 2) throw new Exception("You need 2 values for a subtraction operator.");
                    if (values.Any(x => !x.isNumeric)) throw new Exception("You can't substract with a non-number type.");
                    return new(new(VarDef.EvarType.num, ""), true, values[0].numValue - values[1].numValue);
                case "*":
                    if (values.Count != 2) throw new Exception("You need 2 values for a multiplication operator.");
                    if (values.Any(x => !x.isNumeric)) throw new Exception("You can't multiply with a non-number type.");
                    return new(new(VarDef.EvarType.num, ""), true, values[0].numValue * values[1].numValue);
                case "/":
                    if (values.Count != 2) throw new Exception("You need 2 values for a division operator.");
                    if (values.Any(x => !x.isNumeric)) throw new Exception("You can't devide with a non-number type.");
                    return new(new(VarDef.EvarType.num, ""), true, values[0].numValue / values[1].numValue);
                case "and":
                    if (values.Count != 2) throw new Exception("You need 2 values for an and operator.");
                    return new(new(VarDef.EvarType.@bool, ""), true, values[0].GetBoolValue && values[1].GetBoolValue);
                case "or":
                    if (values.Count != 2) throw new Exception("You need 2 values for an or operator.");
                    return new(new(VarDef.EvarType.@bool, ""), true, values[0].GetBoolValue || values[1].GetBoolValue);
                case "!" or "not":
                    if (values.Count != 1) throw new Exception("You need 1 value for a not operator.");
                    return new(new(VarDef.EvarType.@bool, ""), true, !values[0].GetBoolValue);
                case "%":
                    if (values.Count != 2) throw new Exception("You need 2 values for a modulus operator.");
                    if (values.Any(x => !x.isNumeric)) throw new Exception("You can't mod with a non-number type.");
                    return new(new(VarDef.EvarType.num, ""), true, values[0].numValue % values[1].numValue);
                case "=": //Non strict equal
                    if (values.Count < 3 || values[0].varDef.varType != VarDef.EvarType.@string) throw new Exception("You need at least 3 values for the non strict equal operator and the first value must be the comparison-type in a string form.");

                    switch (values[0].stringValue.ToLower())
                    {
                        case "string":

                            string? firstStringValue = values[1].ObjectValue.ToString();
                            values.RemoveRange(0, 2);
                            return new(new(VarDef.EvarType.@bool, ""), true, !values.Any(x => x.ObjectValue.ToString() != firstStringValue));
                        case "bool":
                            try
                            {
                                bool firstBoolValue = values[1].GetBoolValue;
                                values.RemoveRange(0, 2);
                                return new(new(VarDef.EvarType.@bool, ""), true, !values.Any(x => x.GetBoolValue != firstBoolValue));
                            }
                            catch (Exception ex)
                            { //If one value couldn't be converted to a bool
                                return new(new(VarDef.EvarType.@bool, ""), true, false);
                            }
                        case "num":
                            try
                            {
                                double firstNumValue = double.Parse(values[1].ObjectValue.ToString());
                                values.RemoveRange(0, 2);
                                return new(new(VarDef.EvarType.@bool, ""), true, !values.Any(x => double.Parse(x.ObjectValue.ToString()) != firstNumValue));
                            }
                            catch (Exception ex)
                            { //If one value couldn't be converted to a double
                                return new(new(VarDef.EvarType.@bool, ""), true, false);
                            }
                        default:
                            throw new Exception($"The \"{values[0].stringValue}\"-type either doesn't exist, or is not suitable for the non strinct equal operator. Suitable types are:\nstring, bool and num.");
                    }

                case "==": //Type strict equal
                    if (values.Count < 2) throw new Exception("You need at least 2 values for the type strict equal operator.");

                    VarDef.EvarType firstType = values[0].varDef.varType;
                    if (values.Any(x => x.varDef.varType != firstType)) return new(new(VarDef.EvarType.@bool, ""), true, false);
                    object firstValue = values[0].ObjectValue;
                    values.RemoveAt(0);
                    return new(new(VarDef.EvarType.@bool, ""), true, !values.Any(x => !x.ObjectValue.Equals(firstValue)));
                case "<":
                    if (values.Count != 2) throw new Exception("You need 2 values for a less than operator.");
                    if (values.Any(x => !x.isNumeric)) throw new Exception("You can't use the less than operator with a non-number type.");
                    return new(new(VarDef.EvarType.@bool, ""), true, values[0].numValue < values[1].numValue);
                case ">":
                    if (values.Count != 2) throw new Exception("You need 2 values for a greater than operator.");
                    if (values.Any(x => !x.isNumeric)) throw new Exception("You can't use the greater than operator with a non-number type.");
                    return new(new(VarDef.EvarType.@bool, ""), true, values[0].numValue > values[1].numValue);
                default: throw new Exception($"Internal: \"{@operator.ToLower()}\" is not a valid operator and should have thrown an exeption already.");





            }
        }

        public static readonly string[] operators = { "+", "-", "*", "/", "and", "or", "!", "not", "%", "=", "==", "<", ">" };
        public enum Type
        {
            @operator, returnStatement, value, calculation, function
        }
        public static char[] number = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };

        public Type type;
        public string? @operator;
        public List<Command>? returnStatement;
        public FunctionCall? functionCall;
        public Var? value;
        public List<CalculationType>? subValues;
        public CalculationType(Command command)
        {
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
                    if (!double.TryParse(command.commandText, out double value)) throw new Exception($"\"{command.commandText}\" is neither an operator nor a number. If you want to use return statements like variables inside calculations, you need the statement calculation. E.g.:\n(($variable) + 15)");
                    type = Type.value;
                    this.value = new(new(VarDef.EvarType.num, ""), true, value);
                    return;
                case Command.CommandTypes.String:
                    type = Type.value;
                    this.value = new(new(VarDef.EvarType.@string, ""), true, command.commandText);
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
                        bool isNumber = false;
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
