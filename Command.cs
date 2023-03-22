namespace TASI
{
    public class Command
    {
        public string commandText;
        public CommandTypes commandType;
        public long commandLine;
        public enum CommandTypes
        {
            VoidMethod, BoolMethod, StringMethod, NumMethod, UnknownMethod, Statement, NumCalculation, String, CodeContainer, EndCommand
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
        }
    }
}
