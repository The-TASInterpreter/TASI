﻿namespace Text_adventure_Script_Interpreter
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
                    return new Var(new(VarDef.evarType.String, ""), true, Console.ReadLine());
                case "Console.Clear":
                    Console.Clear();
                    return new();
                case "Console.WriteLine":
                    Console.WriteLine(input[0].stringValue);
                    return new();


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
                    if (!Enum.TryParse<VarDef.evarType>(methodCall.inputVars[1].stringValue, out VarDef.evarType result))
                        throw new Exception($"{methodCall.inputVars[1].stringValue} is an invalid variable type. E.U 0011\nValid types are:\nnum\nvoid\nbool\nstring");

                    break;

            }
        }
    }
}
