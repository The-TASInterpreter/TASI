

namespace TASI
{
    public class Command
    {
        public string commandText;
        public CommandTypes commandType;
        public int commandLine;
        public int commandEnd;
        public string originalCommandText;
        public List<Command>? codeContainerCommands;
        public FunctionCall? functionCall;
        public CalculationType? calculation;

        public void initCodeContainerFunctions(NamespaceInfo namespaceInfo)
        {
            foreach(Command command in codeContainerCommands)
            {
                if (command.commandType == CommandTypes.FunctionCall) command.functionCall.SearchCallFunction(namespaceInfo);
                if (command.commandType == CommandTypes.CodeContainer) command.initCodeContainerFunctions(namespaceInfo);
                if (command.commandType == CommandTypes.Calculation) command.calculation.InitFunctions(namespaceInfo);

            }
        }


        
        public enum CommandTypes
        {
            FunctionCall, Statement, Calculation, String, CodeContainer, EndCommand
        }
        /// <summary>
        /// For creating code containers
        /// </summary>
        /// <param name="codeContainerCommands"></param>
        /// <param name="commandLine"></param>
        /// <param name="commandEnd"></param>
        public Command(List<Command> codeContainerCommands, int commandLine = - 1)
        {
            commandType = CommandTypes.CodeContainer;
            commandText = string.Empty;
            this.commandLine = commandLine;
            if (codeContainerCommands.Any())
            {
                commandEnd = codeContainerCommands.Last().commandEnd;
            }
            else
            {
                commandEnd = commandLine;
            }

            
            this.codeContainerCommands = codeContainerCommands;
            
        }

        /// <summary>
        /// General purpose command creation
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandLine"></param>
        /// <param name="commandEnd"></param>
        public Command(CommandTypes commandType, string commandText, int commandLine = -1, int commandEnd = -1)
        {
            this.commandText = commandText;
            this.commandType = commandType;
            this.commandLine = commandLine;
            this.commandEnd = commandEnd;
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
