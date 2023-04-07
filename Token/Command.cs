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
        public MethodCall? methodCall;


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
                if (methodCall != null)
                    result.SubRegions.Add(methodCall.Region);
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
            List<Region> methodCallRegionCheck = region.FindSubregionWithNameArray("MC").ToList();
            if (methodCallRegionCheck.Any())
            {
                methodCall = new(methodCallRegionCheck[0]);
            }
        }

        
        public enum CommandTypes
        {
            MethodCall, Statement, NumCalculation, String, CodeContainer, EndCommand
        }
        public Command(CommandTypes commandType, string commandText, int commandLine = -1)
        {
            this.commandText = commandText;
            this.commandType = commandType;
            this.commandLine = commandLine;
            switch (commandType)
            {
                case CommandTypes.MethodCall:
                    this.methodCall = new(this);
                    originalCommandText = $"[{commandText}]";
                    break;
                case CommandTypes.NumCalculation:
                    originalCommandText = $"({commandText})";
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
