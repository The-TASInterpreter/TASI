using DataTypeStore;

namespace TASI
{
    public class Command
    {
        public string commandText;
        public CommandTypes commandType;
        public int commandLine;
        public string originalCommandText;
        public List<Command>? codeContainerCommands;
        public FunctionCall? functionCall;
        public CalculationType? calculation;

        public FunctionCall FunctionCall
        {
            get
            {
                if (functionCall == null)
                {
                    if (commandType == CommandTypes.FunctionCall)
                    {
                        throw new InternalInterpreterException("Function call of function token was null.");
                    }
                    else
                    {
                        throw new InternalInterpreterException("Trying to access a function call of a non function-token");
                    }
                }
                return functionCall;
            }
        }
        public CalculationType Calculation
        {
            get
            {
                if (calculation == null)
                {
                    if (commandType == CommandTypes.Calculation)
                    {
                        throw new InternalInterpreterException("Calculation of calculation token was null.");
                    }
                    else
                    {
                        throw new InternalInterpreterException("Trying to access a calculation of a non calculation token");
                    }
                }
                return calculation;
            }
        }

        public void initCodeContainerFunctions(NamespaceInfo namespaceInfo)
        {
            foreach (Command command in codeContainerCommands)
            {
                if (command.commandType == CommandTypes.FunctionCall) command.FunctionCall.SearchCallFunction(namespaceInfo);
                if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(namespaceInfo);
                if (command.commandType == CommandTypes.Calculation) command.Calculation.InitFunctions(namespaceInfo);

            }
        }
        public Region Region
        {
            get
            {
                Region result = new("Cmd", new List<Region>(), new());
                result.directValues.Add(new("cTxt", commandText, false));
                result.directValues.Add(new("cL", commandLine.ToString(), false));
                result.directValues.Add(new("cTyp", commandType.ToString(), false));
                if (codeContainerCommands != null)
                    foreach (Command c in codeContainerCommands)
                        result.SubRegions.Add(c.Region);
                if (functionCall != null)
                    result.SubRegions.Add(functionCall.Region);
                return result;


            }
        }
        public Command(Region region)
        {
            commandText = region.FindDirectValue("cTXT").value;
            commandLine = Convert.ToInt32(region.FindDirectValue("cL").value);
            commandType = Enum.Parse<CommandTypes>(region.FindDirectValue("cTyp").value);
            List<Region> codeContainerCommandsCheck = region.FindSubregionWithNameArray("Cmd").ToList();
            if (codeContainerCommandsCheck.Any())
            {
                codeContainerCommands = new();
                foreach (Region region1 in codeContainerCommandsCheck)
                    codeContainerCommands.Add(new(region1));

            }
            List<Region> functionCallRegionCheck = region.FindSubregionWithNameArray("MC").ToList();
            if (functionCallRegionCheck.Any())
            {
                functionCall = new(functionCallRegionCheck[0]);
            }
        }


        public enum CommandTypes
        {
            FunctionCall, Statement, Calculation, String, CodeContainer, EndCommand
        }
        public Command(CommandTypes commandType, string commandText, int commandLine = -1)
        {
            this.commandText = commandText;
            this.commandType = commandType;
            this.commandLine = commandLine;
            switch (commandType)
            {
                case CommandTypes.FunctionCall:
                    this.functionCall = new(this);
                    originalCommandText = $"[{commandText}]";
                    break;
                case CommandTypes.Calculation:
                    originalCommandText = $"({commandText})";
                    this.calculation = new(this);
                    break;
                case CommandTypes.String:
                    originalCommandText = $"\"{commandText}\"";
                    break;
                case CommandTypes.CodeContainer:
                    this.codeContainerCommands = StringProcess.ConvertLineToCommand(commandText, commandLine);

                    originalCommandText = "{" + commandText + "}";
                    break;
                default:
                    originalCommandText = commandText;
                    break;
            }
        }
    }
}
