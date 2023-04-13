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
                    return new(Value.ValueType.@string, Console.ReadLine());
                case "console.clear":
                    Console.Clear();
                    return null;
                case "console.writeline":

                    if (input[0].IsNumeric)
                        Console.WriteLine(input[0].numValue);
                    else
                        Console.WriteLine(input[0].stringValue);
                    return null;
                case "program.pause":
                    if (input.Count == 1 && input[0].numValue == 1)
                        Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    return null;
                case "inf.defvar":

                    if (!Enum.TryParse<Value.ValueType>(input[0].stringValue, true, out Value.ValueType varType)) throw new Exception($"The vartype \"{input[0].stringValue}\" doesn't exist.");

                    accessableObjects.accessableVars.Add(new(new(varType, input[1].stringValue), false, null));
                    return null;
                case "convert.tonum":
                    if (!double.TryParse(input[0].stringValue, out double result))
                        if (input[1].GetBoolValue)
                            throw new Exception("Can't convert string in current format to double.");
                        else
                            return new Var(new(VarConstruct.EvarType.num, ""), true, double.NaN);



                    return new Var(new(VarConstruct.EvarType.num, ""), true, result);



                default: throw new Exception("Internal: No definition for " + funcName);
            }

        }




    }

    }
}
