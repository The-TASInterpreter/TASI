namespace TASI
{
    internal class InternalFunctionHandle
    {

        public static Value? HandleInternalFunc(string funcName, List<Value> input, AccessableObjects accessableObjects)
        {
            switch (funcName)
            {
                case "test.helloworld":
                    if (input[0].NumValue == 1)
                        Console.WriteLine(input[1].StringValue);
                    else
                        Console.WriteLine("No text pritable.");
                    return null;
                case "console.readline":
                    return new(Value.ValueType.@string, Console.ReadLine() ?? throw new RuntimeCodeExecutionFailException("Console.ReadLine Returned null.", "InternalMethodException"));
                case "console.clear":
                    Console.Clear();
                    return null;
                case "console.writeline":

                    if (input[0].IsNumeric)
                        Console.WriteLine(input[0].NumValue);
                    else
                        Console.WriteLine(input[0].StringValue);
                    return null;
                case "program.pause":
                    if (input.Count == 1 && input[0].numValue == 1)
                        Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    return null;
                case "inf.defvar":

                    if (!Enum.TryParse<Value.ValueType>(input[0].StringValue, true, out Value.ValueType varType) && input[0].StringValue != "all") throw new CodeSyntaxException($"The vartype \"{input[0].StringValue}\" doesn't exist.");
                    if (input[0].StringValue == "all")
                    {
                        accessableObjects.accessableVars.Add(new(new(VarConstruct.VarType.all, input[1].StringValue), new(varType)));
                        return null;
                    }
                    accessableObjects.accessableVars.Add(new(new(Value.ConvertValueTypeToVarType(varType), input[1].StringValue), new(varType)));
                    return null;
                case "convert.tonum":
                    if (!double.TryParse(input[0].StringValue, out double result))
                        if (input[1].GetBoolValue)
                            throw new CodeSyntaxException("Can't convert string in current format to double.");
                        else
                            return new(Value.ValueType.num, double.NaN);
                    return new(Value.ValueType.num, result);

                case "story.askquestion":
                    Console.WriteLine(input[0].StringValue);
                    return new(Value.ValueType.@string, Console.ReadLine() ?? throw new RuntimeCodeExecutionFailException("Console.ReadLine did return null.", "InternalMethodException"));
                case "program.exit":
                    Environment.Exit(0);
                    return null;
                case "string.getletter":
                    try
                    {
                        return new(Value.ValueType.@string, input[0].StringValue[Convert.ToInt32(input[1].NumValue)]);
                    }
                    catch (Exception)
                    {
                        throw new RuntimeCodeExecutionFailException("Invalid index of string.getletter", "InternalMethodException");
                    }
                case "string.tolower":
                    return new(Value.ValueType.@string, input[0].StringValue.ToLower());






                default: throw new InternalInterpreterException("Internal: No definition for " + funcName);
            }

        }




    }

}

