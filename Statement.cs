using System.Runtime.CompilerServices;

namespace Text_adventure_Script_Interpreter
{



    internal class Statement
    {
        public static string[] staticStatements = { "set" };

        public static void StaticStatement(CommandLine commandLine)
        {
            if (commandLine.commands[0].commandType != Command.CommandTypes.Statement)
                throw new Exception("Internal: StaticStatements must start with a Statement");

            switch (commandLine.commands[0].commandText)
            {
                case "set":
                    //Validate syntax
                    StaticStatementSet(commandLine);
                    break;



            }
        }


        public static Var GetVarOfCommandLine(CommandLine commandLine, VarDef.evarType expectedType)
        {

            switch (commandLine.commands[0].commandType)//Check var type thats provided
            {
                case Command.CommandTypes.UnknownMethod:
                    MethodCall methodCall = new MethodCall(commandLine.commands[0]);
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a method call
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after Methodcall.");
                    if (methodCall.callMethod.returnType != expectedType) //Find out if Method returns desired type
                        throw new Exception($"The method {methodCall.callMethod.methodLocation} does not return the expected {expectedType} type.");
                    return methodCall.DoMethodCall();

                case Command.CommandTypes.NumCalculation:
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a calculation
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after Num calculation.");
                    throw new NotImplementedException("NumberCalculation types have not been implemented yet.");
                case Command.CommandTypes.Statement:
                    Var returnStatementCall = ReturnStatement(commandLine.commands);
                    if (returnStatementCall.varDef.varType != expectedType)
                        throw new Exception($"The ReturnStatement \"{commandLine.commands[0]}\" does not return the expected {expectedType} value at all or in the given configuation.");
                    return returnStatementCall;
                case Command.CommandTypes.String:
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a string
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after Num calculation.");
                    return new Var(new(VarDef.evarType.String, "", false), true, commandLine.commands[0]);

                default:
                    throw new Exception($"Unexpected type ({commandLine.commands[0].commandType})");
            }
        }


        private static void StaticStatementSet(CommandLine commandLine)
        {
            if (commandLine.commands.Count < 3) throw new Exception("Invalid syntax for set command\nExpected: set <variable(Statement)> <value>;");
            if (commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new Exception("Invalid syntax for set command\nExpected: set <variable(Statement)> <value>;");
            if (commandLine.commands[2].commandType == Command.CommandTypes.Statement) throw new Exception("Statement cant be converted to value");

            Var? correctVar = null;
            foreach (Var var in Global.CurrentlyAccessableVars) //Search for variable
            {
                if (var.varDef.varName == commandLine.commands[1].commandText)
                {
                    correctVar = var;
                    break;
                }
            }
            if (correctVar == null) throw new Exception($"The variable {commandLine.commands[1].commandText} cant be found.");

            switch (correctVar.varDef.varType) //Check var type thats needed
            {
                case VarDef.evarType.Num or VarDef.evarType.Bool:
                    correctVar.numValue = GetVarOfCommandLine(new CommandLine(commandLine.commands.GetRange(2, commandLine.commands.Count - 2), commandLine.lineIDX), correctVar.varDef.varType).numValue;
                    break;
                case VarDef.evarType.String:
                    correctVar.stringValue = GetVarOfCommandLine(new CommandLine(commandLine.commands.GetRange(2, commandLine.commands.Count - 2), commandLine.lineIDX), correctVar.varDef.varType).stringValue;
                    break;
                default: throw new Exception("Internal: Unimplemented VarType");
            }
        }
        public static Var ReturnStatement(List<Command> commands)
        {
            if (commands[0].commandType != Command.CommandTypes.Statement)
                throw new Exception("Internal: ReturnStatements must start with a Statement");

            switch (commands[0].commandText)
            {
                case "true":
                    return new Var(new(VarDef.evarType.Bool, ""), true, true);
                case "false":
                    return new Var(new(VarDef.evarType.Bool, ""), true, false);
                case "new":
                    if (commands[1].commandType != Command.CommandTypes.Statement)
                        throw new Exception($"Unexpected {commands[1].commandType} at argument 1 of new statement\nA statement would be expected at this point.");
                    throw new NotImplementedException("Internal: New statement is not fully implemented yet");


                default:
                    // Is probably var
                    foreach (Var var in Global.CurrentlyAccessableVars)
                        if (var.varDef.varName == commands[0].commandText)
                            return var;
                    //Var not found


                    throw new Exception($"Unknown return statement \"{commands[0].commandText}\"");
            }

        }

    }
}
