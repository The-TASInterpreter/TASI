namespace TASI
{
    internal class FuncHandle
    {

        public static Var HandleInternalFunc(string funcName, List<Var> input)
        {
            switch (funcName)
            {
                case "Test.HelloWorld":
                    if (input[0].numValue == 1)
                        Console.WriteLine(input[1].stringValue);
                    else
                        Console.WriteLine("No text pritable.");
                    return new Var();
                case "Console.ReadLine":
                    return new Var(new(VarDef.EvarType.String, ""), true, Console.ReadLine());
                case "Console.Clear":
                    Console.Clear();
                    return new();
                case "Console.WriteLine":

                    if (input[0].isNumeric)
                        Console.WriteLine(input[0].numValue);
                    else
                        Console.WriteLine(input[0].stringValue);
                    return new();
                case "Programm.Pause":
                    if (input.Count == 1 && input[0].numValue == 1)
                        Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    return new();
                case "Inf.DefVar":


                    Global.CurrentlyAccessableVars.Add(new(new(Enum.Parse<VarDef.EvarType>(input[0].stringValue), input[1].stringValue), false, null));
                    return new();
                case "Convert.ToNum":
                    if (!double.TryParse(input[0].stringValue, out double result))
                        if (input[1].GetBoolValue)
                            throw new Exception("Can't convert string in current format to double.");
                        else
                            return new Var(new(VarDef.EvarType.Num, ""), true, double.NaN);



                    return new Var(new(VarDef.EvarType.Num, ""), true, result);



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
