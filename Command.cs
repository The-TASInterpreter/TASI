namespace TASI
{
    public class Command
    {
        public string commandText;
        public CommandTypes commandType;
        public long commandLine;
        public string originalCommandText;
        public enum CommandTypes
        {
            MethodCall, Statement, NumCalculation, String, CodeContainer, EndCommand
        }
        public Command(CommandTypes commandType, string commandText, long commandLine)
        {
            this.commandText = commandText;
            this.commandType = commandType;
            this.commandLine = commandLine;
        }
        public Command(CommandTypes commandType, string commandText)
        {
            this.commandText = commandText;
            this.commandType = commandType;
            commandLine = -1;
            switch (commandType)
            {
                case CommandTypes.MethodCall:
                    originalCommandText = $"[{commandText}]";
                    break;
                case CommandTypes.NumCalculation:
                    originalCommandText = $"({commandText})";
                    break;
                case CommandTypes.String:
                    originalCommandText = $"\"{commandText}\"";
                    break;
                case CommandTypes.CodeContainer:
                    originalCommandText = "{" + commandText + "}";
                    break;
                default:
                    originalCommandText = commandText;
                    break;
            }


        }
    }
}
