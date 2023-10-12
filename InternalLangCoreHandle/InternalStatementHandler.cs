
using System.Xml.Linq;
using TASI.LangCoreHandleInterface;
using TASI.RuntimeObjects;
using TASI.RuntimeObjects.VarClasses;

namespace TASI.InternalLangCoreHandle
{




    internal class InternalStatementHandler : IStatementHandler
    {

        public Value? HandleStatement(List<Command> commands, AccessableObjects accessableObjects)
        {
            Value? returnValue;
            if (commands[0].commandType != Command.CommandTypes.Statement)
                throw new InternalInterpreterException("Internal: StaticStatements must start with a Statement");

            switch (commands[0].commandText.ToLower())
            {
                case "loop":
                    if (!accessableObjects.inLoop) throw new CodeSyntaxException("You can't use a loop statement outside a loop.");
                    return new(Value.SpecialReturns.loop);

                case "return":
                    if (commands.Count == 1) return new();
                    return new(InterpretationHelp.GetValueOfCommands(commands.GetRange(1, commands.Count - 1), accessableObjects));

                case "set":

                    Var? correctVar = InterpretationHelp.FindVar(commands[1].commandText, accessableObjects, true);
                    if (correctVar.varConstruct.isConstant) throw new CodeSyntaxException($"The value of the constant \"{commands[1].commandText}\" cannot be modified!");
                    correctVar.VarValue = InterpretationHelp.GetValueOfCommands(commands.GetRange(2, commands.Count - 2), accessableObjects);
                    return null;
                case "setlist":
                    if (commands.Count < 4) throw new CodeSyntaxException("invalid use of setlist statement. Correct use: setlist <statement: name> <num/s: index> <value: value>");
                    Value foundValue = InterpretationHelp.GetValueOfListUsingIndex(commands.GetRange(2, commands.Count - 3), (Var)(accessableObjects.accessableVars[commands[1].commandText.ToLower()] ?? throw new CodeSyntaxException($"The list \"{commands[0].commandText}\" couldn't be found")), accessableObjects);
                    Value commandValue = InterpretationHelp.GetValueOfCommands(new(new List<Command> { commands.Last() }), accessableObjects);
                    if (foundValue.comesFromVarValue != null && commandValue.comesFromVarValue == null) //Is linked value so update both
                    {
                        foundValue.comesFromVarValue.VarValue = commandValue;
                    }
                    foundValue.ObjectValue = commandValue.ObjectValue;
                    return null;
                case "add":
                    if (commands.Count < 3) throw new CodeSyntaxException("invalid use of add statement. Correct use: add <statement: name> (optional<num/s: index for nested list>) <value: value>;");
                    foundValue = InterpretationHelp.GetValueOfListUsingIndex(commands.GetRange(2, commands.Count - 3), (Var)(accessableObjects.accessableVars[commands[1].commandText.ToLower()] ?? throw new CodeSyntaxException($"The list \"{commands[1].commandText}\" couldn't be found")), accessableObjects);
                    commandValue = InterpretationHelp.GetValueOfCommands(new(new List<Command> { commands.Last() }), accessableObjects);
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
                    while (InterpretationHelp.GetValueOfCommands(checkStatement, accessableObjects).BoolValue)
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
                        if (InterpretationHelp.GetValueOfCommands(new List<Command> { commands[1] }, accessableObjects).BoolValue)
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
                        
                        if (InterpretationHelp.GetValueOfCommands(new List<Command> { commands[1] }, accessableObjects).BoolValue)
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
                    InterpretationHelp.FindVar(commands[1].commandText, accessableObjects, true).varValueHolder = InterpretationHelp.FindVar(commands[2].commandText, accessableObjects, true).varValueHolder;
                    return null;
                case "unlink":
                    if (commands.Count != 2 || commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid use of unlink return statement. Correct usage:\nunlink <statement: variable to unlink>");
                    Var foundVar = InterpretationHelp.FindVar(commands[1].commandText, accessableObjects, true);
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

                    foundVar = InterpretationHelp.FindVar(commands[1].commandText, accessableObjects, true);
                    AccessableObjects newPromise = new AccessableObjects(new(), accessableObjects.currentNamespace, accessableObjects.global.CreateNewContext(accessableObjects.global.CurrentFile));
                    foreach (Var var in accessableObjects.accessableVars.Values)
                    {
                        newPromise.accessableVars.Add(var.varConstruct.name, new Var(var, true));
                    }

                    if (commands.Count == 4)
                        InterpretMain.InterpretNormalMode(commands[2].codeContainerCommands, newPromise);
                    foundVar.Promise(commands.Last(), newPromise);
                    return null;

                case "unpromise":
                    if (commands.Count != 2 || commands[1].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid use of unpromise return statement. Correct usage:\nunpromise <statement: variable to abort promise>");
                    foundVar = InterpretationHelp.FindVar(commands[1].commandText, accessableObjects, true);
                    if (foundVar.promised != null)
                    {
                        foundVar.CancelPromise();
                    }
                    return null;
                case "makevar":

                    if (commands.Count != 3 || commands[1].commandType != Command.CommandTypes.Statement || commands[2].commandType != Command.CommandTypes.Statement) throw new CodeSyntaxException("Invalid usage of makevar. Correct usage:\nmakevar <statement: var type> <statement: var name>;");



                    if (!Enum.TryParse(commands[1].commandText, true, out Value.ValueType varType) && commands[1].commandText != "all") throw new CodeSyntaxException($"The vartype \"{commands[1].commandText}\" doesn't exist.");
                    if (InterpretationHelp.FindVar(commands[2].commandText, accessableObjects, false) != null) throw new CodeSyntaxException($"A variable with the name \"{commands[2].commandText}\" already exists in this context.");
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

                        if (!Enum.TryParse(commands[1].commandText, true, out Value.ValueType constVarType) && commands[1].commandText != "all") throw new CodeSyntaxException($"The vartype \"{commands[1].commandText}\" doesn't exist.");
                        if (InterpretationHelp.FindVar(commands[2].commandText, accessableObjects, false) != null) throw new CodeSyntaxException($"A variable with the name \"{commands[2].commandText}\" already exists in this context.");
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
                    throw new InternalInterpreterException("Invalid statement for this statement handler");
            }
        }
    }
    
        
    public class InternalReturnStatementHandler : IReturnStatementHandler
    {
        public  Value HandleReturnStatement(List<Command> commands, AccessableObjects accessableObjects)
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
                    if (InterpretationHelp.GetValueOfCommands(new List<Command> { commands[1] }, accessableObjects).BoolValue)
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
                    Var foundLinkableVar = InterpretationHelp.FindVar(commands[1].commandText, accessableObjects, true) ?? throw new InternalInterpreterException("Found variable is null");
                    returnValueFromVar = new(foundLinkableVar.VarValue);
                    returnValueFromVar.comesFromVarValue = foundLinkableVar;
                    return returnValueFromVar;










                default:
                   throw new InternalInterpreterException("Invalid statement for this statement handler");

            }

            



        }
        
    }

    public class UnknownStatementHandler
    {
        public static Value HandleUnknownReturnStatement(List<Command> commands, AccessableObjects accessableObjects)
        {
            // Is probably var

            if (commands.Count != 1)
            {
                Value foundValue = InterpretationHelp.GetValueOfListUsingIndex(commands.GetRange(1, commands.Count - 1), (Var)(accessableObjects.accessableVars[commands[0].commandText.ToLower()] ?? throw new CodeSyntaxException($"Unknown return statement \"{commands[0].commandText}\"")), accessableObjects);
                if (foundValue.comesFromVarValue != null)
                    return foundValue.comesFromVarValue.VarValue;
                return foundValue;
            }
            double doubleResult;
            int intResult;
            if (commands[0].commandText.Length != 1)
            if (commands[0].commandText.Last() == 'i')
            {
                if (int.TryParse(commands[0].commandText[0..(commands[0].commandText.Length - 1)], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out  intResult))
                {
                    return new(Value.ValueType.@int, intResult);
                }
            }
            else if (commands[0].commandText.Last() == 'n')
            {
                if (double.TryParse(commands[0].commandText[0..(commands[0].commandText.Length - 1)], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out  doubleResult))
                {
                    return new(Value.ValueType.num, doubleResult);
                }
            }



            if (int.TryParse(commands[0].commandText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out  intResult))
            {
                return new(Value.ValueType.@int, intResult);
            }

            if (double.TryParse(commands[0].commandText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out  doubleResult))
            {
                return new(Value.ValueType.num, doubleResult);
            }
            commands[0].commandText = commands[0].commandText.ToLower();
            return ((Var?)accessableObjects.accessableVars[commands[0].commandText] ?? throw new CodeSyntaxException($"Unknown return statement \"{commands[0].commandText}\"")).VarValue;


            //Var not found
        }
    }
}
