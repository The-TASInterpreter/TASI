using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASI.RuntimeObjects.VarClasses;
using TASI.RuntimeObjects;
using TASI.InitialisationObjects;

namespace TASI.InternalLangCoreHandle
{
    public static class InterpretationHelp
    {
        public static Value HandleReturnStatement(List<Command> returnStatementCommands, AccessableObjects accessableObjects)
        {
            if (!accessableObjects.global.AllNormalReturnStatements.TryGetValue(returnStatementCommands[0].commandText.ToLower(), out ReturnStatement returnStatement))
                return UnknownStatementHandler.HandleUnknownReturnStatement(returnStatementCommands, accessableObjects);
            if (!returnStatement.IsValidInput(returnStatementCommands))
                throw new CodeSyntaxException($"Incorrect usage of statement. {returnStatement.CorrectUsage}");
            return returnStatement.returnStatementHandler.HandleReturnStatement(returnStatementCommands, accessableObjects);
        }
        public static Value HandleStatement(List<Command> returnStatementCommands, AccessableObjects accessableObjects)
        {
            if (!accessableObjects.global.AllNormalStatements.TryGetValue(returnStatementCommands[0].commandText.ToLower(), out Statement statement))
                throw new CodeSyntaxException($"Unknown statement \"{returnStatementCommands[0].commandText}\"");
            if (!statement.IsValidInput(returnStatementCommands))
                throw new CodeSyntaxException($"Incorrect usage of statement. {statement.CorrectUsage}");
            return statement.statementHandler.HandleStatement(returnStatementCommands, accessableObjects);
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
                    Value returnStatementCall = HandleReturnStatement(commands, accessableObjects);
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
                    Value returnStatementCall = HandleReturnStatement(commands, accessableObjects);
                    return returnStatementCall;

                case Command.CommandTypes.String:
                    if (commands.Count != 1) //There shouldnt be anything after a string
                        throw new CodeSyntaxException($"Unexpected {commands[1].commandType} after calculation.");
                    return new(Value.ValueType.@string, commands[0].commandText);

                default:
                    throw new CodeSyntaxException($"Unexpected type ({commands[0].commandType})");
            }
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
