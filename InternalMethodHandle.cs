namespace TASI
{
    internal class InternalMethodHandle
    {

        public static Var HandleInternalFunc(string funcName, List<Var> input, List<Var> accessableVars)
        {
            switch (funcName)
            {
                case "test.helloworld":
                    if (input[0].numValue == 1)
                        Console.WriteLine(input[1].stringValue);
                    else
                        Console.WriteLine("No text pritable.");
                    return new Var();
                case "console.readline":
                    return new Var(new(VarDef.EvarType.@string, ""), true, Console.ReadLine());
                case "console.clear":
                    Console.Clear();
                    return new();
                case "console.writeline":

                    if (input[0].isNumeric)
                        Console.WriteLine(input[0].numValue);
                    else
                        Console.WriteLine(input[0].stringValue);
                    return new();
                case "program.pause":
                    if (input.Count == 1 && input[0].numValue == 1)
                        Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    return new();
                case "inf.defvar":

                    if (!Enum.TryParse<VarDef.EvarType>(input[0].stringValue, true, out VarDef.EvarType varType) || input[0].stringValue.ToLower() == VarDef.EvarType.@return.ToString()) throw new Exception($"The vartype \"{input[0].stringValue}\" doesn't exist.");

                    accessableVars.Add(new(new(varType, input[1].stringValue), false, null));
                    return new();
                case "convert.tonum":
                    if (!double.TryParse(input[0].stringValue, out double result))
                        if (input[1].GetBoolValue)
                            throw new Exception("Can't convert string in current format to double.");
                        else
                            return new Var(new(VarDef.EvarType.num, ""), true, double.NaN);



                    return new Var(new(VarDef.EvarType.num, ""), true, result);



                default: throw new Exception("Internal: No definition for " + funcName);
            }

        }




    }

    internal class InternalFuncs
    {
        public static void INF(MethodCall methodCall)
        {
            switch (methodCall.callMethod.methodLocation)
            {
                case "INF.DefFunc":
                    if (InterpretMain.FindMethodUsingMethodPath(methodCall.inputVars[0].stringValue) == null)
                        throw new Exception($"Can't define func {methodCall.inputVars[0].stringValue}, because it isn't declared anywhere. E.U 0010\nTry to add something like this:\nmethod {methodCall.inputVars[0].stringValue} {{\n/code here\n}}.");
                    if (!Enum.TryParse<VarDef.EvarType>(methodCall.inputVars[1].stringValue, out VarDef.EvarType result))
                        throw new Exception($"{methodCall.inputVars[1].stringValue} is an invalid variable type. E.U 0011\nValid types are:\nnum\nvoid\nbool\nstring");

                    break;

            }
        }
    }
}
