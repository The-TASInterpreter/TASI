
using System.Xml.Linq;

namespace TASI
{



    internal class Statement
    {
        public static string[] staticStatements = { "set" };

        public static Value? StaticStatement(List<Command> commands, AccessableObjects accessableObjects)
        {
            Value? returnValue;
            if (commands[0].commandType != Command.CommandTypes.Statement)
                throw new InternalInterpreterException("Internal: StaticStatements must start with a Statement");

            switch (commands[0].commandText.ToLower())
            {
                case "loop":
                    if (!accessableObjects.inLoop) throw new CodeSyntaxException("You can't use a loop statement outside a loop.");
                    if (commands.Count != 1) throw new CodeSyntaxException("Invalid use of loop statement. Correct use: loop;");
                    return new(Value.SpecialReturns.loop);

                case "return":
                    if (commands.Count == 1) return new();
                    if (commands.Count < 2) throw new CodeSyntaxException("Invalid return statement usage; Right usage: return <value>;");
                    return new(GetValueOfCommands(commands.GetRange(1, commands.Count - 1), accessableObjects));

                case "set":
                    //Validate syntax
                    if (commands.Count < 3) throw new CodeSyntaxException("Invalid syntax for set command\nExpected: set <variable(Statement)> <value>;");
                    if (commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid syntax for set command\nExpected: set <variable(Statement)> <value>;");

                    Var? correctVar = FindVar(commands[1].commandText, accessableObjects, true);
                    if (correctVar.varConstruct.isConstant) throw new CodeSyntaxException($"The value of the constant \"{commands[1].commandText}\" cannot be modified!");
                    correctVar.VarValue = GetValueOfCommands(commands.GetRange(2, commands.Count - 2), accessableObjects);
                    return null;
                case "setlist":
                    if (commands.Count < 4) throw new CodeSyntaxException("invalid use of setlist statement. Correct use: setlist <statement: name> <num/s: index> <value: value>");
                    Value foundValue = GetValueOfListUsingIndex(commands.GetRange(2, commands.Count - 3), (Var)(accessableObjects.accessableVars[commands[1].commandText.ToLower()] ?? throw new CodeSyntaxException($"The list \"{commands[0].commandText}\" couldn't be found")), accessableObjects);
                    Value commandValue = GetValueOfCommands(new(new List<Command> { commands.Last() }), accessableObjects);
                    if (foundValue.comesFromVarValue != null && commandValue.comesFromVarValue == null) //Is linked value so update both
                    {
                        foundValue.comesFromVarValue.VarValue = commandValue;
                    }
                    foundValue.ObjectValue = commandValue.ObjectValue;
                    return null;
                case "add":
                    if (commands.Count < 3) throw new CodeSyntaxException("invalid use of add statement. Correct use: add <statement: name> (optional<num/s: index for nested list>) <value: value>;");
                    foundValue = GetValueOfListUsingIndex(commands.GetRange(2, commands.Count - 3), (Var)(accessableObjects.accessableVars[commands[1].commandText.ToLower()] ?? throw new CodeSyntaxException($"The list \"{commands[1].commandText}\" couldn't be found")), accessableObjects);
                    commandValue = GetValueOfCommands(new(new List<Command> { commands.Last() }), accessableObjects);
                    if (foundValue.comesFromVarValue != null && commandValue.comesFromVarValue == null) //Is linked value so update both
                    {
                        foundValue.comesFromVarValue.VarValue.ListValue.Add(commandValue);
                    }
                    foundValue.ListValue.Add(commandValue);
                    return null;



                case "while":
                    List<Command> checkStatement = new();
                    for (int i = 1; i < commands.Count; i++)
                    {
                        if (commands[i].commandType == Command.CommandTypes.CodeContainer)
                            break;
                        checkStatement.Add(commands[i]);
                    }
                    if (commands.Count != checkStatement.Count + 2)
                        if (commands.Count > checkStatement.Count + 2)
                            throw new CodeSyntaxException("Missing statement (code container)");
                        else
                            throw new CodeSyntaxException($"Unexpected {commands[checkStatement.Count + 1].commandType} in while loop.");
                    if (commands[checkStatement.Count + 1].commandType != Command.CommandTypes.CodeContainer)
                        throw new CodeSyntaxException("Invalid stuff in while loop I hate writeing these messages pls kill me");
                    List<Command> code = commands[checkStatement.Count + 1].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list.");
                    accessableObjects.inLoop = true;
                    while (GetValueOfCommands(checkStatement, accessableObjects).BoolValue)
                    {
                        returnValue = InterpretMain.InterpretNormalMode(code, accessableObjects);
                        if (returnValue != null && returnValue.specialReturn == null)
                        {
                            accessableObjects.inLoop = false;
                            return returnValue;
                        }
                    }
                    accessableObjects.inLoop = false;

                    return null;
                case "if":
                    if (commands.Count < 3) throw new CodeSyntaxException("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                    if (commands[2].commandType != Command.CommandTypes.CodeContainer)
                        throw new CodeSyntaxException("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");

                    if (commands.Count == 3)
                    {
                        if (GetValueOfCommands(new List<Command> { commands[1] }, accessableObjects).BoolValue)
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue != null) return returnValue;
                        }

                    }
                    else if (commands.Count == 5)
                    {
                        if (commands[3].commandType != Command.CommandTypes.Statement || commands[3].commandText != "else")
                            throw new CodeSyntaxException("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                        if (commands[4].commandType != Command.CommandTypes.CodeContainer)
                            throw new CodeSyntaxException("Invalid if statement syntax. Example for right syntax:\nif <bool> <code container>;\nor:\nif <bool> <code container> else <code container>;");
                        if (GetValueOfCommands(new List<Command> { commands[1] }, accessableObjects).BoolValue)
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue != null) return returnValue;
                        }
                        else
                        {
                            returnValue = InterpretMain.InterpretNormalMode(commands[4].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                            if (returnValue != null) return returnValue;
                        }


                    }
                    return null;
                case "helpm":
                    if (commands.Count != 2) throw new CodeSyntaxException("Invalid helpm statement syntax. Example for right syntax:\nhelpm <function call>;");
                    if (commands[1].commandType != Command.CommandTypes.FunctionCall) throw new CodeSyntaxException("Invalid helpm statement syntax. Example for right syntax:\nhelpm <function call>;");
                    FunctionCall helpCall = commands[1].functionCall ?? throw new InternalInterpreterException("Internal: function call was not converted to a function call.");
                    Help.ListFunctionArguments(helpCall.CallFunction);
                    return null;
                case "listm":
                    if (commands.Count != 2) throw new CodeSyntaxException("Invalid listm statement syntax. Example for right syntax:\nhelpm <string location>;");
                    if (commands[1].commandType != Command.CommandTypes.String) throw new CodeSyntaxException("Invalid listm statement syntax. Example for right syntax:\nhelpm <string location>;");
                    Help.ListLocation(commands[1].commandText, accessableObjects.global);
                    return null;
                case "rootm":
                    if (commands.Count != 1) throw new CodeSyntaxException("Invalid rootm statement syntax. Example for right syntax:\nhelpm; (It's that simple)");
                    Console.WriteLine("All registered namespaces are:");
                    Help.ListNamespaces(accessableObjects.global.Namespaces);
                    return null;
                case "link":
                    if (commands.Count != 3 || commands[1].commandType != Command.CommandTypes.Statement || commands[2].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid use of link return statement. Correct usage:\nlink <statement: variable> <statement: variable linking to>;");
                    FindVar(commands[1].commandText, accessableObjects, true).varValueHolder = FindVar(commands[2].commandText, accessableObjects, true).varValueHolder;
                    return null;
                case "unlink":
                    if (commands.Count != 2 || commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid use of unlink return statement. Correct usage:\nunlink <statement: variable to unlink>");
                    Var foundVar = FindVar(commands[1].commandText, accessableObjects, true);
                    if (foundVar.promised != null)
                    { //After unlink value will be reset, so promise can be aborted
                        foundVar.promiseCancel.Cancel();
                        foundVar.WaitPromise();
                        foundVar.promiseCancel = null;
                        foundVar.promised = null;
                    }
                    foundVar.varValueHolder = new(new(foundVar.varValueHolder.value.valueType ?? throw new InternalInterpreterException("Value type of value was null"), foundVar.varValueHolder.value.ObjectValue));
                    return null;
                case "promise": //promise <var> <init> <code>;
                    if (commands.Count != 3 && commands.Count != 4) throw new CodeSyntaxException("Invalid use of promise stratement. Valid use: promise <statement: var name> <code container: init> <code container: execute code>;\nOr\npromise <statement: var name> <code container: execute code>;");

                    foundVar = FindVar(commands[1].commandText, accessableObjects, true);
                    AccessableObjects newPromise = new AccessableObjects(new(), accessableObjects.currentNamespace, accessableObjects.global.CreateNewContext(accessableObjects.global.CurrentFile));
                    foreach(Var var in accessableObjects.accessableVars.Values)
                    {
                        newPromise.accessableVars.Add(var.varConstruct.name, new Var(var, true));
                    }

                    if (commands.Count == 4)
                        InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands, newPromise);
                    foundVar.Promise(commands.Last(), newPromise);
                    return null;

                case "unpromise":
                    if (commands.Count != 2 || commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid use of unpromise return statement. Correct usage:\nunpromise <statement: variable to abort promise>");
                    foundVar = FindVar(commands[1].commandText, accessableObjects, true);
                    if (foundVar.promised != null)
                    {
                        foundVar.CancelPromise();
                    }
                    return null;
                case "makevar":

                    if (commands.Count != 3 || commands[1].commandType != Command.CommandTypes.Statement || commands[2].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid usage of makevar. Correct usage:\nmakevar <statement: var type> <statement: var name>;");



                    if (!Enum.TryParse<Value.ValueType>(commands[1].commandText, true, out Value.ValueType varType) && commands[1].commandText != "all") throw new CodeSyntaxException($"The vartype \"{commands[1].commandText}\" doesn't exist.");
                    if (FindVar(commands[2].commandText, accessableObjects, false) != null) throw new CodeSyntaxException($"A variable with the name \"{commands[2].commandText}\" already exists in this context.");
                    if (commands[1].commandText == "all")
                    {
                        accessableObjects.accessableVars.Add(commands[2].commandText, new Var(new VarConstruct(VarConstruct.VarType.all, commands[2].commandText), new(varType)));
                        return null;
                    }
                    accessableObjects.accessableVars.Add(commands[2].commandText.ToLower(), new Var(new VarConstruct(Value.ConvertValueTypeToVarType(varType), commands[2].commandText), new(varType)));
                    return null;
                case "makeconst":
                    {
                        if (commands.Count != 4 || commands[1].commandType != Command.CommandTypes.Statement || commands[2].commandType != Command.CommandTypes.Statement)
                            throw new CodeSyntaxException("Invalid usage of makeconst. Correct usage:\nmakeconst < statement: var type > < statement: var name > < statement: value >;");

                        if (!Enum.TryParse<Value.ValueType>(commands[1].commandText, true, out Value.ValueType constVarType) && commands[1].commandText != "all") throw new CodeSyntaxException($"The vartype \"{commands[1].commandText}\" doesn't exist.");
                        if (FindVar(commands[2].commandText, accessableObjects, false) != null) throw new CodeSyntaxException($"A variable with the name \"{commands[2].commandText}\" already exists in this context.");
                        if (commands[1].commandText == "all")
                        {
                            accessableObjects.accessableVars.Add(commands[2].commandText, new Var(new VarConstruct(VarConstruct.VarType.all, commands[2].commandText), new(constVarType)));
                            return null;
                        }
                        Var newConstVar = new Var(new VarConstruct(Value.ConvertValueTypeToVarType(constVarType), commands[2].commandText, isConst: true), new(constVarType));
                        newConstVar.VarValue = new(constVarType, commands[3].commandText);

                        accessableObjects.accessableVars.Add(commands[2].commandText, newConstVar);

                        return null;
                    }
                default:
                    throw new CodeSyntaxException($"Unknown statement: \"{commands[0].commandText}\"");
            }
        }
        public static Value GetValueOfCommands(List<Command> commands, Value.ValueType expectedType, AccessableObjects accessableObjects)
        {

            switch (commands[0].commandType)//Check var type thats provided
            {
                case Command.CommandTypes.FunctionCall:
                    FunctionCall functionCall = commands[0].functionCall ?? throw new InternalInterpreterException("Internal: function call was not converted to a function call.");
                    if (commands.Count != 1) //There shouldnt be anything after a function call
                        throw new CodeSyntaxException($"Unexpected {commands[1].commandType} after functioncall.");
                    if (functionCall.CallFunction.returnType != Value.ConvertValueTypeToVarType(expectedType)) //Find out if function returns desired type
                        throw new CodeSyntaxException($"The function {functionCall.CallFunction.functionLocation} does not return the expected {expectedType} type.");
                    return functionCall.DoFunctionCall(accessableObjects);

                case Command.CommandTypes.Calculation:
                    if (commands.Count != 1) //There shouldnt be anything after a calculation
                        throw new CodeSyntaxException($"Unexpected {commands[1].commandType} after alculation.");
                    Value numCalcRet = Calculation.DoCalculation(commands[0], accessableObjects);
                    if (numCalcRet.valueType != expectedType) throw new CodeSyntaxException($"The calculation does not return the expected {expectedType} type.");
                    return numCalcRet;

                case Command.CommandTypes.Statement:
                    Value returnStatementCall = ReturnStatement(commands, accessableObjects);
                    if (returnStatementCall.valueType != expectedType)
                        throw new CodeSyntaxException($"The ReturnStatement \"{commands[0].commandText}\" does not return the expected {expectedType} value at all or in the given configuation.");
                    return returnStatementCall;

                case Command.CommandTypes.String:
                    if (expectedType != Value.ValueType.@string) throw new CodeSyntaxException($"String is not the expected {expectedType} type.");
                    if (commands.Count != 1) //There shouldnt be anything after a string
                        throw new CodeSyntaxException($"Unexpected {commands[1].commandType} after calculation.");
                    return new(Value.ValueType.@string, commands[0].commandText);

                default:
                    throw new CodeSyntaxException($"Unexpected type ({commands[0].commandType})");
            }
        }
        public static Value GetValueOfCommands(List<Command> commands, AccessableObjects accessableObjects)
        {

            switch (commands[0].commandType)//Check var type thats provided
            {
                case Command.CommandTypes.FunctionCall:
                    FunctionCall functionCall = commands[0].functionCall ?? throw new InternalInterpreterException("Internal: function call was not converted to a function call.");
                    if (commands.Count != 1) //There shouldnt be anything after a function call
                        throw new CodeSyntaxException($"Unexpected {commands[1].commandType} after functioncall.");

                    return functionCall.DoFunctionCall(accessableObjects);

                case Command.CommandTypes.Calculation:
                    if (commands.Count != 1) //There shouldnt be anything after a calculation
                        throw new CodeSyntaxException($"Unexpected {commands[1].commandType} after calculation.");
                    Value numCalcRet = Calculation.DoCalculation(commands[0], accessableObjects);

                    return numCalcRet;

                case Command.CommandTypes.Statement:
                    Value returnStatementCall = ReturnStatement(commands, accessableObjects);
                    return returnStatementCall;

                case Command.CommandTypes.String:
                    if (commands.Count != 1) //There shouldnt be anything after a string
                        throw new CodeSyntaxException($"Unexpected {commands[1].commandType} after calculation.");
                    return new(Value.ValueType.@string, commands[0].commandText);

                default:
                    throw new CodeSyntaxException($"Unexpected type ({commands[0].commandType})");
            }
        }


        private static void StaticStatementSet(CommandLine commandLine, AccessableObjects accessableObjects)
        {

        }
        public static Var? FindVar(string name, AccessableObjects accessableObjects, bool failAtNotFind = false)
        {
            Var? foundVar = (Var?)accessableObjects.accessableVars[name.ToLower()];
            if (foundVar != null)
                return foundVar;
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

            switch (commands[0].commandText.ToLower())
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
                    if (GetValueOfCommands(new List<Command> { commands[1] }, accessableObjects).BoolValue)
                        returnValue = InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                    else
                        returnValue = InterpretMain.InterpretNormalMode(commands[4].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
                    if (returnValue != null)
                        return returnValue;
                    else
                        throw new CodeSyntaxException("The return-type if statemtent didn't return anything");
                case "do":
                    if (commands.Count != 2 || commands[1].commandType != Command.CommandTypes.CodeContainer) throw new CodeSyntaxException("Invalid usage of do return-statement. Correct usage:\ndo <code container>");
                    returnValue = InterpretMain.InterpretNormalMode(commands[1].codeContainerCommands ?? throw new InternalInterpreterException("Internal: Code container was not converted to a command list."), accessableObjects);
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










                default:
                    // Is probably var

                    if (commands.Count != 1)
                    {
                        Value foundValue = GetValueOfListUsingIndex(commands.GetRange(1, commands.Count - 1), (Var)(accessableObjects.accessableVars[commands[0].commandText.ToLower()] ?? throw new CodeSyntaxException($"Unknown return statement \"{commands[0].commandText}\"")), accessableObjects);
                        if (foundValue.comesFromVarValue != null)
                            return foundValue.comesFromVarValue.VarValue;
                        return foundValue;
                    }
                    

                    if (int.TryParse(commands[0].commandText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out int intResult))
                    {
                        return new(Value.ValueType.@int, intResult);
                    }

                    if (double.TryParse(commands[0].commandText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double doubleResult))
                    {
                        return new(Value.ValueType.num, doubleResult);
                    }
                    commands[0].commandText = commands[0].commandText.ToLower();
                    return ((Var?)accessableObjects.accessableVars[commands[0].commandText] ?? throw new CodeSyntaxException($"Unknown return statement \"{commands[0].commandText}\"")).varValueHolder.value;


                    //Var not found



            }



        }
        public static Value GetValueOfListUsingIndex(List<Command> indexes, Var listVar, AccessableObjects accessableObjects)
        {
            if (listVar == null || listVar.VarValue.valueType != Value.ValueType.list) throw new CodeSyntaxException($"Unknown or non-list variable \"{indexes[0].commandText}\"");
            Value lastValue = listVar.VarValue;
            for (int i = 0; i < indexes.Count; i++)
            {
                if (lastValue.valueType != Value.ValueType.list) throw new CodeSyntaxException("You can only use list fetch return statements with a list-type variable");
                int index = (int)GetValueOfCommands(new(new List<Command> { indexes[i] }), Value.ValueType.@int, accessableObjects).NumValue;
                List<Value> listValue = lastValue.ListValue;
                if (index < 0 || index >= listValue.Count) throw new CodeSyntaxException("Index out of bounds");
                lastValue = listValue[index];
            }
            return lastValue;

        }
    }
}
