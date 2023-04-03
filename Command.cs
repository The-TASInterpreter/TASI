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

        public enum CommandTypes
        {
            MethodCall, Statement, NumCalculation, String, CodeContainer, EndCommand
        }
        public Command(CommandTypes commandType, string commandText, int commandLine)
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
                    this.codeContainerCommands = StringProcess.ConvertLineToCommand(commandText);

                    originalCommandText = "{" + commandText + "}";
                    break;
                default:
                    originalCommandText = commandText;
                    break;
            }
        }
        public Command(CommandTypes commandType, string commandText)
        {
            this.commandText = commandText;
            this.commandType = commandType;
            commandLine = -1;
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
                    this.codeContainerCommands = StringProcess.ConvertLineToCommand(commandText);

                    originalCommandText = "{" + commandText + "}";
                    break;
                default:
                    originalCommandText = commandText;
                    break;
            }


        }
    }
}
