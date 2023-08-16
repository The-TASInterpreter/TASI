using System.Text;

namespace TASI.LangCoreHandleInterface
{
    public class StatementInputType
    {


        public readonly string inputName;
        public readonly StatementInput statementInput;
        public enum StatementInput
        {
            FixedStatement, //E.g an if {} else {} statement. The else would be a fixed statement as if {} {} would also work just as well. It doesn't do anything, it just has to be there.
            CodeContainer, //A command.commandTypes code container
            String,//A command.commandTypes string
            Calculation, //A command.commandTypes calculation
            FunctionCall, //A command.commandTypes Function call
            Statement, //A command.commandTypes statement
            ValueReturner //Anything that returns a Value


        }

        public bool IsCommandInputType(Command command)
        {
            switch (statementInput)
            {
                case StatementInput.FixedStatement:
                    if (command.commandType != Command.CommandTypes.Statement)
                        return false;
                    if (!command.commandText.Equals(fixedStatement, StringComparison.CurrentCultureIgnoreCase))
                        return false;
                    return true;
                case StatementInput.CodeContainer:
                    if (command.commandType != Command.CommandTypes.CodeContainer)
                        return false;
                    else
                        return true;
                case StatementInput.String:
                    if (command.commandType != Command.CommandTypes.String)
                        return false;
                    else
                        return true;
                case StatementInput.Calculation:
                    if (command.commandType != Command.CommandTypes.Calculation)
                        return false;
                    else
                        return true;
                case StatementInput.FunctionCall:
                    if (command.commandType != Command.CommandTypes.FunctionCall)
                        return false;
                    else
                        return true;
                case StatementInput.Statement:
                    if (command.commandType != Command.CommandTypes.Statement)
                        return false;
                    else
                        return true;
                case StatementInput.ValueReturner:
                    if (command.commandType == Command.CommandTypes.FunctionCall)
                        return true;
                    if (command.commandType == Command.CommandTypes.String)
                        return true;
                    if (command.commandType == Command.CommandTypes.Statement)
                        return true;
                    if (command.commandType == Command.CommandTypes.Calculation)
                        return true;
                    return false;
                default:
                    throw new InternalInterpreterException("Unimplemented statement input type " + statementInput);
            }
        }

        public string? fixedStatement;

        public StatementInputType(string fixedStatement, string inputName)
        {
            this.fixedStatement = fixedStatement;
            statementInput = StatementInput.FixedStatement;

            this.inputName = inputName;
        }

        public StatementInputType(StatementInput statementInput, string inputName)
        {
            this.statementInput = statementInput;
            this.inputName = inputName;
        }


    }

    public abstract class StatementInput
    {
        public List<List<StatementInputType>> PossibleInputs { get; }

        private bool IsReturnStatement { get; }

        public string StatementName { get; }
        public StatementInput(string name, List<List<StatementInputType>> possibleInput, bool isReturnStatement)
        {
            PossibleInputs = possibleInput;
            StatementName = name;
            IsReturnStatement = isReturnStatement;
        }

        public string CorrectUsage
        {
            get
            {
                StringBuilder sb;
                if (IsReturnStatement)
                    sb = new StringBuilder($"Correct usage of \"{StatementName}\" return statement: ");
                else
                    sb = new StringBuilder($"Correct usage of \"{StatementName}\" statement: ");

                for (int i = 0; i < PossibleInputs.Count; i++)
                {


                    if (i != 0)
                    {
                        
                        sb.Append(", or: ");

                    }
                    sb.Append(StatementName);
                    for (int j = 0; j < PossibleInputs[i].Count; j++)
                    {
                        switch (PossibleInputs[i][j].statementInput)
                        {
                            case StatementInputType.StatementInput.FixedStatement:
                                sb.Append(' ');
                                sb.Append(PossibleInputs[i][j].fixedStatement);
                                break;
                            case StatementInputType.StatementInput.CodeContainer:
                                sb.Append(" { < ");
                                sb.Append(PossibleInputs[i][j].inputName);
                                sb.Append(" > }");

                                break;
                            case StatementInputType.StatementInput.String:
                            case StatementInputType.StatementInput.Calculation:
                            case StatementInputType.StatementInput.FunctionCall:
                            case StatementInputType.StatementInput.Statement:
                                sb.Append(" < ");
                                sb.Append(PossibleInputs[i][j].statementInput);
                                sb.Append(": ");
                                sb.Append(PossibleInputs[i][j].inputName);
                                sb.Append(" >");
                                break;
                            case StatementInputType.StatementInput.ValueReturner:
                                sb.Append(" < value: ");
                                sb.Append(PossibleInputs[i][j].inputName);
                                sb.Append(" >");
                                break;
                            default:
                                throw new InternalInterpreterException("Unknown or unaccounted statement input type.");
                        }
                    }
                    if (!IsReturnStatement)
                        sb.Append("; ");
                }
                return sb.ToString();
            }
        }

        public bool IsValidInput(List<Command> input)
        {
            foreach (var possibleInput in PossibleInputs)
            {
                if (input.Count != possibleInput.Count + 1)
                    continue;
                for (int i = 0; i < input.Count - 1; i++)
                {

                    if (!possibleInput[i].IsCommandInputType(input[i + 1]))
                        continue;
                }
                return true;
            }
            return false;
        }

    }
}
