
namespace TASI
{



    internal class Statement
    {
        public static string[] staticStatements = { "set" };

        public static Value? StaticStatement(CommandLine commandLine, AccessableObjects accessableObjects)
        {
            Value? returnValue = new();
            if (commandLine.commands[0].commandType != Command.CommandTypes.Statement)
                throw new InternalInterpreterException("Internal: StaticStatements must start with a Statement");

            switch (commandLine.commands[0].commandText.ToLower())
            {


                case "return":
                    if (commandLine.commands.Count == 1) return new();
                    if (commandLine.commands.Count < 2) throw new CodeSyntaxException("Invalid return statement usage; Right usage: return <value>;");
                    return new(GetValueOfCommandLine(new(commandLine.commands.GetRange(1, commandLine.commands.Count - 1), -1), accessableObjects));

                case "set":
                    //Validate syntax
                    StaticStatementSet(commandLine, accessableObjects);
                    return null;
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
                            throw new CodeSyntaxException("Missing statement (code container)");
                        else
                            throw new CodeSyntaxException($"Unexpected {commandLine.commands[checkStatement.commands.Count + 1].commandType} in while loop.");
                    if (commandLine.commands[checkStatement.commands.Count + 1].commandType != Command.CommandTypes.CodeContainer)
                        throw new CodeSyntaxException("Invalid stuff in while loop I hate writeing these messages pls kill me");
                    List<Command> code = commandLine.commands[checkStatement.commands.Count + 1].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list.");

                    while (GetValueOfCommandLine(checkStatement, accessableObjects).GetBoolValue)
                    {
                        returnValue = InterpretMain.InterpretNormalMode(code, accessableObjects);
                        if (returnValue != null) return returnValue;
                    }


                    return null;
                case "if":
                    if (commandLine.commands.Count < 3) throw new CodeSyntaxException("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                    if (commandLine.commands[2].commandType != Command.CommandTypes.CodeContainer)
                        throw new CodeSyntaxException("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");

                    if (commandLine.commands.Count == 3)
                    {
                        if (GetValueOfCommandLine(new(new List<Command> { commandLine.commands[1] }, -1), accessableObjects).GetBoolValue)
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commandLine.commands[2].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue != null) return returnValue;
                        }

                    }
                    else if (commandLine.commands.Count == 5)
                    {
                        if (commandLine.commands[3].commandType != Command.CommandTypes.Statement || commandLine.commands[3].commandText != "else")
                            throw new CodeSyntaxException("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                        if (commandLine.commands[4].commandType != Command.CommandTypes.CodeContainer)
                            throw new CodeSyntaxException("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                        if (GetValueOfCommandLine(new(new List<Command> { commandLine.commands[1] }, -1), accessableObjects).GetBoolValue)
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commandLine.commands[2].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue != null) return returnValue;
                        }
                        else
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commandLine.commands[4].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue != null) return returnValue;
                        }


                    }
                    return null;
                case "helpm":
                    if (commandLine.commands.Count != 2) throw new CodeSyntaxException("Invalid helpm statement syntax. Example for right syntax:\nhelpm <function call>;");
                    if (commandLine.commands[1].commandType != Command.CommandTypes.FunctionCall) throw new CodeSyntaxException("Invalid helpm statement syntax. Example for right syntax:\nhelpm <function call>;");
                    FunctionCall helpCall = commandLine.commands[1].functionCall ?? throw new InternalInterpreterException("Internal: function call was not converted to a function call.");
                    Help.ListFunctionArguments(helpCall.callFunction);
                    return null;
                case "listm":
                    if (commandLine.commands.Count != 2) throw new CodeSyntaxException("Invalid listm statement syntax. Example for right syntax:\nhelpm <string location>;");
                    if (commandLine.commands[1].commandType != Command.CommandTypes.String) throw new CodeSyntaxException("Invalid listm statement syntax. Example for right syntax:\nhelpm <string location>;");
                    Help.ListLocation(commandLine.commands[1].commandText);
                    return null;
                case "rootm":
                    if (commandLine.commands.Count != 1) throw new CodeSyntaxException("Invalid rootm statement syntax. Example for right syntax:\nhelpm; (It's that simple)");
                    Console.WriteLine("All registered namespaces are:");
                    Help.ListNamespaces(Global.Namespaces);
                    return null;
                case "link":
                    if (commandLine.commands.Count != 3 || commandLine.commands[1].commandType != Command.CommandTypes.Statement || commandLine.commands[2].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid use of link return statement. Correct usage:\nlink <statement: variable> <statement: variable linking to>;");
                    FindVar(commandLine.commands[1].commandText, accessableObjects, true).varValueHolder = FindVar(commandLine.commands[2].commandText, accessableObjects, true).varValueHolder;
                    return null;
                case "unlink":
                    if (commandLine.commands.Count != 2 || commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid use of unlink return statement. Correct usage:\nlink <statement: variable to unlink>");
                    Var foundVar = FindVar(commandLine.commands[1].commandText, accessableObjects, true);
                    foundVar.varValueHolder = new(new(foundVar.varValueHolder.value.valueType, foundVar.varValueHolder.value.ObjectValue));
                    return null;
                case "makevar":

                    if (commandLine.commands.Count != 3 || commandLine.commands[1].commandType != Command.CommandTypes.Statement || commandLine.commands[2].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid usage of makevar. Correct usage:\nmakevar <statement: var type> <statement: var name>;");



                    if (!Enum.TryParse<Value.ValueType>(commandLine.commands[1].commandText, true, out Value.ValueType varType) && commandLine.commands[1].commandText != "all") throw new CodeSyntaxException($"The vartype \"{commandLine.commands[1].commandText}\" doesn't exist.");
                    if (FindVar(commandLine.commands[2].commandText, accessableObjects, false) != null) throw new CodeSyntaxException($"A variable with the name \"{commandLine.commands[2].commandText}\" already exists in this context.");
                    if (commandLine.commands[1].commandText == "all")
                    {
                        accessableObjects.accessableVars.Add(new(new(VarConstruct.VarType.all, commandLine.commands[2].commandText), new(varType)));
                        return null;
                    }
                    accessableObjects.accessableVars.Add(new(new(Value.ConvertValueTypeToVarType(varType), commandLine.commands[2].commandText), new(varType)));
                    return null;





                default:
                    if (commandLine.commands.Count != 1)
                        throw new CodeSyntaxException($"Unknown statement: \"{commandLine.commands[0].commandText}\"");
                    var checkIfExist = accessableObjects.customStatements.FirstOrDefault(x => x.statementType == CustomStatement.StatementType.statement && x.statementName == commandLine.commands[0].commandText, null);
                    if (checkIfExist != null)
                            return checkIfExist.treeElement.SimulateTree(null, accessableObjects, out bool aintGonnUseThat) ?? throw new CodeSyntaxException("A custom return statement gotta return something.");
                        
                    else
                        throw new CodeSyntaxException($"Unknown statement: \"{commandLine.commands[0].commandText}\"");

            }
        }
        public static Value GetValueOfCommandLine(CommandLine commandLine, Value.ValueType expectedType, AccessableObjects accessableObjects)
        {

            switch (commandLine.commands[0].commandType)//Check var type thats provided
            {
                case Command.CommandTypes.FunctionCall:
                    FunctionCall functionCall = commandLine.commands[0].functionCall ?? throw new InternalInterpreterException("Internal: function call was not converted to a function call.");
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a function call
                        throw new CodeSyntaxException($"Unexpected {commandLine.commands[1].commandType} after functioncall.");
                    if (functionCall.callFunction.returnType != Value.ConvertValueTypeToVarType(expectedType)) //Find out if function returns desired type
                        throw new CodeSyntaxException($"The function {functionCall.callFunction.functionLocation} does not return the expected {expectedType} type.");
                    return functionCall.DoFunctionCall(accessableObjects);

                case Command.CommandTypes.Calculation:
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a calculation
                        throw new CodeSyntaxException($"Unexpected {commandLine.commands[1].commandType} after alculation.");
                    Value numCalcRet = Calculation.DoCalculation(commandLine.commands[0], accessableObjects);
                    if (numCalcRet.valueType != expectedType) throw new CodeSyntaxException($"The calculation does not return the expected {expectedType} type.");
                    return numCalcRet;

                case Command.CommandTypes.Statement:
                    Value returnStatementCall = ReturnStatement(commandLine.commands, accessableObjects);
                    if (returnStatementCall.valueType != expectedType)
                        throw new CodeSyntaxException($"The ReturnStatement \"{commandLine.commands[0].commandText}\" does not return the expected {expectedType} value at all or in the given configuation.");
                    return returnStatementCall;

                case Command.CommandTypes.String:
                    if (expectedType != Value.ValueType.@string) throw new CodeSyntaxException($"String is not the expected {expectedType} type.");
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a string
                        throw new CodeSyntaxException($"Unexpected {commandLine.commands[1].commandType} after calculation.");
                    return new(Value.ValueType.@string, commandLine.commands[0].commandText);

                default:
                    throw new CodeSyntaxException($"Unexpected type ({commandLine.commands[0].commandType})");
            }
        }
        public static Value GetValueOfCommandLine(CommandLine commandLine, AccessableObjects accessableObjects)
        {
            Global.currentLine = commandLine.commands[0].commandLine;
            switch (commandLine.commands[0].commandType)//Check var type thats provided
            {
                case Command.CommandTypes.FunctionCall:
                    FunctionCall functionCall = commandLine.commands[0].functionCall ?? throw new InternalInterpreterException("Internal: function call was not converted to a function call.");
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a function call
                        throw new CodeSyntaxException($"Unexpected {commandLine.commands[1].commandType} after functioncall.");

                    return functionCall.DoFunctionCall(accessableObjects);

                case Command.CommandTypes.Calculation:
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a calculation
                        throw new CodeSyntaxException($"Unexpected {commandLine.commands[1].commandType} after calculation.");
                    Value numCalcRet = Calculation.DoCalculation(commandLine.commands[0], accessableObjects);

                    return numCalcRet;

                case Command.CommandTypes.Statement:
                    Value returnStatementCall = ReturnStatement(commandLine.commands, accessableObjects);
                    return returnStatementCall;

                case Command.CommandTypes.String:
                    if (commandLine.commands.Count != 1) //There shouldnt be anything after a string
                        throw new CodeSyntaxException($"Unexpected {commandLine.commands[1].commandType} after calculation.");
                    return new(Value.ValueType.@string, commandLine.commands[0].commandText);

                default:
                    throw new CodeSyntaxException($"Unexpected type ({commandLine.commands[0].commandType})");
            }
        }


        private static void StaticStatementSet(CommandLine commandLine, AccessableObjects accessableObjects)
        {
            if (commandLine.commands.Count < 3) throw new CodeSyntaxException("Invalid syntax for set command\nExpected: set <variable(Statement)> <value>;");
            if (commandLine.commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid syntax for set command\nExpected: set <variable(Statement)> <value>;");

            Var? correctVar = null;
            commandLine.commands[1].commandText = commandLine.commands[1].commandText.ToLower();
            foreach (Var var in accessableObjects.accessableVars) //Search for variable
            {
                if (var.varConstruct.name == commandLine.commands[1].commandText)
                {
                    correctVar = var;
                    break;
                }
            }
            if (correctVar == null) throw new CodeSyntaxException($"The variable {commandLine.commands[1].commandText} cant be found.");
            correctVar.VarValue = GetValueOfCommandLine(new CommandLine(commandLine.commands.GetRange(2, commandLine.commands.Count - 2), commandLine.lineIDX), accessableObjects);
        }
        public static Var? FindVar(string name, AccessableObjects accessableObjects, bool failAtNotFind = false)
        {
            name = name.ToLower();
            foreach (Var var in accessableObjects.accessableVars) //Search for variable
            {
                if (var.varConstruct.name == name)
                {
                    return var;
                }
            }
            if (failAtNotFind)
                throw new CodeSyntaxException($"The variable \"{name}\" wasn't found.");
            else
                return null;

        }
        public static Value ReturnStatement(List<Command> commands, AccessableObjects accessableObjects)
        {
            Value returnValueFromVar;
            if (commands[0].commandType != Command.CommandTypes.Statement)
                throw new InternalInterpreterException("Internal: ReturnStatements must start with a Statement");

            switch (commands[0].commandText)
            {

                case "true":
                    if (commands.Count != 1) throw new CodeSyntaxException($"Unexpected {commands[1].commandType}");

                    return new(Value.ValueType.@bool, true);
                case "false":
                    if (commands.Count != 1) throw new CodeSyntaxException($"Unexpected {commands[1].commandType}");
                    return new(Value.ValueType.@bool, false);
                case "new":
                    if (commands[1].commandType != Command.CommandTypes.Statement)
                        throw new CodeSyntaxException($"Unexpected {commands[1].commandType} at argument 1 of new statement\nA statement would be expected at this point.");
                    throw new NotImplementedException("Internal: New statement is not fully implemented yet");
                case "void":
                    if (commands.Count != 1) throw new CodeSyntaxException($"Unexpected {commands[1].commandType}");
                    return new();

                case "nl":
                    return new(Value.ValueType.@string, "\n");
                case "lineChar":
                    return new(Value.ValueType.@string, "Ⅼ");

                case "if":
                    //Check if if statement usage is correct
                    Value? returnValue = null;
                    if (commands.Count != 5 || commands[2].commandType != Command.CommandTypes.CodeContainer || commands[3].commandType != Command.CommandTypes.Statement || commands[3].commandText.ToLower() != "else" || commands[4].commandType != Command.CommandTypes.CodeContainer)
                        throw new CodeSyntaxException("Invalid return-type if statement; Correct usage:\nif <code container> else <code container>");
                    if (GetValueOfCommandLine(new(new List<Command> { commands[1] }, -1), accessableObjects).GetBoolValue)
                        returnValue = InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                    else
                        returnValue = InterpretMain.InterpretNormalMode(commands[4].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                    if (returnValue != null)
                        return returnValue;
                    else
                        throw new CodeSyntaxException("The return-type if statemtent didn't return anything");
                case "do":
                    if (commands.Count != 2 || commands[1].commandType != Command.CommandTypes.CodeContainer) throw new CodeSyntaxException("Invalid usage of do return-statement. Correct usage:\ndo <code container>");
                    returnValue = InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                    if (returnValue != null)
                        return returnValue;
                    else
                        throw new CodeSyntaxException("The return-type if statemtent didn't return anything");
                case "linkable":
                    if (commands.Count != 2 || commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("");
                    Var foundLinkableVar = FindVar(commands[1].commandText, accessableObjects, true) ?? throw new InternalInterpreterException("Found variable is null");
                    returnValueFromVar = new(foundLinkableVar.VarValue);
                    returnValueFromVar.comesFromVarValue = foundLinkableVar;
                    return returnValueFromVar;
                case "isaprilfools":
                    if (commands.Count != 1) throw new CodeSyntaxException("Incorrect use of isAprilFools return-statement. Correct use:\nisAprilFools");
                    return new(Value.ValueType.@bool, DateTime.Now.Month == 4 && DateTime.Now.Day == 1 && new Random().Next(0, 20) == 1);








                default:
                    // Is probably var

                    if (commands.Count != 1) throw new CodeSyntaxException($"Unexpected syntax after varname \"{commands[0].commandText}\".");
                    if (double.TryParse(commands[0].commandText, out double result))
                    {
                        return new(Value.ValueType.num, result);
                    }


                    var checkIfExist = accessableObjects.customStatements.FirstOrDefault(x => x.statementType == CustomStatement.StatementType.returnstatement && x.statementName == commands[0].commandText, null);
                    if (checkIfExist != null)
                        if (checkIfExist.treeElement.provide == null)
                            return checkIfExist.treeElement.SimulateTree(null, accessableObjects, out bool aintGonnUseThat) ?? throw new CodeSyntaxException("A custom return statement gotta return something.");

                        else
                            return checkIfExist.treeElement.SimulateTree(Statement.GetValueOfCommandLine(checkIfExist.treeElement.provide, accessableObjects), accessableObjects, out bool aintGonnUseThat) ?? throw new CodeSyntaxException( "A custom return statement gotta return something.");


                    commands[0].commandText = commands[0].commandText.ToLower();
                    foreach (Var var in accessableObjects.accessableVars)
                        if (var.varConstruct.name == commands[0].commandText)
                        {

                            return new(var.VarValue);
                        }

                    //Var not found


                    throw new CodeSyntaxException($"Unknown return statement \"{commands[0].commandText}\"");
            }

        }

    }
}
