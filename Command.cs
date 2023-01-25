namespace Text_adventure_Script_Interpreter
{
    public class Command
    {
        public string commandText;
        public CommandTypes commandType;
        public long commandLine;
        public enum CommandTypes
        {
            VoidMethod, BoolMethod, StringMethod, NumMethod, UnknownMethod, Statement, NumCalculation, String, Brace, EndCommand
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

    public class MethodCall
    {
        public Method callMethod;
        public List<Var> inputVars;
        public MethodCall(Method callMethod, List<Var> inputVars)
        {
            this.callMethod = callMethod;
            this.inputVars = new List<Var>(  inputVars);
        }
    }
}
