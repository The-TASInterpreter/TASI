using System.Collections.Generic;

namespace TASI
{



    internal class Statement
    {
        public static string[] staticStatements = { "set" };

        public static Var StaticStatement(CommandLine commandLine, AccessableObjects accessableObjects)
        {
            Var returnValue = new();
            if (commandLine.commands[0].commandType != Command.CommandTypes.Statement)
                throw new Exception("Internal: StaticStatements must start with a Statement");

            switch (commandLine.commands[0].commandText.ToLower())
            {
                case "return":
                    if (commandLine.commands.Count == 1) return new(new Var());
                    if (commandLine.commands.Count < 2) throw new Exception("Invalid return statement usage; Right usage: return <value>;");
                    return new(GetVarOfCommandLine(new(commandLine.commands.GetRange(1, commandLine.commands.Count - 1), -1), accessableObjects));

                case "set":
                    //Validate syntax
                    StaticStatementSet(commandLine, accessableObjects);
                    return new();
                case "while":
                    CommandLine checkStatement = new(new(), -1);
                    for (int i = 1; i < commandLine.commands.Count; i++)
                    {
                        if (commandLine.commands[i].commandType == Command.CommandTypes.CodeContainer)
                            break;
                        checkStatement.commands.Add(commandLine.commands[i]);
                    }
                    if (commandLine.commands.Count != checkStatement.commands.Count + 2)
                        if (commandLine.commands.Count > checkStatement.commands.Count + 2)
                            throw new Exception("Missing statement (code container)");
                        else
                            throw new Exception($"Unexpected {commandLine.commands[checkStatement.commands.Count + 1].commandType} in while loop.");
                    if (commandLine.commands[checkStatement.commands.Count + 1].commandType != Command.CommandTypes.CodeContainer)
                        throw new Exception("Invalid stuff in while loop I hate writeing these messages pls kill me");
                    List<Command> code = commandLine.commands[checkStatement.commands.Count + 1].codeContainerCommands ?? throw new Exception("Internal: Code container was not converted to a command list.");
                    
                    while (GetVarOfCommandLine(checkStatement, accessableObjects).GetBoolValue)
                    {
                        returnValue = InterpretMain.InterpretNormalMode(code, accessableObjects);
                        if (returnValue.varDef.varType == VarDef.EvarType.@return) return returnValue;
                    }
                    

                    return new();
                case "if":
                    if (commandLine.commands.Count < 3) throw new Exception("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                    if (commandLine.commands[2].commandType != Command.CommandTypes.CodeContainer)
                        throw new Exception("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");

                    if (commandLine.commands.Count == 3)
                    {
                        if (GetVarOfCommandLine(new(new List<Command> { commandLine.commands[1] }, -1), accessableObjects).GetBoolValue)
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commandLine.commands[2].codeContainerCommands ?? throw new Exception("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue.varDef.varType == VarDef.EvarType.@return) return returnValue;
                        }

                    }
                    else if (commandLine.commands.Count == 5)
                    {
                        if (commandLine.commands[3].commandType != Command.CommandTypes.Statement || commandLine.commands[3].commandText != "else")
                            throw new Exception("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                        if (commandLine.commands[4].commandType != Command.CommandTypes.CodeContainer)
                            throw new Exception("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                        if (GetVarOfCommandLine(new(new List<Command> { commandLine.commands[1] }, -1 ), accessableObjects).GetBoolValue)
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commandLine.commands[2].codeContainerCommands ?? throw new Exception("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue.varDef.varType == VarDef.EvarType.@return) return returnValue;
                        }
                        else
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commandLine.commands[4].codeContainerCommands ?? throw new Exception("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue.varDef.varType == VarDef.EvarType.@return) return returnValue;
                        }


                    }
                    return new();
                case "helpm":
                    if (commandLine.commands.Count != 2) throw new Exception("Invalid helpm statement syntax. Example for right syntax:\nhelpm <function call>;");
                    if (commandLine.commands[1].commandType != Command.CommandTypes.FunctionCall) throw new Exception("Invalid helpm statement syntax. Example for right syntax:\nhelpm <function call>;");
                    FunctionCall helpCall = commandLine.commands[1].functionCall ?? throw new Exception("Internal: function call was not converted to a function call.");
                    Help.ListFunctionArguments(helpCall.callFunction);
                    return new();
                case "listm":
                    if (commandLine.commands.Count != 2) throw new Exception("Invalid listm statement syntax. Example for right syntax:\nhelpm <string location>;");
                    if (commandLine.commands[1].commandType != Command.CommandTypes.String) throw new Exception("Invalid listm statement syntax. Example for right syntax:\nhelpm <string location>;");
                    Help.ListLocation(commandLine.commands[1].commandText);
                    return new();
                case "rootm":
                    if (commandLine.commands.Count != 1) throw new Exception("Invalid rootm statement syntax. Example for right syntax:\nhelpm; (It's that simple)");
                    Console.WriteLine("All registered namespaces are:");
                    Help.ListNamespaces(Global.Namespaces);
                    return new();



                default:
                    throw new Exception($"Unknown statement: \"{commandLine.commands[0].commandText}\"");
            }
        }
        public static Var GetVarOfCommandLine(CommandLine commandLine, VarDef.EvarType expectedType, AccessableObjects accessableObjects)
        {

            switch (commandLine.commands[0].commandType)//Check var type thats provided
            {
                case Command.CommandTypes.FunctionCall:
                    FunctionCall functionCall = commandLine.commands[0].functionCall ?? throw new Exception("Internal: function call was not converted to a function call.");
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a function call
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after functioncall.");
                    if (functionCall.callFunction.returnType != expectedType) //Find out if function returns desired type
                        throw new Exception($"The function {functionCall.callFunction.functionLocation} does not return the expected {expectedType} type.");
                    return functionCall.DoFunctionCall(accessableObjects);

                case Command.CommandTypes.Calculation:
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a calculation
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after alculation.");
                    Var numCalcRet = Calculation.DoCalculation(commandLine.commands[0], accessableObjects);
                    if (numCalcRet.varDef.varType != expectedType) throw new Exception($"The calculation does not return the expected {expectedType} type.");
                    return numCalcRet;

                case Command.CommandTypes.Statement:
                    Var returnStatementCall = ReturnStatement(commandLine.commands, accessableObjects);
                    if (returnStatementCall.varDef.varType != expectedType)
                        throw new Exception($"The ReturnStatement \"{commandLine.commands[0].commandText}\" does not return the expected {expectedType} value at all or in the given configuation.");
                    return returnStatementCall;

                case Command.CommandTypes.String:
                    if (expectedType != VarDef.EvarType.@string) throw new Exception($"String is not the expected {expectedType} type.");
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a string
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after calculation.");
                    return new Var(new(VarDef.EvarType.@string, "", false), true, commandLine.commands[0].commandText);

                default:
                    throw new Exception($"Unexpected type ({commandLine.commands[0].commandType})");
            }
        }
        public static Var GetVarOfCommandLine(CommandLine commandLine, AccessableObjects accessableObjects)
        {

            switch (commandLine.commands[0].commandType)//Check var type thats provided
            {
                case Command.CommandTypes.FunctionCall:
                    FunctionCall functionCall = commandLine.commands[0].functionCall ?? throw new Exception("Internal: function call was not converted to a function call.");
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a function call
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after functioncall.");

                    return functionCall.DoFunctionCall(accessableObjects);

                case Command.CommandTypes.Calculation:
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a calculation
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after calculation.");
                    Var numCalcRet = Calculation.DoCalculation(commandLine.commands[0], accessableObjects);

                    return numCalcRet;

                case Command.CommandTypes.Statement:
                    Var returnStatementCall = ReturnStatement(commandLine.commands, accessableObjects);
                    return returnStatementCall;

                case Command.CommandTypes.String:
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a string
                        throw new Exception($"Unexpected {commandLine.commands[1].commandType} after calculation.");
                    return new Var(new(VarDef.EvarType.@string, "", false), true, commandLine.commands[0].commandText);

                default:
                    throw new Exception($"Unexpected type ({commandLine.commands[0].commandType})");
            }
        }


        private static void StaticStatementSet(CommandLine commandLine, AccessableObjects accessableObjects)
        {
            if (commandLine.commands.Count < 3) throw new Exception("Invalid syntax for set command\nExpected: set <variable(Statement)> <value>;");
            if (commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new Exception("Invalid syntax for set command\nExpected: set <variable(Statement)> <value>;");

            Var? correctVar = null;
            commandLine.commands[1].commandText = commandLine.commands[1].commandText.ToLower();
            foreach (Var var in accessableObjects.accessableVars) //Search for variable
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
                case VarDef.EvarType.num or VarDef.EvarType.@bool:
                    correctVar.numValue = GetVarOfCommandLine(new CommandLine(commandLine.commands.GetRange(2, commandLine.commands.Count - 2), commandLine.lineIDX), correctVar.varDef.varType, accessableObjects).numValue;
                    break;
                case VarDef.EvarType.@string:
                    correctVar.stringValue = GetVarOfCommandLine(new CommandLine(commandLine.commands.GetRange(2, commandLine.commands.Count - 2), commandLine.lineIDX), correctVar.varDef.varType, accessableObjects).stringValue;
                    break;
                
                default: throw new Exception("Internal: Unimplemented VarType");
            }
        }
        public static Var ReturnStatement(List<Command> commands, AccessableObjects accessableObjects)
        {
            if (commands[0].commandType != Command.CommandTypes.Statement)
                throw new Exception("Internal: ReturnStatements must start with a Statement");

            switch (commands[0].commandText)
            {
                case "return":
                    if (commands.Count == 1) return new(new Var());
                    if (commands.Count < 2) throw new Exception("Invalid return statement usage; Right usage: return <value>;");
                    return new(GetVarOfCommandLine(new(commands.GetRange(1, commands.Count - 1), -1), accessableObjects));

                case "true":
                    if (commands.Count != 1) throw new Exception($"Unexpected {commands[1].commandType}");
                    return new Var(new(VarDef.EvarType.@bool, ""), true, true);
                case "false":
                    if (commands.Count != 1) throw new Exception($"Unexpected {commands[1].commandType}");
                    return new Var(new(VarDef.EvarType.@bool, ""), true, false);
                case "new":
                    if (commands[1].commandType != Command.CommandTypes.Statement)
                        throw new Exception($"Unexpected {commands[1].commandType} at argument 1 of new statement\nA statement would be expected at this point.");
                    throw new NotImplementedException("Internal: New statement is not fully implemented yet");
                case "void":
                    if (commands.Count != 1) throw new Exception($"Unexpected {commands[1].commandType}");
                    return new();

                case "nl":
                    return new Var(new(VarDef.EvarType.@string, ""), true, "\n");
                case "lineChar":
                    return new Var(new(VarDef.EvarType.@string, ""), true, "Ⅼ");

                case "if":
                    //Check if if statement usage is correct
                    Var? returnVar = null;
                    if (commands.Count != 5 || commands[2].commandType != Command.CommandTypes.CodeContainer || commands[3].commandType != Command.CommandTypes.Statement || commands[3].commandText.ToLower() != "else" || commands[4].commandType != Command.CommandTypes.CodeContainer)
                        throw new Exception("Invalid return-type if statement; Correct usage:\nif <code container> else <code container>");
                    if (GetVarOfCommandLine(new(new List<Command> { commands[1] }, -1), accessableObjects).GetBoolValue)
                        returnVar = InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands ?? throw new Exception("Internal: Code container was not converted to a command list."), accessableObjects);
                    else
                        returnVar = InterpretMain.InterpretNormalMode(commands[4].codeContainerCommands ?? throw new Exception("Internal: Code container was not converted to a command list."), accessableObjects);
                    if (returnVar.varDef.varType == VarDef.EvarType.@return)
                        return returnVar.returnStatementValue ?? throw new Exception("Internal: return-var var is null");
                    else
                        throw new Exception("The return-type if statemtent didn't return anything");
                case "do":
                    if (commands.Count != 2 || commands[1].commandType != Command.CommandTypes.CodeContainer) throw new Exception("Invalid usage of do return-statement. Correct usage:\ndo <code container>");
                    returnVar = InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands ?? throw new Exception("Internal: Code container was not converted to a command list."), accessableObjects);
                    if (returnVar.varDef.varType == VarDef.EvarType.@return)
                        return returnVar.returnStatementValue ?? throw new Exception("Internal: return-var var is null");
                    else
                        throw new Exception("The return-type if statemtent didn't return anything");
                    

                default:
                    // Is probably var
                    
                    if (commands.Count != 1) throw new Exception("Unexpected syntax after varname.");
                    commands[0].commandText = commands[0].commandText.ToLower();
                    foreach (Var var in accessableObjects.accessableVars)
                        if (var.varDef.varName == commands[0].commandText)
                            return var;
                    //Var not found


                    throw new Exception($"Unknown return statement \"{commands[0].commandText}\"");
            }

        }

    }
}
